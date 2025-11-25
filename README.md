# 🎛️ AerionDeck

**The Open Source, Cross-Platform Control Deck for Power Users.**

AerionDeck transforms any device into a powerful remote control interface for your PC. Built with performance and extensibility in mind, it serves as a software-based alternative to hardware stream decks.

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey)
![Stack](https://img.shields.io/badge/stack-.NET%208%20%7C%20AvaloniaUI-purple)

## 🚀 Features (Roadmap)

- **Cross-Platform Core:** Runs natively on Windows, Linux, and macOS using AvaloniaUI.
- **Zero Latency:** Instant execution via local WebSockets (SignalR).
- **Mobile Companion:** Control your desktop from any phone via a PWA (no app store required).
- **Plugin System:** Extend functionality with C# DLLs.
- **Aerion UI:** Clean, minimalist aesthetics matching the AerionOS ecosystem.

## 🛠️ Architecture

AerionDeck is built on a **Hybrid Desktop Architecture**:

1.  **Desktop Client (AvaloniaUI):** Handles the UI, configuration, and OS native interop (PInvoke/Bash).
2.  **Embedded Server (ASP.NET Core Kestrel):** Runs silently in the background to serve the mobile interface.
3.  **SignalR Hub:** Manages real-time bidirectional communication between the mobile deck and the host PC.

## ⚡ Getting Started

### Prerequisites
* .NET 8.0 SDK

### Running Locally

1.  Clone the repository:
    ```bash
    git clone [https://github.com/JuansesDev/AerionDeck.git](https://github.com/JuansesDev/AerionDeck.git)
    ```
2.  Navigate to the project:
    ```bash
    cd AerionDeck
    ```
3.  Run the desktop application:
    ```bash
    dotnet run --project AerionDeck.Desktop
    ```

## 🤝 Contributing

We welcome contributions! This project follows standard C# coding conventions.

## 📄 License

This project is licensed under the MIT License.

---
**Part of the Aerion Ecosystem.**