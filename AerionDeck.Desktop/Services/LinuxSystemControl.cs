using System;
using System.Diagnostics;

namespace AerionDeck.Desktop.Services;

/// <summary>
/// Implementación de control del sistema para Linux
/// </summary>
public class LinuxSystemControl : ISystemControl
{
    public void ToggleMute()
    {
        if (ExecuteCommand("pactl", "set-sink-mute @DEFAULT_SINK@ toggle"))
        {
            Console.WriteLine("✅ [Linux] Mute/Unmute successful with pactl.");
            return;
        }

        if (ExecuteCommand("amixer", "set Master toggle"))
        {
            Console.WriteLine("✅ [Linux] Mute/Unmute successful with amixer.");
            return;
        }

        Console.WriteLine("❌ [Linux] ERROR: Both pactl and amixer commands failed.");
    }

    public void VolumeUp(int percent = 5)
    {
        if (ExecuteCommand("pactl", $"set-sink-volume @DEFAULT_SINK@ +{percent}%"))
        {
            Console.WriteLine($"✅ [Linux] Volume +{percent}%");
            return;
        }

        if (ExecuteCommand("amixer", $"set Master {percent}%+"))
        {
            Console.WriteLine($"✅ [Linux] Volume +{percent}%");
            return;
        }

        Console.WriteLine("❌ [Linux] ERROR: Volume up failed.");
    }

    public void VolumeDown(int percent = 5)
    {
        if (ExecuteCommand("pactl", $"set-sink-volume @DEFAULT_SINK@ -{percent}%"))
        {
            Console.WriteLine($"✅ [Linux] Volume -{percent}%");
            return;
        }

        if (ExecuteCommand("amixer", $"set Master {percent}%-"))
        {
            Console.WriteLine($"✅ [Linux] Volume -{percent}%");
            return;
        }

        Console.WriteLine("❌ [Linux] ERROR: Volume down failed.");
    }

    public void ToggleMicMute()
    {
        if (ExecuteCommand("pactl", "set-source-mute @DEFAULT_SOURCE@ toggle"))
        {
            Console.WriteLine("✅ [Linux] Mic mute toggled with pactl.");
            return;
        }

        if (ExecuteCommand("amixer", "set Capture toggle"))
        {
            Console.WriteLine("✅ [Linux] Mic mute toggled with amixer.");
            return;
        }

        Console.WriteLine("❌ [Linux] ERROR: Mic mute toggle failed.");
    }

    public void MuteMic()
    {
        if (ExecuteCommand("pactl", "set-source-mute @DEFAULT_SOURCE@ 1"))
        {
            Console.WriteLine("✅ [Linux] Mic muted.");
            return;
        }

        if (ExecuteCommand("amixer", "set Capture nocap"))
        {
            Console.WriteLine("✅ [Linux] Mic muted with amixer.");
            return;
        }

        Console.WriteLine("❌ [Linux] ERROR: Mic mute failed.");
    }

    public void UnmuteMic()
    {
        if (ExecuteCommand("pactl", "set-source-mute @DEFAULT_SOURCE@ 0"))
        {
            Console.WriteLine("✅ [Linux] Mic unmuted.");
            return;
        }

        if (ExecuteCommand("amixer", "set Capture cap"))
        {
            Console.WriteLine("✅ [Linux] Mic unmuted with amixer.");
            return;
        }

        Console.WriteLine("❌ [Linux] ERROR: Mic unmute failed.");
    }

    public bool LaunchApplication(string appName, string arguments = "")
    {
        Console.WriteLine($"🚀 Launching: {appName} {arguments}");
        
        // Intentar ejecutar directamente
        if (ExecuteCommandAsync(appName, arguments))
        {
            return true;
        }
        
        // Si falla, intentar con gtk-launch (para aplicaciones .desktop)
        if (ExecuteCommandAsync("gtk-launch", appName))
        {
            return true;
        }
        
        // Último intento: xdg-open (para URLs y algunos tipos de archivos)
        if (ExecuteCommandAsync("xdg-open", appName))
        {
            return true;
        }
        
        Console.WriteLine($"❌ [Linux] ERROR: Could not launch {appName}");
        return false;
    }

    public void LaunchMultipleApplications(string[] appNames)
    {
        foreach (var app in appNames)
        {
            var trimmed = app.Trim();
            if (!string.IsNullOrEmpty(trimmed))
            {
                // Separar comando de argumentos
                var parts = trimmed.Split(' ', 2);
                var appName = parts[0];
                var args = parts.Length > 1 ? parts[1] : "";
                
                LaunchApplication(appName, args);
            }
        }
    }

    public bool RunCommand(string command, string arguments = "")
    {
        return ExecuteCommand(command, arguments);
    }

    /// <summary>
    /// Ejecuta un comando de forma asíncrona (no espera a que termine)
    /// Útil para lanzar aplicaciones GUI
    /// </summary>
    private bool ExecuteCommandAsync(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        try
        {
            var process = Process.Start(psi);
            if (process != null)
            {
                Console.WriteLine($"✅ [Linux] Started: {fileName} {arguments}");
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Could not start {fileName}: {ex.Message}");
            return false;
        }
    }

    private bool ExecuteCommand(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardError = true, 
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using var process = Process.Start(psi);
            
            if (process == null) return false;

            process.WaitForExit(2000); 

            if (process.ExitCode == 0)
            {
                return true; 
            }
            else
            {
                string error = process.StandardError.ReadToEnd();
                Console.WriteLine($"⚠️ {fileName} failed (Exit Code {process.ExitCode}): {error.Trim()}");
                return false;
            }
        }
        catch (System.ComponentModel.Win32Exception ex)
        {
            Console.WriteLine($"❌ Error: Command '{fileName}' not found. ({ex.Message})");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error executing {fileName}: {ex.Message}");
            return false;
        }
    }
}