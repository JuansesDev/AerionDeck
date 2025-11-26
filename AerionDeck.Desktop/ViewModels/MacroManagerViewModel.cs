using System;
using System.Collections.ObjectModel;
using System.Linq;
using AerionDeck.Desktop.Models;
using AerionDeck.Desktop.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AerionDeck.Desktop.ViewModels;

public partial class MacroManagerViewModel : ViewModelBase
{
    private readonly MacroManagerService _macroManager;

    public ObservableCollection<Macro> Macros => _macroManager.Macros;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsMacroSelected))]
    private Macro? _selectedMacro;

    public bool IsMacroSelected => SelectedMacro != null;

    // Available types for the dropdown
    public ObservableCollection<MacroActionType> AvailableMacroTypes { get; }

    public MacroManagerViewModel(MacroManagerService macroManager)
    {
        _macroManager = macroManager;
        AvailableMacroTypes = new ObservableCollection<MacroActionType>(
            Enum.GetValues(typeof(MacroActionType)).Cast<MacroActionType>()
        );
    }

    [RelayCommand]
    private void AddMacro()
    {
        var newMacro = new Macro
        {
            Name = "New Macro",
            Description = "Description..."
        };
        _macroManager.AddMacro(newMacro);
        SelectedMacro = newMacro;
    }

    [RelayCommand]
    private void DeleteMacro()
    {
        if (SelectedMacro != null)
        {
            _macroManager.RemoveMacro(SelectedMacro);
            SelectedMacro = null;
        }
    }

    [RelayCommand]
    private void SaveMacros()
    {
        _macroManager.SaveMacros();
    }

    [RelayCommand]
    private void AddStep()
    {
        if (SelectedMacro == null) return;

        SelectedMacro.Steps.Add(new MacroStep
        {
            Type = MacroActionType.Delay,
            Value = "1000"
        });
        
        // Force UI update (simple way)
        var index = _macroManager.Macros.IndexOf(SelectedMacro);
        if (index != -1)
        {
            // Trigger property changed for the list item if needed, 
            // but for ObservableCollection inside the object, we might need a better binding strategy 
            // or use ObservableCollection for Steps too.
            // For now, let's assume the UI binds to SelectedMacro.Steps directly.
            // Ideally Steps should be ObservableCollection.
        }
        _macroManager.SaveMacros();
    }

    [RelayCommand]
    private void RemoveStep(MacroStep step)
    {
        if (SelectedMacro == null) return;
        SelectedMacro.Steps.Remove(step);
        _macroManager.SaveMacros();
    }
}
