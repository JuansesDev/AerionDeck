using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace AerionDeck.Desktop.Services;

public class LocalizationService : INotifyPropertyChanged
{
    private static LocalizationService? _instance;
    public static LocalizationService Instance => _instance ??= new LocalizationService();

    private Dictionary<string, string> _strings = new();
    private string _currentLanguage = "es";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
                LoadLanguage(value);
                // Refresh all bindings, including indexer
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
            }
        }
    }

    public string this[string key] => _strings.TryGetValue(key, out var value) ? value : key;

    private LocalizationService()
    {
        // Usar la cultura actual si es "es" o "en", si no, por defecto a "es"
        var currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        if (currentCulture == "en")
        {
            LoadLanguage("en");
        }
        else
        {
            LoadLanguage("es");
        }
    }

    private void LoadLanguage(string lang)
    {
        _strings.Clear();
        
        // Dictionary in code to avoid file loading issues for now
        if (lang == "es")
        {
            _strings["AppTitle"] = "AerionDeck";
            _strings["Settings"] = "Configuración";
            _strings["Actions"] = "Acciones";
            _strings["Add"] = "Agregar";
            _strings["Save"] = "Guardar"; // Cadenas faltantes en español
            _strings["Cancel"] = "Cancelar"; // Cadenas faltantes en español
            _strings["Delete"] = "Eliminar"; // Cadena faltante en español
            _strings["Edit"] = "Editar"; // Cadena faltante en español
            _strings["Name"] = "Nombre";
            _strings["Icon"] = "Icono";
            _strings["Color"] = "Color"; // Cadena faltante en español
            _strings["Command"] = "Comando";
            _strings["ActionType"] = "Tipo de acción";
            _strings["Enabled"] = "Habilitada"; // Cadena faltante en español
            _strings["SearchIcon"] = "Buscar icono...";
            _strings["Selected"] = "Seleccionado:";
            _strings["Profile"] = "Perfil";
            _strings["ExportProfile"] = "Exportar perfil";
            _strings["DeleteProfile"] = "Eliminar perfil";
            _strings["NewProfilePlaceholder"] = "Nombre del nuevo perfil...";
            _strings["NoActions"] = "No hay acciones configuradas";
            _strings["NoActionsHint"] = "Presiona 'Agregar' para crear una nueva acción";
            _strings["EditAction"] = "Editar acción";
            _strings["CommandPlaceholder"] = "Ej: code, obs, firefox";
            _strings["MultiLaunchHint"] = "💡 Para abrir varias apps, sepáralas con comas: code, obs, discord";
            _strings["ActionEnabled"] = "Acción habilitada";
            _strings["MacroSequence"] = "Secuencia de acciones";
            _strings["AddStep"] = "Agregar paso";
            _strings["Export"] = "Exportar"; // Cadena faltante en español
            _strings["ServerStart"] = "Iniciar servidor";
            _strings["ServerStop"] = "Detener servidor";
            _strings["ServerStatusStopped"] = "Servidor detenido";
            _strings["ServerStatusStarting"] = "Iniciando servidor...";
            _strings["ServerStatusActive"] = "Servidor activo en";
            _strings["Language"] = "Idioma / Language";
            _strings["NewAction"] = "Nueva Acción";
            _strings["ServerTitle"] = "Servidor"; // Cadena faltante en español
            _strings["MobileConnection"] = "Conexión móvil"; // Cadena faltante en español
            _strings["ScanQrCode"] = "Escanea el código QR con tu teléfono para abrir el panel de control"; // Cadena faltante en español
            _strings["LocalIp"] = "IP Local: {0}"; // Cadena faltante en español
            _strings["LocalTest"] = "Prueba local"; // Cadena faltante en español
            _strings["LocalTestDesc"] = "Probar acciones configuradas directamente"; // Cadena faltante en español
            _strings["NoActionsDesc"] = "Abrir configuración para agregar acciones"; // Cadena faltante en español
        }
        else // English
        {
            _strings["AppTitle"] = "AerionDeck";
            _strings["Settings"] = "Settings";
            _strings["Actions"] = "Actions";
            _strings["Add"] = "Add";
            _strings["Save"] = "Save";
            _strings["Cancel"] = "Cancel";
            _strings["Delete"] = "Delete";
            _strings["Edit"] = "Edit";
            _strings["Name"] = "Name";
            _strings["Icon"] = "Icon";
            _strings["Color"] = "Color";
            _strings["Command"] = "Command";
            _strings["ActionType"] = "Action Type";
            _strings["Enabled"] = "Enabled";
            _strings["SearchIcon"] = "Search icon...";
            _strings["Selected"] = "Selected:";
            _strings["Profile"] = "Profile";
            _strings["ExportProfile"] = "Export Profile";
            _strings["DeleteProfile"] = "Delete Profile";
            _strings["NewProfilePlaceholder"] = "New profile name...";
            _strings["NoActions"] = "No actions configured";
            _strings["NoActionsHint"] = "Press 'Add' to create a new action";
            _strings["EditAction"] = "Edit Action";
            _strings["CommandPlaceholder"] = "Ex: code, obs, firefox";
            _strings["MultiLaunchHint"] = "💡 To open multiple apps, separate with commas: code, obs, discord";
            _strings["ActionEnabled"] = "Action Enabled";
            _strings["MacroSequence"] = "Action Sequence";
            _strings["AddStep"] = "Add Step";
            _strings["Export"] = "Export";
            _strings["ServerStart"] = "Start Server";
            _strings["ServerStop"] = "Stop Server";
            _strings["ServerStatusStopped"] = "Server stopped";
            _strings["ServerStatusStarting"] = "Starting Server...";
            _strings["ServerStatusActive"] = "Server active at";
            _strings["Language"] = "Idioma / Language";
            _strings["NewAction"] = "New Action";
            _strings["ServerTitle"] = "Server";
            _strings["MobileConnection"] = "Mobile Connection";
            _strings["ScanQrCode"] = "Scan the QR code with your phone to open the control panel";
            _strings["LocalIp"] = "Local IP: {0}";
            _strings["LocalTest"] = "Local Test";
            _strings["LocalTestDesc"] = "Test configured actions directly";
            _strings["NoActionsDesc"] = "Open settings to add actions";
        }
    }
}