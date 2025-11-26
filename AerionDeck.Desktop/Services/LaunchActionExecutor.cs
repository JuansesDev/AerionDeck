using System;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

/// <summary>
/// Ejecutor de acciones para lanzar aplicaciones
/// </summary>
public class LaunchActionExecutor : IActionExecutor
{
    private readonly ISystemControl _systemControl;

    public ActionType SupportedType => ActionType.Launch;

    public LaunchActionExecutor(ISystemControl systemControl)
    {
        _systemControl = systemControl;
    }

    public void Execute(DeckAction action)
    {
        if (string.IsNullOrWhiteSpace(action.Command))
        {
            Console.WriteLine("⚠️ No se especificó aplicación para lanzar");
            return;
        }

        // Si el comando contiene comas, son múltiples aplicaciones
        if (action.Command.Contains(','))
        {
            var apps = action.Command.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            Console.WriteLine($"🚀 Lanzando {apps.Length} aplicaciones...");
            _systemControl.LaunchMultipleApplications(apps);
        }
        else
        {
            // Una sola aplicación
            _systemControl.LaunchApplication(action.Command.Trim(), action.Arguments);
        }
    }
}
