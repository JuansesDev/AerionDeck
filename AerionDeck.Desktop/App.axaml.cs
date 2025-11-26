using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using AerionDeck.Desktop.ViewModels;
using AerionDeck.Desktop.Views;

namespace AerionDeck.Desktop;

public partial class App : Application
{
    private MainWindowViewModel? _viewModel;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Crear el ViewModel (él maneja todo el ciclo de vida del servidor)
            _viewModel = new MainWindowViewModel();

            desktop.MainWindow = new MainWindow
            {
                DataContext = _viewModel,
            };

            // Detener el servidor cuando se cierre la app
            desktop.Exit += async (s, e) => 
            {
                if (_viewModel != null)
                {
                    await _viewModel.StopServerAsync();
                }
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}