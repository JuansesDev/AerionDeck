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

    // Localization
    public LocalizationService Localization => LocalizationService.Instance;

    // === Server State ===
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ServerButtonText))]
    [NotifyPropertyChangedFor(nameof(ServerButtonColor))]
    private bool _isServerRunning;

    [ObservableProperty]
    private string _serverStatus;

    // === Derived properties for button ===
    public string ServerButtonText => IsServerRunning ? Localization["ServerStop"] : Localization["ServerStart"];
    public IBrush ServerButtonColor => IsServerRunning 
        ? new SolidColorBrush(Color.Parse("#e63946")) 
        : new SolidColorBrush(Color.Parse("#6366f1"));

    // === Connection Info ===
    public string LocalIpAddress { get; }
    public string MobilePanelUrl { get; }
    public Bitmap? QrCodeImage { get; }

    // === Navigation ===
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentViewTitle))]
    private bool _isSettingsOpen;

    public string CurrentViewTitle => IsSettingsOpen ? Localization["Settings"] : Localization["AppTitle"];

    // === Settings ViewModel ===
    public SettingsViewModel SettingsViewModel { get; }

    // === Local Test Actions ===
    public ObservableCollection<DeckActionViewModel> TestActions { get; } = new();

    public MainWindowViewModel()
    {
        // Initialize state
        _serverStatus = Localization["ServerStatusStopped"];

        // Subscribe to language changes
        Localization.PropertyChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(ServerButtonText));
            OnPropertyChanged(nameof(CurrentViewTitle));
            OnPropertyChanged(nameof(FormattedLocalIpAddress));
            // Update status if not running (if running, it has dynamic message)
            if (!IsServerRunning)
            {
                ServerStatus = Localization["ServerStatusStopped"];
            }
        };

        // Load settings
        _settings = AppSettings.Load();
        
        // Create action registry
        _actionRegistry = new ActionRegistry(_settings);
        
        // Register action executors
        var systemControl = new LinuxSystemControl();
        _actionRegistry.RegisterExecutor(new AudioActionExecutor(systemControl));
        _actionRegistry.RegisterExecutor(new LaunchActionExecutor(systemControl));
        _actionRegistry.RegisterExecutor(new MacroActionExecutor(_actionRegistry));
        _actionRegistry.RegisterExecutor(new DelayActionExecutor());
        
        // Create SettingsViewModel
        SettingsViewModel = new SettingsViewModel(_settings);
        
        // Load local test actions
        RefreshTestActions();
        
        // Configure network info
        LocalIpAddress = EmbeddedWebServer.GetLocalIPAddress();
        MobilePanelUrl = $"http://{LocalIpAddress}:{ServerPort}";
        QrCodeImage = GenerateQrCode(MobilePanelUrl);

        // Auto-start server if configured
        if (_settings.AutoStartServer)
        {
            Task.Run(StartServerAsync);
        }
    }

    public string FormattedLocalIpAddress => string.Format(Localization["LocalIp"], LocalIpAddress);

    private void RefreshTestActions()
    {
        TestActions.Clear();
        foreach (var action in _actionRegistry.GetActions().Where(a => a.IsEnabled).OrderBy(a => a.Order))
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
            // Refresh test actions when closing settings
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
            ServerStatus = Localization["ServerStatusStarting"];
            
            _webServer = new EmbeddedWebServer();
            
            // Configure action provider for API
            _webServer.SetActionsProvider(() => _actionRegistry.GetActions());
            
            // Subscribe to Hub to receive actions
            AerionHub.OnActionReceived += HandleRemoteAction;
            
            // Start in separate thread
            _ = Task.Run(() => _webServer.StartAsync(ServerPort));
            
            // Wait a bit for startup
            await Task.Delay(500);
            
            IsServerRunning = true;
            ServerStatus = $"{Localization["ServerStatusActive"]} {MobilePanelUrl}";
            
            Console.WriteLine($"🚀 Server started: {MobilePanelUrl}");
        }
        catch (Exception ex)
        {
            ServerStatus = $"Error: {ex.Message}";
            Console.WriteLine($"❌ Error starting server: {ex.Message}");
        }
    }

    public async Task StopServerAsync()
    {
        if (!IsServerRunning || _webServer == null) return;

        try
        {
            ServerStatus = Localization["ServerStop"] + "...";
            
            AerionHub.OnActionReceived -= HandleRemoteAction;
            await _webServer.StopAsync();
            
            IsServerRunning = false;
            ServerStatus = Localization["ServerStatusStopped"];
            
            Console.WriteLine("🛑 Server stopped");
        }
        catch (Exception ex)
        {
            ServerStatus = $"Error: {ex.Message}";
            Console.WriteLine($"❌ Error stopping server: {ex.Message}");
        }
    }

    private void HandleRemoteAction(string actionId)
    {
        Console.WriteLine($"📱 Remote action received: {actionId}");
        _actionRegistry.ExecuteAction(actionId);
    }

    private Bitmap? GenerateQrCode(string url)
    {
        try
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            
            // Inverted colors: dark background, light code
            var qrCodeBytes = qrCode.GetGraphic(10, new byte[] { 255, 255, 255 }, new byte[] { 26, 26, 46 });
            
            using var stream = new MemoryStream(qrCodeBytes);
            return new Bitmap(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error generating QR: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Gets ActionRegistry for external use
    /// </summary>
    public ActionRegistry GetActionRegistry() => _actionRegistry;
    
    /// <summary>
    /// Gets Settings
    /// </summary>
    public AppSettings GetSettings() => _settings;
}