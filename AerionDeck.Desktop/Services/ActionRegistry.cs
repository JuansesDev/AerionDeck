using System;
using System.Collections.Generic;
using System.Linq;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

/// <summary>
/// Interfaz para ejecutores de acciones específicas
/// </summary>
public interface IActionExecutor
{
    ActionType SupportedType { get; }
    void Execute(DeckAction action);
}

/// <summary>
/// Registro central de acciones - gestiona y ejecuta todas las acciones del deck
/// </summary>
public class ActionRegistry
{
    private readonly Dictionary<ActionType, IActionExecutor> _executors = new();
    private readonly AppSettings _settings;

    public ActionRegistry(AppSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Registra un ejecutor para un tipo de acción
    /// </summary>
    public void RegisterExecutor(IActionExecutor executor)
    {
        _executors[executor.SupportedType] = executor;
        Console.WriteLine($"📝 Registrado ejecutor para: {executor.SupportedType}");
    }

    /// <summary>
    /// Ejecuta una acción por su ID
    /// </summary>
    public bool ExecuteAction(string actionId)
    {
        var action = _settings.Actions.FirstOrDefault(a => a.Id == actionId);
        
        if (action == null)
        {
            Console.WriteLine($"⚠️ Acción no encontrada: {actionId}");
            return false;
        }

        if (!action.IsEnabled)
        {
            Console.WriteLine($"⚠️ Acción deshabilitada: {action.Name}");
            return false;
        }

        return ExecuteAction(action);
    }

    /// <summary>
    /// Ejecuta una acción directamente
    /// </summary>
    public bool ExecuteAction(DeckAction action)
    {
        if (!_executors.TryGetValue(action.Type, out var executor))
        {
            Console.WriteLine($"❌ No hay ejecutor registrado para: {action.Type}");
            return false;
        }

        try
        {
            Console.WriteLine($"🎯 Ejecutando: {action.Name} ({action.Type})");
            executor.Execute(action);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error ejecutando {action.Name}: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Obtiene todas las acciones configuradas
    /// </summary>
    public IReadOnlyList<DeckAction> GetActions() => _settings.Actions.AsReadOnly();

    /// <summary>
    /// Obtiene acciones de una carpeta específica (o raíz si parentId es null)
    /// </summary>
    public IEnumerable<DeckAction> GetActionsInFolder(string? parentId = null)
    {
        return _settings.Actions
            .Where(a => a.ParentFolderId == parentId)
            .OrderBy(a => a.Order);
    }

    /// <summary>
    /// Agrega una nueva acción
    /// </summary>
    public void AddAction(DeckAction action)
    {
        _settings.Actions.Add(action);
        _settings.Save();
    }

    /// <summary>
    /// Elimina una acción por ID
    /// </summary>
    public bool RemoveAction(string actionId)
    {
        var action = _settings.Actions.FirstOrDefault(a => a.Id == actionId);
        if (action != null)
        {
            _settings.Actions.Remove(action);
            _settings.Save();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Actualiza una acción existente
    /// </summary>
    public void UpdateAction(DeckAction action)
    {
        var index = _settings.Actions.FindIndex(a => a.Id == action.Id);
        if (index >= 0)
        {
            _settings.Actions[index] = action;
            _settings.Save();
        }
    }
}
