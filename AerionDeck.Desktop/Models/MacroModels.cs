using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AerionDeck.Desktop.Models;

public enum MacroActionType
{
    RunCommand,
    SendKeys,
    Delay,
    ChangeScene,
    OBS_SwitchScene
}

public class MacroStep
{
    public MacroActionType Type { get; set; }
    public string Value { get; set; } = string.Empty;
}

public class Macro
{
    public string Id { get; set; } = System.Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ObservableCollection<MacroStep> Steps { get; set; } = new ObservableCollection<MacroStep>();
}
