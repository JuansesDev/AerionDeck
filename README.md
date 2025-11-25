<div align="center">

# AerionDeck

**The Open Source, Cross-Platform Control Deck for Power Users**

<br/>

[![License](https://img.shields.io/badge/license-MIT-blue.svg?style=flat-square)](LICENSE)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey?style=flat-square)](#)
[![Stack](https://img.shields.io/badge/stack-.NET%208%20%7C%20AvaloniaUI-purple?style=flat-square)](#)

<br/>

> **Alpha Release Coming Soon**  
> The first alpha version of AerionDeck is currently in development.  
> Pre-built binaries for **Windows**, **Linux**, and **macOS** will be available shortly.

</div>

---

AerionDeck transforms any device into a powerful remote control interface for your PC. Built for performance and extensibility, it serves as a software-based alternative to hardware stream decks.

Works natively on **Windows**, **Linux**, and **macOS** — no emulation, no compatibility layers.

---

## Table of Contents

- [Features](#features)
- [Download](#download)
- [Architecture](#architecture)
- [Building from Source](#building-from-source)
- [Contributing](#contributing)
- [Contributors](#contributors)
- [License](#license)

---

## Features

| Feature | Description |
|---------|-------------|
| **Cross-Platform** | Native support for Windows, Linux, and macOS via AvaloniaUI |
| **Zero Latency** | Instant execution using local WebSockets (SignalR) |
| **Mobile Companion** | Control your desktop from any phone via a PWA — no app store required |
| **Plugin System** | Extend functionality with C# DLLs |
| **Aerion UI** | Clean, minimalist aesthetics matching the AerionOS ecosystem |

---

## Download

Pre-built binaries will be available for:

| Platform | Status |
|----------|--------|
| Windows (x64) | Coming soon |
| Linux (x64) | Coming soon |
| macOS (x64 / ARM) | Coming soon |

Stay tuned for the first **alpha release**.

---

## Architecture

AerionDeck uses a **Hybrid Desktop Architecture**:

| Component | Description |
|-----------|-------------|
| **Desktop Client** | AvaloniaUI-based app handling UI, configuration, and OS native interop |
| **Embedded Server** | ASP.NET Core Kestrel serving the mobile interface in the background |
| **SignalR Hub** | Real-time, bidirectional communication between the mobile deck and host PC |

---

## Building from Source

If you prefer to build AerionDeck yourself:

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Instructions

```bash
# Clone the repository
git clone https://github.com/JuansesDev/AerionDeck.git

# Navigate to the project directory
cd AerionDeck

# Run the desktop application
dotnet run --project AerionDeck.Desktop
```

---

## Contributing

Contributions are welcome. Please follow standard C# coding conventions and feel free to open issues or submit pull requests.

---

## Contributors

<a href="https://github.com/JuansesDev/AerionDeck/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=JuansesDev/AerionDeck" alt="Contributors" />
</a>

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

<div align="center">

**Part of the Aerion Ecosystem**

</div>

</div>