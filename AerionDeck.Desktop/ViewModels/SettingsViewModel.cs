using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using AerionDeck.Desktop.Converters;
using AerionDeck.Desktop.Models;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AerionDeck.Desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly AppSettings _settings;
    
    // Converters estáticos para XAML
    public static IValueConverter EnabledOpacityConverter { get; } = new BoolToOpacityConverter();
    public static IValueConverter EnabledTextConverter { get; } = new BoolToEnabledTextConverter();
    
    // Evento para solicitar abrir archivo
    public event Action? RequestFileOpen;

    [ObservableProperty]
    private ObservableCollection<DeckActionViewModel> _actions = new();

    [ObservableProperty]
    private DeckActionViewModel? _selectedAction;

    [ObservableProperty]
    private bool _isEditing;

    // Para edición
    [ObservableProperty]
    private string _editName = "";
    
    [ObservableProperty]
    private string _editIcon = "";
    
    [ObservableProperty]
    private string _editCommand = "";
    
    [ObservableProperty]
    private string _editBackgroundColor = "#0055aa";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredIconOptions))]
    private string _iconSearchText = "";

    [ObservableProperty]
    private bool _editIsEnabled = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AvailableCommands))]
    [NotifyPropertyChangedFor(nameof(IsLaunchType))]
    private ActionType _editActionType = ActionType.Audio;

    // Tipo de acciones disponibles
    public ActionType[] AvailableActionTypes { get; } = new[]
    {
        ActionType.Audio,
        ActionType.Launch
    };

    // Si es tipo Launch, mostrar campo de texto libre
    public bool IsLaunchType => EditActionType == ActionType.Launch;

    // Comandos disponibles según el tipo de acción
    public string[] AvailableCommands => EditActionType switch
    {
        ActionType.Audio => new[]
        {
            "toggle-mute",
            "volume-up",
            "volume-down",
            "toggle-mic",
            "mic-mute",
            "mic-unmute"
        },
        ActionType.Launch => new[]
        {
            "code",
            "obs",
            "firefox",
            "google-chrome",
            "discord",
            "spotify",
            "nautilus",
            "gnome-terminal"
        },
        _ => Array.Empty<string>()
    };

    // Iconos disponibles (usando el nuevo sistema)
    public IEnumerable<IconOption> FilteredIconOptions => 
        string.IsNullOrWhiteSpace(IconSearchText) 
            ? Converters.AvailableIcons.All 
            : Converters.AvailableIcons.All.Where(i => 
                i.Name.Contains(IconSearchText, StringComparison.OrdinalIgnoreCase) || 
                i.Id.Contains(IconSearchText, StringComparison.OrdinalIgnoreCase));

    // Colores sugeridos
    public string[] SuggestedColors { get; } = new[]
    {
        "#e63946", "#f4a261", "#2a9d8f", "#6366f1", "#8b5cf6", 
        "#ec4899", "#10b981", "#f59e0b", "#64748b", "#1e293b"
    };

    public SettingsViewModel(AppSettings settings)
    {
        _settings = settings;
        LoadActions();
    }

    private void LoadActions()
    {
        Actions.Clear();
        foreach (var action in _settings.Actions.OrderBy(a => a.Order))
        {
            Actions.Add(new DeckActionViewModel(action));
        }
    }

    [RelayCommand]
    private void MoveUp(DeckActionViewModel? action)
    {
        var target = action ?? SelectedAction;
        if (target == null) return;
        
        var index = Actions.IndexOf(target);
        if (index > 0)
        {
            Actions.Move(index, index - 1);
            UpdateOrders();
            SaveChanges();
        }
    }

    [RelayCommand]
    private void MoveDown(DeckActionViewModel? action)
    {
        var target = action ?? SelectedAction;
        if (target == null) return;
        
        var index = Actions.IndexOf(target);
        if (index < Actions.Count - 1)
        {
            Actions.Move(index, index + 1);
            UpdateOrders();
            SaveChanges();
        }
    }

    [RelayCommand]
    private void ToggleEnabled(DeckActionViewModel? action)
    {
        var target = action ?? SelectedAction;
        if (target == null) return;
        
        target.IsEnabled = !target.IsEnabled;
        target.Model.IsEnabled = target.IsEnabled;
        SaveChanges();
    }

    [RelayCommand]
    private void EditAction(DeckActionViewModel? action)
    {
        var target = action ?? SelectedAction;
        if (target == null) return;
        
        SelectedAction = target;
        EditName = target.Name;
        EditIcon = target.Icon;
        EditActionType = target.Model.Type;
        EditCommand = target.Model.Command;
        EditBackgroundColor = target.BackgroundColor;
        EditIsEnabled = target.IsEnabled;
        IconSearchText = ""; // Reset search
        IsEditing = true;
    }

    [RelayCommand]
    private void BrowseFile()
    {
        RequestFileOpen?.Invoke();
    }

    [RelayCommand]
    private void SaveEdit()
    {
        if (SelectedAction == null) return;

        SelectedAction.Name = EditName;
        SelectedAction.Icon = EditIcon;
        SelectedAction.BackgroundColor = EditBackgroundColor;
        SelectedAction.IsEnabled = EditIsEnabled;
        
        // Actualizar el modelo
        SelectedAction.Model.Name = EditName;
        SelectedAction.Model.Icon = EditIcon;
        SelectedAction.Model.Type = EditActionType;
        SelectedAction.Model.Command = EditCommand;
        SelectedAction.Model.BackgroundColor = EditBackgroundColor;
        SelectedAction.Model.IsEnabled = EditIsEnabled;

        IsEditing = false;
        SaveChanges();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
    }

    [RelayCommand]
    private void AddAction()
    {
        var newAction = new DeckAction
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Nueva Acción",
            Icon = "lightning",
            Type = ActionType.Audio,
            Command = "toggle-mute",
            BackgroundColor = "#6366f1",
            Order = Actions.Count,
            IsEnabled = true
        };

        _settings.Actions.Add(newAction);
        var vm = new DeckActionViewModel(newAction);
        Actions.Add(vm);
        SelectedAction = vm;
        SaveChanges();
        
        // Abrir editor automáticamente
        EditAction(vm);
    }

    [RelayCommand]
    private void DeleteAction(DeckActionViewModel? action)
    {
        var target = action ?? SelectedAction;
        if (target == null) return;

        var model = target.Model;
        _settings.Actions.Remove(model);
        Actions.Remove(target);
        SelectedAction = null;
        UpdateOrders();
        SaveChanges();
    }

    private void UpdateOrders()
    {
        for (int i = 0; i < Actions.Count; i++)
        {
            Actions[i].Model.Order = i;
        }
    }

    private void SaveChanges()
    {
        _settings.Save();
        Console.WriteLine("✅ Configuración guardada");
    }
}

/// <summary>
/// ViewModel para cada acción individual (para binding en la UI)
/// </summary>
public partial class DeckActionViewModel : ObservableObject
{
    public DeckAction Model { get; }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _icon;

    [ObservableProperty]
    private string _backgroundColor;

    [ObservableProperty]
    private bool _isEnabled;

    public DeckActionViewModel(DeckAction model)
    {
        Model = model;
        _name = model.Name;
        _icon = model.Icon;
        _backgroundColor = model.BackgroundColor;
        _isEnabled = model.IsEnabled;
    }
}

// Converters inline
public class BoolToOpacityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool enabled && enabled ? 1.0 : 0.4;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class BoolToEnabledTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is bool enabled && enabled ? "" : "OFF";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
