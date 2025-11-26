using System.Collections.Generic;

namespace AerionDeck.Desktop.Models;

public enum MacroActionType
{
    RunCommand,
    SendKeys,
    Delay,
    ChangeScene
}

public class MacroStep
{
    public MacroActionType Type { get; set; }
    public string Value { get; set; } = string.Empty;
}

public class Macro
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<MacroStep> Steps { get; set; } = new List<MacroStep>();
}
