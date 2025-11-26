namespace AerionDeck.Desktop.Services;

/// <summary>
/// Interfaz para control del sistema operativo
/// </summary>
public interface ISystemControl
{
    /// <summary>Alternar mute del audio</summary>
    void ToggleMute();
    
    /// <summary>Subir volumen</summary>
    void VolumeUp(int percent = 5);
    
    /// <summary>Bajar volumen</summary>
    void VolumeDown(int percent = 5);
    
    /// <summary>Alternar mute del micrófono</summary>
    void ToggleMicMute();
    
    /// <summary>Mutear micrófono</summary>
    void MuteMic();
    
    /// <summary>Desmutear micrófono</summary>
    void UnmuteMic();
    
    /// <summary>Ejecutar un comando del sistema</summary>
    bool RunCommand(string command, string arguments = "");
    
    /// <summary>Lanzar una aplicación</summary>
    bool LaunchApplication(string appName, string arguments = "");
    
    /// <summary>Lanzar múltiples aplicaciones</summary>
    void LaunchMultipleApplications(string[] appNames);
}