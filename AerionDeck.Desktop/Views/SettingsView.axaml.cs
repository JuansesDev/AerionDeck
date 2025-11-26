using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using AerionDeck.Desktop.ViewModels;

namespace AerionDeck.Desktop.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is SettingsViewModel vm)
        {
            vm.RequestFileOpen += OnRequestFileOpen;
        }
    }

    private async void OnRequestFileOpen()
    {
        if (DataContext is not SettingsViewModel vm) return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
        {
            Title = "Seleccionar aplicación o ejecutable",
            AllowMultiple = false
        });

        if (files.Count > 0)
        {
            var path = files[0].Path.LocalPath;
            vm.EditCommand = path;
        }
    }

    private void OnIconClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && DataContext is SettingsViewModel vm)
        {
            // El Tag contiene el ID del icono
            vm.EditIcon = button.Tag?.ToString() ?? "lightning";
        }
    }

    private void OnColorClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && DataContext is SettingsViewModel vm)
        {
            // El Background es un IBrush, necesitamos extraer el color
            if (button.Background is Avalonia.Media.SolidColorBrush brush)
            {
                vm.EditBackgroundColor = brush.Color.ToString();
            }
        }
    }
}
