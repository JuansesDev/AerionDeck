using System;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

/// <summary>
/// Ejecutor de acciones de audio (mute, volumen, micrófono, etc.)
/// </summary>
public class AudioActionExecutor : IActionExecutor
{
    private readonly ISystemControl _systemControl;

    public ActionType SupportedType => ActionType.Audio;

    public AudioActionExecutor(ISystemControl systemControl)
    {
        _systemControl = systemControl;
    }

    public void Execute(DeckAction action)
    {
        switch (action.Command.ToLower())
        {
            case "toggle-mute":
            case "mute":
                _systemControl.ToggleMute();
                break;
                
            case "volume-up":
                _systemControl.VolumeUp();
                break;
                
            case "volume-down":
                _systemControl.VolumeDown();
                break;
            
            // Comandos de micrófono
            case "toggle-mic":
            case "mic-toggle":
                _systemControl.ToggleMicMute();
                break;
                
            case "mic-mute":
            case "mute-mic":
                _systemControl.MuteMic();
                break;
                
            case "mic-unmute":
            case "unmute-mic":
                _systemControl.UnmuteMic();
                break;
                
            default:
                Console.WriteLine($"⚠️ Comando de audio desconocido: {action.Command}");
                break;
        }
    }
}
