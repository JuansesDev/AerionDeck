using AerionDeck.Desktop.Models;
using AerionDeck.Desktop.Server;
using AerionDeck.Desktop.Services;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QRCoder;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AerionDeck.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly AppSettings _settings;
    private readonly ActionRegistry _actionRegistry;
    private EmbeddedWebServer? _webServer;
    
    private const int ServerPort = 5000;

    // === Estado del servidor ===
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ServerButtonText))]
    [NotifyPropertyChangedFor(nameof(ServerButtonColor))]
    private bool _isServerRunning;

    [ObservableProperty]
    private string _serverStatus = "Servidor detenido";

    // === Propiedades derivadas para el botón ===
    public string ServerButtonText => IsServerRunning ? "Detener servidor" : "Iniciar servidor";
    public IBrush ServerButtonColor => IsServerRunning 
        ? new SolidColorBrush(Color.Parse("#e63946")) 
        : new SolidColorBrush(Color.Parse("#6366f1"));

    // === Información de conexión ===
    public string LocalIpAddress { get; }
    public string MobilePanelUrl { get; }
    public Bitmap? QrCodeImage { get; }

    // === Navegación ===
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentViewTitle))]
    private bool _isSettingsOpen;

    public string CurrentViewTitle => IsSettingsOpen ? "Configuración" : "AerionDeck";

    // === Settings ViewModel ===
    public SettingsViewModel SettingsViewModel { get; }

    // === Acciones para prueba local ===
    public ObservableCollection<DeckActionViewModel> TestActions { get; } = new();

    public MainWindowViewModel()
    {
        // Cargar configuración
        _settings = AppSettings.Load();
        
        // Crear el registro de acciones
        _actionRegistry = new ActionRegistry(_settings);
        
        // Registrar ejecutores de acciones
        var systemControl = new LinuxSystemControl();
        _actionRegistry.RegisterExecutor(new AudioActionExecutor(systemControl));
        _actionRegistry.RegisterExecutor(new LaunchActionExecutor(systemControl));
        
        // Crear SettingsViewModel
        SettingsViewModel = new SettingsViewModel(_settings);
        
        // Cargar acciones para prueba local
        RefreshTestActions();
        
        // Configurar información de red
        LocalIpAddress = EmbeddedWebServer.GetLocalIPAddress();
        MobilePanelUrl = $"http://{LocalIpAddress}:{ServerPort}";
        QrCodeImage = GenerateQrCode(MobilePanelUrl);

        // Auto-iniciar servidor si está configurado
        if (_settings.AutoStartServer)
        {
            Task.Run(StartServerAsync);
        }
    }

    private void RefreshTestActions()
    {
        TestActions.Clear();
        foreach (var action in _settings.Actions.Where(a => a.IsEnabled).OrderBy(a => a.Order))
        {
            TestActions.Add(new DeckActionViewModel(action));
        }
    }

    [RelayCommand]
    private async Task ToggleServerAsync()
    {
        if (IsServerRunning)
        {
            await StopServerAsync();
        }
        else
        {
            await StartServerAsync();
        }
    }

    [RelayCommand]
    private void ToggleSettings()
    {
        if (IsSettingsOpen)
        {
            // Al cerrar settings, refrescar las acciones de prueba
            RefreshTestActions();
        }
        IsSettingsOpen = !IsSettingsOpen;
    }

    [RelayCommand]
    private void ExecuteAction(string actionId)
    {
        _actionRegistry.ExecuteAction(actionId);
    }

    public async Task StartServerAsync()
    {
        if (IsServerRunning) return;

        try
        {
            ServerStatus = "Iniciando servidor...";
            
            _webServer = new EmbeddedWebServer();
            
            // Configurar el proveedor de acciones para la API
            _webServer.SetActionsProvider(() => _actionRegistry.GetActions());
            
            // Suscribirse al Hub para recibir acciones
            AerionHub.OnActionReceived += HandleRemoteAction;
            
            // Iniciar en un hilo separado
            _ = Task.Run(() => _webServer.StartAsync(ServerPort));
            
            // Esperar un poco para que inicie
            await Task.Delay(500);
            
            IsServerRunning = true;
            ServerStatus = $"Servidor activo en {MobilePanelUrl}";
            
            Console.WriteLine($"🚀 Servidor iniciado: {MobilePanelUrl}");
        }
        catch (Exception ex)
        {
            ServerStatus = $"Error: {ex.Message}";
            Console.WriteLine($"❌ Error iniciando servidor: {ex.Message}");
        }
    }

    public async Task StopServerAsync()
    {
        if (!IsServerRunning || _webServer == null) return;

        try
        {
            ServerStatus = "Deteniendo servidor...";
            
            AerionHub.OnActionReceived -= HandleRemoteAction;
            await _webServer.StopAsync();
            
            IsServerRunning = false;
            ServerStatus = "Servidor detenido";
            
            Console.WriteLine("🛑 Servidor detenido");
        }
        catch (Exception ex)
        {
            ServerStatus = $"Error: {ex.Message}";
            Console.WriteLine($"❌ Error deteniendo servidor: {ex.Message}");
        }
    }

    private void HandleRemoteAction(string actionId)
    {
        Console.WriteLine($"📱 Acción remota recibida: {actionId}");
        _actionRegistry.ExecuteAction(actionId);
    }

    private Bitmap? GenerateQrCode(string url)
    {
        try
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            
            // Colores invertidos: fondo oscuro, código claro
            var qrCodeBytes = qrCode.GetGraphic(10, new byte[] { 255, 255, 255 }, new byte[] { 26, 26, 46 });
            
            using var stream = new MemoryStream(qrCodeBytes);
            return new Bitmap(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error generando QR: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Obtiene el ActionRegistry para uso externo
    /// </summary>
    public ActionRegistry GetActionRegistry() => _actionRegistry;
    
    /// <summary>
    /// Obtiene la configuración
    /// </summary>
    public AppSettings GetSettings() => _settings;
}