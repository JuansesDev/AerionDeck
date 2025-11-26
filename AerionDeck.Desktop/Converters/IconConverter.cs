using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace AerionDeck.Desktop.Converters;

/// <summary>
/// Convierte identificadores de iconos a StreamGeometry para usar en PathIcon.
/// También maneja conversiones especiales con parámetros.
/// </summary>
public class IconConverter : IValueConverter
{
    public static readonly IconConverter Instance = new();

    private static readonly Dictionary<string, StreamGeometry> IconMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // Audio
        ["volume-mute"] = Icons.VolumeMute,
        ["volume-up"] = Icons.VolumeUp,
        ["volume-down"] = Icons.VolumeDown,
        ["volume-high"] = Icons.VolumeHigh,
        ["microphone"] = Icons.Microphone,
        ["microphone-off"] = Icons.MicrophoneOff,
        
        // Sistema
        ["settings"] = Icons.Settings,
        ["server"] = Icons.Server,
        ["play"] = Icons.Play,
        ["stop"] = Icons.Stop,
        ["back"] = Icons.Back,
        
        // Acciones
        ["plus"] = Icons.Plus,
        ["add"] = Icons.Plus,
        ["delete"] = Icons.Delete,
        ["edit"] = Icons.Edit,
        ["up"] = Icons.ChevronUp,
        ["down"] = Icons.ChevronDown,
        ["eye"] = Icons.Eye,
        ["eye-off"] = Icons.EyeOff,
        ["check"] = Icons.Check,
        ["close"] = Icons.Close,
        
        // Conexión
        ["qr-code"] = Icons.QrCode,
        ["phone"] = Icons.Phone,
        ["link"] = Icons.Link,
        
        // Media
        ["record"] = Icons.Record,
        ["camera"] = Icons.Camera,
        ["monitor"] = Icons.Monitor,
        ["gamepad"] = Icons.Gamepad,
        
        // Misc
        ["palette"] = Icons.Palette,
        ["lightning"] = Icons.Lightning,
        ["keyboard"] = Icons.Keyboard,
        ["web"] = Icons.Web,
        
        // Apps & Files
        ["folder"] = Icons.Folder,
        ["terminal"] = Icons.Terminal,
        ["code"] = Icons.Code,
        ["image"] = Icons.Image,
        ["music"] = Icons.Music,
        ["discord"] = Icons.Discord,
        ["spotify"] = Icons.Spotify,
    };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var param = parameter as string;
        
        // Handle special conversion modes based on parameter
        if (!string.IsNullOrEmpty(param))
        {
            return param switch
            {
                // Boolean -> Play/Stop icon
                "playStop" when value is bool isRunning => isRunning ? Icons.Stop : Icons.Play,
                
                // Boolean -> Status color
                "statusColor" when value is bool isRunning => isRunning ? Color.Parse("#10b981") : Color.Parse("#555"),
                
                // Boolean -> Eye icon
                "eye" when value is bool isEnabled => isEnabled ? Icons.Eye : Icons.EyeOff,
                
                // Boolean -> Eye color
                "eyeColor" when value is bool isEnabled => isEnabled 
                    ? new SolidColorBrush(Color.Parse("#10b981"))
                    : new SolidColorBrush(Color.Parse("#666")),
                
                _ => ConvertIconId(value)
            };
        }
        
        return ConvertIconId(value);
    }

    private static object ConvertIconId(object? value)
    {
        if (value is string iconId && IconMap.TryGetValue(iconId, out var geometry))
        {
            return geometry;
        }
        
        // Fallback a lightning si el icono no existe
        return Icons.Lightning;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Proporciona lista de iconos disponibles para selección
/// </summary>
public static class AvailableIcons
{
    public static readonly List<IconOption> All = new()
    {
        new("volume-mute", "Silenciar"),
        new("volume-up", "Volumen +"),
        new("volume-down", "Volumen -"),
        new("volume-high", "Volumen Alto"),
        new("microphone", "Micrófono"),
        new("microphone-off", "Mic. Apagado"),
        new("play", "Reproducir"),
        new("stop", "Detener"),
        new("record", "Grabar"),
        new("camera", "Cámara"),
        new("monitor", "Monitor"),
        new("gamepad", "Gamepad"),
        new("keyboard", "Teclado"),
        new("web", "Web"),
        new("link", "Enlace"),
        new("settings", "Config"),
        new("lightning", "Rayo"),
        new("folder", "Carpeta"),
        new("terminal", "Terminal"),
        new("code", "Código"),
        new("image", "Imagen"),
        new("music", "Música"),
        new("discord", "Discord"),
        new("spotify", "Spotify"),
    };
}

public record IconOption(string Id, string Name);
