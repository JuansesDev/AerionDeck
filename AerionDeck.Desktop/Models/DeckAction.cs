using System;

namespace AerionDeck.Desktop.Models;

/// <summary>
/// Tipos de acciones soportadas por AerionDeck
/// </summary>
public enum ActionType
{
    /// <summary>Controles de audio del sistema</summary>
    Audio,
    
    /// <summary>Ejecutar aplicación o comando</summary>
    Launch,
    
    /// <summary>Enviar combinación de teclas (hotkey)</summary>
    Hotkey,
    
    /// <summary>Abrir URL en el navegador</summary>
    WebLink,
    
    /// <summary>Carpeta que contiene más acciones</summary>
    Folder,
    
    /// <summary>Acción personalizada (plugin futuro)</summary>
    Custom
}

/// <summary>
/// Representa una acción configurable del deck
/// </summary>
public class DeckAction
{
    /// <summary>Identificador único de la acción</summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>Nombre visible de la acción</summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Identificador del icono (ej: "volume-mute", "volume-up")</summary>
    public string Icon { get; set; } = "lightning";
    
    /// <summary>Tipo de acción</summary>
    public ActionType Type { get; set; } = ActionType.Custom;
    
    /// <summary>Comando o parámetro principal de la acción</summary>
    public string Command { get; set; } = string.Empty;
    
    /// <summary>Argumentos adicionales si aplica</summary>
    public string Arguments { get; set; } = string.Empty;
    
    /// <summary>Color de fondo del botón (hex)</summary>
    public string BackgroundColor { get; set; } = "#0055aa";
    
    /// <summary>Si la acción está habilitada</summary>
    public bool IsEnabled { get; set; } = true;
    
    /// <summary>Orden de visualización</summary>
    public int Order { get; set; } = 0;
    
    /// <summary>ID de la carpeta padre (null si está en raíz)</summary>
    public string? ParentFolderId { get; set; }
}
