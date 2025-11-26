using System;
using System.Threading.Tasks;

namespace AerionDeck.Desktop.Services;

/// <summary>
/// Service to control OBS Studio via WebSocket.
/// Simulates the OBS-WebSocket-NET library.
/// </summary>
public class ObsControlService
{
    private SimulatedObsClient? _client;
    private bool _isConnected;

    public bool IsConnected => _isConnected;

    public async Task ConnectAsync(string url, string password)
    {
        if (_isConnected) return;

        try
        {
            Console.WriteLine($"🔌 Connecting to OBS at {url}...");
            _client = new SimulatedObsClient(url, password);
            await _client.ConnectAsync();
            _isConnected = true;
            Console.WriteLine("✅ Connected to OBS");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ OBS Connection Failed: {ex.Message}");
            _isConnected = false;
        }
    }

    public async Task SwitchSceneAsync(string sceneName)
    {
        if (!_isConnected || _client == null)
        {
            Console.WriteLine("⚠️ Cannot switch scene: Not connected to OBS");
            return;
        }

        try
        {
            Console.WriteLine($"🎬 Switching OBS Scene to: {sceneName}");
            await _client.SetCurrentProgramSceneAsync(sceneName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Failed to switch scene: {ex.Message}");
        }
    }
}

// --- SIMULATED LIBRARY (Mock) ---
// This represents the external NuGet package 'Obs.WebSocket.Client'
public class SimulatedObsClient
{
    private readonly string _url;
    private readonly string _password;

    public SimulatedObsClient(string url, string password)
    {
        _url = url;
        _password = password;
    }

    public async Task ConnectAsync()
    {
        // Simulate network delay
        await Task.Delay(500);
        
        // In a real scenario, this would throw if connection fails
        if (string.IsNullOrEmpty(_url)) throw new ArgumentException("Invalid URL");
    }

    public async Task SetCurrentProgramSceneAsync(string sceneName)
    {
        // Simulate network delay
        await Task.Delay(100);
        
        // Simulate command execution
        Console.WriteLine($"[OBS-MOCK] Sent SetCurrentProgramScene: {sceneName}");
    }
}
