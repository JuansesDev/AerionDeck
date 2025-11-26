using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using AerionDeck.Desktop.Converters;
using AerionDeck.Desktop.Models;
using AerionDeck.Desktop.Services;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AerionDeck.Desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly AppSettings _settings;
    
    // Localization
    public LocalizationService Localization => LocalizationService.Instance;

    public class LanguageOption
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
    }

    public LanguageOption[] Languages { get; } = new[]
    {
        new LanguageOption { Code = "es", Name = "Español" },
        new LanguageOption { Code = "en", Name = "English" }
    };

    public LanguageOption SelectedLanguage
    {
        get => Languages.FirstOrDefault(l => l.Code == Localization.CurrentLanguage) ?? Languages[0];
        set
        {
            if (value != null)
            {
                Localization.CurrentLanguage = value.Code;
                OnPropertyChanged();
            }
        }
    }

    // Static Converters for XAML
    public static IValueConverter EnabledOpacityConverter { get; } = new BoolToOpacityConverter();
    public static IValueConverter EnabledTextConverter { get; } = new BoolToEnabledTextConverter();
    
    // Event to request file open
    public event Action? RequestFileOpen;

    [ObservableProperty]
    private ObservableCollection<DeckActionViewModel> _actions = new();



    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredIconOptions))]
    private string _iconSearchText = "";

    // For editing
    [ObservableProperty]
    private string _editName = "";
    
    [ObservableProperty]
    private string _editIcon = "";

    [ObservableProperty]
    private ActionType _editActionType;

    [ObservableProperty]
    private string _editCommand = "";

    [ObservableProperty]
    private string _editArguments = "";

    [ObservableProperty]
    private string _editBackgroundColor = "";

    [ObservableProperty]
    private bool _editIsEnabled;
    
    private readonly MacroManagerService _macroManager;

    public ObservableCollection<ActionType> AvailableActionTypes { get; }

    // === Macro Manager ===
    public MacroManagerViewModel MacroManager { get; }

    // === Selection State ===
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsActionSelected))]
    [NotifyPropertyChangedFor(nameof(EditName))]
    [NotifyPropertyChangedFor(nameof(EditIcon))]
    [NotifyPropertyChangedFor(nameof(EditActionType))]
    [NotifyPropertyChangedFor(nameof(EditCommand))]
    [NotifyPropertyChangedFor(nameof(EditArguments))]
    [NotifyPropertyChangedFor(nameof(EditBackgroundColor))]
    [NotifyPropertyChangedFor(nameof(IsMacroType))]
    [NotifyPropertyChangedFor(nameof(SelectedMacroId))]
    private DeckActionViewModel? _selectedAction;

    public bool IsActionSelected => SelectedAction != null;
    public bool IsMacroType => EditActionType == ActionType.Macro;

    // === Macro Selection for Action ===
    public ObservableCollection<Macro> AvailableMacros => _macroManager.Macros;
    
    public string? SelectedMacroId
    {
        get => SelectedAction?.Model.MacroId;
        set
        {
            if (SelectedAction != null && SelectedAction.Model.MacroId != value)
            {
                SelectedAction.Model.MacroId = value;
                OnPropertyChanged();
                SaveEdit();
            }
        }
    }

    public SettingsViewModel(AppSettings settings, MacroManagerService macroManager)
    {
        _settings = settings;
        _macroManager = macroManager;
        MacroManager = new MacroManagerViewModel(_macroManager);

        AvailableActionTypes = new ObservableCollection<ActionType>(
            Enum.GetValues(typeof(ActionType)).Cast<ActionType>()
        );
        LoadProfiles();
    } // Helper properties for UI visibility
    public bool IsLaunchType => EditActionType == ActionType.Launch;
    public bool IsAudioType => EditActionType == ActionType.Audio;

    // Available commands based on action type
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

    // Available icons (using new system)
    public IEnumerable<IconOption> FilteredIconOptions => 
        string.IsNullOrWhiteSpace(IconSearchText) 
            ? Converters.AvailableIcons.All 
            : Converters.AvailableIcons.All.Where(i => 
                i.Name.Contains(IconSearchText, StringComparison.OrdinalIgnoreCase) || 
                i.Id.Contains(IconSearchText, StringComparison.OrdinalIgnoreCase));

    // Suggested colors
    public string[] SuggestedColors { get; } = new[]
    {
        "#e63946", "#f4a261", "#2a9d8f", "#6366f1", "#8b5cf6", 
        "#ec4899", "#10b981", "#f59e0b", "#64748b", "#1e293b"
    };

    [ObservableProperty]
    private ObservableCollection<Profile> _profiles = new();

    [ObservableProperty]
    private Profile? _selectedProfile;

    [ObservableProperty]
    private string _newProfileName = "";



    private void LoadProfiles()
    {
        Profiles.Clear();
        foreach (var profile in _settings.Profiles)
        {
            Profiles.Add(profile);
        }

        var current = Profiles.FirstOrDefault(p => p.Id == _settings.CurrentProfileId);
        SelectedProfile = current ?? Profiles.FirstOrDefault();
        
        if (SelectedProfile != null)
        {
            LoadActions();
        }
    }

    partial void OnSelectedProfileChanged(Profile? value)
    {
        if (value != null)
        {
            _settings.CurrentProfileId = value.Id;
            LoadActions();
            SaveChanges();
        }
    }

    private void LoadActions()
    {
        Actions.Clear();
        if (SelectedProfile == null) return;

        foreach (var action in SelectedProfile.Actions.OrderBy(a => a.Order))
        {
            Actions.Add(new DeckActionViewModel(action));
        }
    }

    [RelayCommand]
    private void AddProfile()
    {
        if (string.IsNullOrWhiteSpace(NewProfileName)) return;

        var profile = new Profile
        {
            Name = NewProfileName,
            Actions = new List<DeckAction>()
        };

        _settings.Profiles.Add(profile);
        Profiles.Add(profile);
        SelectedProfile = profile;
        NewProfileName = "";
        SaveChanges();
    }

    [RelayCommand]
    private void DeleteProfile()
    {
        if (SelectedProfile == null || Profiles.Count <= 1) return;

        var profileToRemove = SelectedProfile;
        var index = Profiles.IndexOf(profileToRemove);
        
        // Select another profile before deleting
        SelectedProfile = Profiles[index == 0 ? 1 : index - 1];
        
        _settings.Profiles.Remove(profileToRemove);
        Profiles.Remove(profileToRemove);
        SaveChanges();
    }

    [RelayCommand]
    private void ExportProfile()
    {
        if (SelectedProfile == null) return;
        
        try 
        {
            var json = JsonSerializer.Serialize(SelectedProfile, new JsonSerializerOptions { WriteIndented = true });
            var fileName = $"profile_{SelectedProfile.Name}_{DateTime.Now:yyyyMMdd}.json";
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
            File.WriteAllText(path, json);
            Console.WriteLine($"Perfil exportado a: {path}");
            // TODO: Show notification to user
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exportando perfil: {ex.Message}");
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
        EditArguments = target.Model.Arguments;
        EditBackgroundColor = target.BackgroundColor;
        EditIsEnabled = target.IsEnabled;
        IconSearchText = ""; // Reset search
        
        // Load sub-actions if macro
        MacroSubActions.Clear();
        if (target.Model.MacroActions != null)
        {
            foreach (var subAction in target.Model.MacroActions)
            {
                MacroSubActions.Add(new DeckActionViewModel(subAction));
            }
        }

        IsEditing = true;
    }

    [RelayCommand]
    private void BrowseFile()
    {
        RequestFileOpen?.Invoke();
    }

    // === Macro Editing ===
    [ObservableProperty]
    private ObservableCollection<DeckActionViewModel> _macroSubActions = new();



    partial void OnEditActionTypeChanged(ActionType value)
    {
        OnPropertyChanged(nameof(IsLaunchType));
        OnPropertyChanged(nameof(IsMacroType));
        OnPropertyChanged(nameof(AvailableCommands));
    }

    [RelayCommand]
    private void AddSubAction()
    {
        var newAction = new DeckAction
        {
            Id = Guid.NewGuid().ToString(),
            Name = "New Sub-Action",
            Icon = "lightning",
            Type = ActionType.Audio,
            Command = "toggle-mute",
            BackgroundColor = "#444444",
            IsEnabled = true
        };
        MacroSubActions.Add(new DeckActionViewModel(newAction));
    }

    [RelayCommand]
    private void RemoveSubAction(DeckActionViewModel? action)
    {
        if (action != null)
        {
            MacroSubActions.Remove(action);
        }
    }

    [RelayCommand]
    private void MoveSubActionUp(DeckActionViewModel? action)
    {
        if (action == null) return;
        var index = MacroSubActions.IndexOf(action);
        if (index > 0)
        {
            MacroSubActions.Move(index, index - 1);
        }
    }

    [RelayCommand]
    private void MoveSubActionDown(DeckActionViewModel? action)
    {
        if (action == null) return;
        var index = MacroSubActions.IndexOf(action);
        if (index < MacroSubActions.Count - 1)
        {
            MacroSubActions.Move(index, index + 1);
        }
    }

    [ObservableProperty]
    private bool _isAddingNew;

    [RelayCommand]
    private void SaveEdit()
    {
        if (SelectedAction == null) return;

        SelectedAction.Name = EditName;
        SelectedAction.Icon = EditIcon;
        SelectedAction.BackgroundColor = EditBackgroundColor;
        SelectedAction.IsEnabled = EditIsEnabled;
        
        // Update model
        SelectedAction.Model.Name = EditName;
        SelectedAction.Model.Icon = EditIcon;
        SelectedAction.Model.Type = EditActionType;
        SelectedAction.Model.Command = EditCommand;
        SelectedAction.Model.Arguments = EditArguments;
        SelectedAction.Model.BackgroundColor = EditBackgroundColor;
        SelectedAction.Model.IsEnabled = EditIsEnabled;

        // Save sub-actions if macro
        if (EditActionType == ActionType.Macro)
        {
            SelectedAction.Model.MacroActions = MacroSubActions.Select(vm => vm.Model).ToList();
        }

        IsAddingNew = false;
        IsEditing = false;
        SaveChanges();
    }

    [RelayCommand]
    private void CancelEdit()
    {
        if (IsAddingNew && SelectedAction != null)
        {
            DeleteAction(SelectedAction);
        }
        IsAddingNew = false;
        IsEditing = false;
    }

    [RelayCommand]
    private void AddAction()
    {
        if (SelectedProfile == null) return;

        var newAction = new DeckAction
        {
            Id = Guid.NewGuid().ToString(),
            Name = Localization["NewAction"],
            Icon = "lightning",
            Type = ActionType.Audio,
            Command = "toggle-mute",
            BackgroundColor = "#6366f1",
            Order = Actions.Count,
            IsEnabled = true
        };

        SelectedProfile.Actions.Add(newAction);
        var vm = new DeckActionViewModel(newAction);
        Actions.Add(vm);
        SelectedAction = vm;
        SaveChanges();
        
        IsAddingNew = true;
        // Open editor automatically
        EditAction(vm);
    }

    [RelayCommand]
    private void DeleteAction(DeckActionViewModel? action)
    {
        var target = action ?? SelectedAction;
        if (target == null || SelectedProfile == null) return;

        var model = target.Model;
        SelectedProfile.Actions.Remove(model);
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
/// ViewModel for individual action (for UI binding)
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

// Inline Converters
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
