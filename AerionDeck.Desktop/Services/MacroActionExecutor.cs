using System;
using System.Threading.Tasks;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

public class MacroActionExecutor : IActionExecutor
{
    private readonly ActionRegistry _registry;

    public ActionType SupportedType => ActionType.Macro;

    public MacroActionExecutor(ActionRegistry registry)
    {
        _registry = registry;
    }

    public void Execute(DeckAction action)
    {
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
                
                // Handle Delay explicitly to ensure we wait
                if (subAction.Type == ActionType.Delay)
                {
                    if (int.TryParse(subAction.Command, out int delayMs))
                    {
                        Console.WriteLine($"⏳ Waiting {delayMs}ms...");
                        await Task.Delay(delayMs);
                    }
                    continue;
                }

                // Pequeño delay por defecto entre acciones para asegurar orden
                await Task.Delay(50);
                
                // Ejecutar la sub-acción
                _registry.ExecuteAction(subAction);
            }
        });
    }
}
