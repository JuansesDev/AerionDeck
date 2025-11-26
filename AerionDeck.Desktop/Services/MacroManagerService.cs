using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using AerionDeck.Desktop.Models;

namespace AerionDeck.Desktop.Services;

public class MacroManagerService
{
    private const string MacrosFileName = "macros.json";
    private readonly string _filePath;

    public ObservableCollection<Macro> Macros { get; private set; } = new();

    public MacroManagerService()
    {
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MacrosFileName);
        LoadMacros();
    }

    private void LoadMacros()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                var loadedMacros = JsonSerializer.Deserialize<List<Macro>>(json);
                if (loadedMacros != null)
                {
                    Macros = new ObservableCollection<Macro>(loadedMacros);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error loading macros: {ex.Message}");
        }

        // Ensure at least one example macro exists if empty
        if (Macros.Count == 0)
        {
            var example = MacroExecutionService.GetExampleMacro();
            example.Id = Guid.NewGuid().ToString(); // Ensure ID
            Macros.Add(example);
            SaveMacros();
        }
    }

    public void SaveMacros()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(Macros, options);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("✅ Macros saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error saving macros: {ex.Message}");
        }
    }

    public void AddMacro(Macro macro)
    {
        Macros.Add(macro);
        SaveMacros();
    }

    public void RemoveMacro(Macro macro)
    {
        Macros.Remove(macro);
        SaveMacros();
    }

    public Macro? GetMacro(string id)
    {
        return Macros.FirstOrDefault(m => m.Id == id);
    }
}
