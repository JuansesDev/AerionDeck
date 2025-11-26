using System;
using System.Collections.Generic;

namespace AerionDeck.Desktop.Models;

/// <summary>
/// Supported action types in AerionDeck
/// </summary>
public enum ActionType
{
    /// <summary>System audio controls</summary>
    Audio,
    
    /// <summary>Launch application or command</summary>
    Launch,
    
    /// <summary>Send key combination (hotkey)</summary>
    Hotkey,
    
    /// <summary>Open URL in browser</summary>
    WebLink,
    
    /// <summary>Folder containing more actions</summary>
    Folder,
    
    /// <summary>Sequence of actions</summary>
    Macro,

    /// <summary>Delay execution (milliseconds)</summary>
    Delay,

    /// <summary>Switch OBS Scene</summary>
    OBS_SwitchScene,
    
    /// <summary>Custom action (future plugin)</summary>
    Custom
}

/// <summary>
/// Represents a configurable deck action
/// </summary>
public class DeckAction
{
    /// <summary>Unique action identifier</summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    /// <summary>Visible name of the action</summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Icon identifier (e.g., "volume-mute", "volume-up")</summary>
    public string Icon { get; set; } = "lightning";
    
    /// <summary>Action type</summary>
    public ActionType Type { get; set; } = ActionType.Custom;
    
    /// <summary>Main command or parameter for the action</summary>
    public string Command { get; set; } = string.Empty;
    
    /// <summary>Additional arguments if applicable</summary>
    public string Arguments { get; set; } = string.Empty;
    
    /// <summary>Button background color (hex)</summary>
    public string BackgroundColor { get; set; } = "#0055aa";
    
    /// <summary>Whether the action is enabled</summary>
    public bool IsEnabled { get; set; } = true;
    
    /// <summary>Display order</summary>
    public int Order { get; set; } = 0;
    
    /// <summary>Parent folder ID (null if root)</summary>
    public string? ParentFolderId { get; set; }

    /// <summary>List of actions for macros (Legacy/Quick Macros)</summary>
    public List<DeckAction> MacroActions { get; set; } = new();

    /// <summary>ID of the linked Advanced Macro</summary>
    public string? MacroId { get; set; }
}
