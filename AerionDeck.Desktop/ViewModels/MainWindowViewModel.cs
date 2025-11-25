using AerionDeck.Desktop.Services;
using ReactiveUI;
using System.Windows.Input;

namespace AerionDeck.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    // Referencia a nuestro servicio (usando la interfaz)
    private readonly ISystemControl _systemControl;

    public string Greeting => "Bienvenido a AerionDeck";

    // El comando que ejecutará el botón
    public ICommand MuteCommand { get; }

    public MainWindowViewModel()
    {
        // TODO: En el futuro, esto se inyectará automáticamente (Dependency Injection)
        // Por ahora, instanciamos la versión de Linux manualmente.
        _systemControl = new LinuxSystemControl();

        // Configuramos el comando para que ejecute nuestro método
        MuteCommand = ReactiveCommand.Create(ToggleAudio);
    }

    private void ToggleAudio()
    {
        _systemControl.ToggleMute();
    }
}