using System;
using System.Threading.Tasks;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

public class DelayActionExecutor : IActionExecutor
{
    public ActionType SupportedType => ActionType.Delay;

    public void Execute(DeckAction action)
    {
        // Delay is typically handled within a macro context (awaited).
        // However, if executed standalone, we just wait (though it won't block UI if fire-and-forget).
        // Ideally, MacroActionExecutor should handle the await.
        // But to fit the IActionExecutor pattern, we can implement it here.
        
        if (int.TryParse(action.Command, out int delayMs))
        {
            // Blocking here would be bad if called on UI thread, but Execute is usually fire-and-forget or background.
            // Since IActionExecutor.Execute is void, we can't await it properly in the caller unless we change the interface.
            // BUT, MacroActionExecutor calls _registry.ExecuteAction(subAction).
            // If we want the macro to PAUSE, MacroActionExecutor needs to know it's a delay and await it.
            
            // Wait! The current MacroActionExecutor calls _registry.ExecuteAction(subAction) which is void.
            // It won't wait for this delay to finish!
            
            // So, MacroActionExecutor needs special handling for Delay, OR IActionExecutor needs to return Task.
            // Changing IActionExecutor to return Task is a bigger refactor.
            
            // Alternative: Handle Delay explicitly in MacroActionExecutor.
            // But for completeness, I'll leave this class here, even if it just sleeps.
            Task.Delay(delayMs).Wait(); 
        }
    }
}
