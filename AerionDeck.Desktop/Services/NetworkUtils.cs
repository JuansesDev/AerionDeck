using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace AerionDeck.Desktop.Services;

public static class NetworkUtils
{
    public static string GetLocalIpAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
        }
        catch (Exception)
        {
            // Fallback
        }
        return "127.0.0.1";
    }
}
