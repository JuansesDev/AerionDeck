using System.Diagnostics;

namespace AerionDeck.Desktop.Services;

public class LinuxSystemControl : ISystemControl
{
    public void ToggleMute()
{
    try
    {
        // Opción A: Usar 'pactl' (Estándar moderno para PulseAudio/PipeWire)
        var psi = new ProcessStartInfo
        {
            FileName = "pactl",
            Arguments = "set-sink-mute @DEFAULT_SINK@ toggle",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process.Start(psi);
        System.Console.WriteLine("🔊 [Linux] Toggle Mute enviado a PulseAudio.");
    }
    catch (System.Exception ex)
    {
        System.Console.WriteLine($"❌ Error: {ex.Message}");
    }
}
}