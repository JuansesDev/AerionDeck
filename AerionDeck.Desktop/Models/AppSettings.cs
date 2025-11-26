using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AerionDeck.Desktop.Models;

/// <summary>
/// Configuración persistente de la aplicación
/// </summary>
public class AppSettings
{
    private static readonly string ConfigPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AerionDeck",
        "settings.json"
    );

    /// <summary>Puerto del servidor web</summary>
    public int ServerPort { get; set; } = 5000;
    
    /// <summary>Iniciar servidor automáticamente al abrir la app</summary>
    public bool AutoStartServer { get; set; } = false;
    
    /// <summary>Minimizar a bandeja del sistema al cerrar</summary>
    public bool MinimizeToTray { get; set; } = false;
    
    /// <summary>Iniciar con el sistema</summary>
    public bool StartWithSystem { get; set; } = false;
    
    /// <summary>Lista de perfiles configurados</summary>
    public List<Profile> Profiles { get; set; } = new();

    /// <summary>ID del perfil actual</summary>
    public string CurrentProfileId { get; set; } = "";

    /// <summary>
    /// Propiedad temporal para migración de versiones anteriores
    /// </summary>
    public List<DeckAction>? Actions { get; set; }

    /// <summary>
    /// Carga la configuración desde disco
    /// </summary>
    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json) ?? CreateDefault();
                
                // Migración: Si hay acciones sueltas pero no perfiles, moverlas a un perfil default
                if (settings.Profiles.Count == 0)
                {
                    var defaultProfile = new Profile
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Default",
                        Actions = settings.Actions ?? new List<DeckAction>()
                    };
                    settings.Profiles.Add(defaultProfile);
                    settings.CurrentProfileId = defaultProfile.Id;
                    settings.Actions = null; // Limpiar lista antigua
                }
                
                return settings;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error cargando configuración: {ex.Message}");
        }
        
        return CreateDefault();
    }

    /// <summary>
    /// Guarda la configuración a disco
    /// </summary>
    public void Save()
    {
        try
        {
            var directory = Path.GetDirectoryName(ConfigPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(ConfigPath, json);
            
            Console.WriteLine("✅ Configuración guardada");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error guardando configuración: {ex.Message}");
        }
    }

    /// <summary>
    /// Crea configuración por defecto con acciones básicas
    /// </summary>
    private static AppSettings CreateDefault()
    {
        var defaultProfile = new Profile
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Default",
            Actions = new List<DeckAction>
            {
                new()
                {
                    Id = "mute",
                    Name = "Mute",
                    Icon = "volume-mute",
                    Type = ActionType.Audio,
                    Command = "toggle-mute",
                    BackgroundColor = "#e63946",
                    Order = 0
                },
                new()
                {
                    Id = "vol-up",
                    Name = "Vol +",
                    Icon = "volume-up",
                    Type = ActionType.Audio,
                    Command = "volume-up",
                    BackgroundColor = "#2a9d8f",
                    Order = 1
                },
                new()
                {
                    Id = "vol-down",
                    Name = "Vol -",
                    Icon = "volume-down",
                    Type = ActionType.Audio,
                    Command = "volume-down",
                    BackgroundColor = "#2a9d8f",
                    Order = 2
                },
                new()
                {
                    Id = "mic-toggle",
                    Name = "Mic",
                    Icon = "microphone",
                    Type = ActionType.Audio,
                    Command = "toggle-mic",
                    BackgroundColor = "#8b5cf6",
                    Order = 3
                },
                new()
                {
                    Id = "launch-vscode",
                    Name = "VSCode",
                    Icon = "monitor",
                    Type = ActionType.Launch,
                    Command = "code",
                    BackgroundColor = "#0078d4",
                    Order = 4
                },
                new()
                {
                    Id = "launch-obs",
                    Name = "OBS",
                    Icon = "record",
                    Type = ActionType.Launch,
                    Command = "obs",
                    BackgroundColor = "#302E31",
                    Order = 5
                }
            }
        };

        var settings = new AppSettings
        {
            Profiles = new List<Profile> { defaultProfile },
            CurrentProfileId = defaultProfile.Id
        };
        
        settings.Save();
        return settings;
    }
}
