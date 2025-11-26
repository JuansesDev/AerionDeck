using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace AerionDeck.Desktop.Server;

public class AerionHub : Hub
{
    // ESTA LÍNEA DEBE SER ESTÁTICA para que App.axaml.cs pueda suscribirse.
    public static event Action<string>? OnActionReceived; 

    public void SendAction(string actionId)
    {
        System.Console.WriteLine($"⚡ [SignalR] Orden recibida: {actionId}");
        
        // Disparar el evento que está escuchando App.axaml.cs
        OnActionReceived?.Invoke(actionId);
    }
}