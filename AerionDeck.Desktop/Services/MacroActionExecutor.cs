using System;
using System.Threading.Tasks;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

public class MacroActionExecutor : IActionExecutor
{
    private readonly ActionRegistry _registry;
    private readonly ObsControlService _obsService;
    private readonly MacroManagerService _macroManager;

    public ActionType SupportedType => ActionType.Macro;

    public MacroActionExecutor(ActionRegistry registry, ObsControlService obsService, MacroManagerService macroManager)
    {
        _registry = registry;
        _obsService = obsService;
        _macroManager = macroManager;
    }

    public void Execute(DeckAction action)
    {
        // 1. Try to get steps from Advanced Macro System
        if (!string.IsNullOrEmpty(action.MacroId))
        {
            var macro = _macroManager.GetMacro(action.MacroId);
            if (macro != null)
            {
                ExecuteAdvancedMacro(macro);
                return;
            }
            Console.WriteLine($"⚠️ Macro not found with ID: {action.MacroId}");
        }

        // 2. Fallback to Legacy MacroActions
        if (action.MacroActions == null || action.MacroActions.Count == 0)
        {
            Console.WriteLine("⚠️ Macro vacía");
            return;
        }

        // Ejecutar en segundo plano para no bloquear UI
        Task.Run(async () =>
        {
            foreach (var subAction in action.MacroActions)
            {
                if (!subAction.IsEnabled) continue;
                
                // Handle Delay explicitly
                if (subAction.Type == ActionType.Delay)
                {
                    if (int.TryParse(subAction.Command, out int delayMs))
                    {
                        Console.WriteLine($"⏳ Waiting {delayMs}ms...");
                        await Task.Delay(delayMs);
                    }
                    continue;
                }

                // Handle OBS Scene Switch
                if (subAction.Type == ActionType.OBS_SwitchScene)
                {
                    Console.WriteLine($"🎬 Switching OBS Scene: {subAction.Command}");
                    await _obsService.SwitchSceneAsync(subAction.Command);
                    continue;
                }

                // Pequeño delay por defecto entre acciones para asegurar orden
                await Task.Delay(50);
                
                // Ejecutar la sub-acción
                _registry.ExecuteAction(subAction);
            }
        });
    }

    private void ExecuteAdvancedMacro(Macro macro)
    {
        Task.Run(async () =>
        {
            Console.WriteLine($"▶️ Executing Macro: {macro.Name}");
            foreach (var step in macro.Steps)
            {
                await ExecuteStep(step);
            }
        });
    }

    private async Task ExecuteStep(MacroStep step)
    {
        try 
        {
            switch (step.Type)
            {
                case MacroActionType.Delay:
                    if (int.TryParse(step.Value, out int delayMs))
                    {
                        Console.WriteLine($"⏳ Waiting {delayMs}ms...");
                        await Task.Delay(delayMs);
                    }
                    break;
                
                case MacroActionType.RunCommand:
                    Console.WriteLine($"🚀 Launching: {step.Value}");
                    // Create a temporary DeckAction to reuse LaunchActionExecutor logic if possible, 
                    // or just use Process.Start if simple. 
                    // Better to use Registry to keep it consistent.
                    var launchAction = new DeckAction 
                    { 
                        Type = ActionType.Launch, 
                        Command = step.Value,
                        IsEnabled = true 
                    };
                    _registry.ExecuteAction(launchAction);
                    break;

                case MacroActionType.OBS_SwitchScene:
                    Console.WriteLine($"🎬 Switching OBS Scene: {step.Value}");
                    await _obsService.SwitchSceneAsync(step.Value);
                    break;
                
                case MacroActionType.ChangeScene: // Legacy/Alias for OBS?
                     Console.WriteLine($"🎬 Switching OBS Scene (Alias): {step.Value}");
                     await _obsService.SwitchSceneAsync(step.Value);
                     break;

                case MacroActionType.SendKeys:
                    Console.WriteLine($"⌨️ Sending Keys: {step.Value} (Not fully implemented)");
                    // TODO: Implement SendKeys
                    break;
            }

            // Default delay between steps
            await Task.Delay(50);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error executing macro step {step.Type}: {ex.Message}");
        }
    }
}
