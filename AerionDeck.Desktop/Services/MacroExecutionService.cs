using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

public class MacroExecutionService
{
    public async Task ExecuteMacroAsync(Macro macro)
    {
        Console.WriteLine($"▶️ Starting Macro: {macro.Name}");

        foreach (var step in macro.Steps)
        {
            try
            {
                switch (step.Type)
                {
                    case MacroActionType.RunCommand:
                        Console.WriteLine($"   Running command: {step.Value}");
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = step.Value,
                            UseShellExecute = true
                        });
                        break;

                    case MacroActionType.Delay:
                        if (int.TryParse(step.Value, out int delayMs))
                        {
                            Console.WriteLine($"   Waiting: {delayMs}ms");
                            await Task.Delay(delayMs);
                        }
                        break;

                    case MacroActionType.SendKeys:
                        Console.WriteLine($"   Sending keys: {step.Value}");
                        // WARNING: System.Windows.Forms is not supported on Linux/Avalonia.
                        // This code is provided as requested but commented out or wrapped to prevent crashes.
                        
                        if (OperatingSystem.IsWindows())
                        {
                            // Requires reference to System.Windows.Forms which is not available in this project context.
                            // System.Windows.Forms.SendKeys.SendWait(step.Value);
                            Console.WriteLine("   ⚠️ SendKeys requires System.Windows.Forms (Windows only).");
                        }
                        else
                        {
                            Console.WriteLine("   ⚠️ SendKeys is not supported on Linux.");
                        }
                        break;

                    case MacroActionType.ChangeScene:
                        Console.WriteLine($"   Changing Scene to: {step.Value} (Not implemented)");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   ❌ Error executing step {step.Type}: {ex.Message}");
            }
        }

        Console.WriteLine($"✅ Macro '{macro.Name}' completed.");
    }

    /// <summary>
    /// Creates the example macro requested by the user.
    /// </summary>
    public static Macro GetExampleMacro()
    {
        return new Macro
        {
            Key = "example_macro",
            Name = "Notepad Automation",
            Steps = new System.Collections.Generic.List<MacroStep>
            {
                // 1. Open Notepad
                new MacroStep { Type = MacroActionType.RunCommand, Value = "notepad.exe" },
                
                // 2. Wait 2 seconds
                new MacroStep { Type = MacroActionType.Delay, Value = "2000" },
                
                // 3. Type "Hello World!"
                new MacroStep { Type = MacroActionType.SendKeys, Value = "Hello World!" },
                
                // 4. Press Enter
                new MacroStep { Type = MacroActionType.SendKeys, Value = "{ENTER}" },
                
                // 5. Save (Ctrl+S)
                new MacroStep { Type = MacroActionType.SendKeys, Value = "^s" }
            }
        };
    }
}
