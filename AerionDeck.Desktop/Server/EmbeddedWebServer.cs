using AerionDeck.Desktop.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace AerionDeck.Desktop.Server;

public class EmbeddedWebServer
{
    private WebApplication? _app;
    private Func<IReadOnlyList<DeckAction>>? _actionsProvider;
    
    /// <summary>
    /// Configura el proveedor de acciones para la API
    /// </summary>
    public void SetActionsProvider(Func<IReadOnlyList<DeckAction>> provider)
    {
        _actionsProvider = provider;
    }
    
    /// <summary>
    /// Obtiene la IP local de la máquina en la red
    /// </summary>
    public static string GetLocalIPAddress()
    {
        try
        {
            // Buscar interfaces de red activas (no loopback, no virtual)
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                             ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                             !ni.Description.Contains("Virtual", StringComparison.OrdinalIgnoreCase) &&
                             !ni.Description.Contains("vEthernet", StringComparison.OrdinalIgnoreCase));

            foreach (var ni in networkInterfaces)
            {
                var ipProps = ni.GetIPProperties();
                var ipv4 = ipProps.UnicastAddresses
                    .FirstOrDefault(addr => addr.Address.AddressFamily == AddressFamily.InterNetwork &&
                                           !IPAddress.IsLoopback(addr.Address));
                
                if (ipv4 != null)
                {
                    return ipv4.Address.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error obteniendo IP: {ex.Message}");
        }

        return "localhost";
    }

    public async Task StartAsync(int port = 5000)
    {
        var builder = WebApplication.CreateBuilder();

        // 1. Configurar Kestrel (el servidor web) para escuchar en todas las IPs de la red local
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(port); 
        });

        // 2. Agregar servicios de SignalR y CORS (para permitir conexión desde el móvil)
        builder.Services.AddSignalR();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyHeader()
                      .AllowAnyMethod()
                      .SetIsOriginAllowed(_ => true)
                      .AllowCredentials();
            });
        });

        // Limpiar logs para no ensuciar la consola de Avalonia
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        _app = builder.Build();

        // 3. Configurar el pipeline HTTP
        _app.UseCors();
        
        // Ruta para el WebSocket de SignalR
        _app.MapHub<AerionHub>("/aerionhub");

        // API para obtener las acciones configuradas
        _app.MapGet("/api/actions", () =>
        {
            var actions = _actionsProvider?.Invoke() ?? new List<DeckAction>();
            // Solo devolver acciones habilitadas y en la raíz
            var rootActions = actions
                .Where(a => a.IsEnabled && a.ParentFolderId == null)
                .OrderBy(a => a.Order)
                .Select(a => new 
                {
                    a.Id,
                    a.Name,
                    a.Icon,
                    a.BackgroundColor
                });
            return Results.Json(rootActions);
        });

        // Servir archivos estáticos desde wwwroot embebido
        var assembly = Assembly.GetExecutingAssembly();
        var embeddedProvider = new EmbeddedFileProvider(assembly, "AerionDeck.Desktop.wwwroot");
        
        _app.UseDefaultFiles(new DefaultFilesOptions
        {
            FileProvider = embeddedProvider
        });
        
        _app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = embeddedProvider,
            RequestPath = ""
        });

        var localIp = GetLocalIPAddress();
        Console.WriteLine($"🚀 Servidor iniciado en: http://localhost:{port}");
        Console.WriteLine($"📱 Panel móvil disponible en: http://{localIp}:{port}");

        await _app.RunAsync();
    }

    public async Task StopAsync()
    {
        if (_app != null)
        {
            await _app.StopAsync();
            await _app.DisposeAsync();
        }
    }
}