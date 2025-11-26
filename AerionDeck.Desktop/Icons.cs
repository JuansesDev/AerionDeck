using Avalonia;
using Avalonia.Media;

namespace AerionDeck.Desktop;

/// <summary>
/// Iconos SVG centralizados para toda la aplicación
/// </summary>
public static class Icons
{
    // === Sistema ===
    public static readonly StreamGeometry Settings = StreamGeometry.Parse(
        "M12 15.5A3.5 3.5 0 0 1 8.5 12 3.5 3.5 0 0 1 12 8.5a3.5 3.5 0 0 1 3.5 3.5 3.5 3.5 0 0 1-3.5 3.5m7.43-2.53c.04-.32.07-.64.07-.97s-.03-.66-.07-1l2.11-1.63c.19-.15.24-.42.12-.64l-2-3.46c-.12-.22-.39-.31-.61-.22l-2.49 1c-.52-.39-1.06-.73-1.69-.98l-.37-2.65A.506.506 0 0 0 14 2h-4c-.25 0-.46.18-.5.42l-.37 2.65c-.63.25-1.17.59-1.69.98l-2.49-1c-.22-.09-.49 0-.61.22l-2 3.46c-.13.22-.07.49.12.64L4.57 11c-.04.34-.07.67-.07 1s.03.65.07.97l-2.11 1.66c-.19.15-.25.42-.12.64l2 3.46c.12.22.39.3.61.22l2.49-1.01c.52.4 1.06.74 1.69.99l.37 2.65c.04.24.25.42.5.42h4c.25 0 .46-.18.5-.42l.37-2.65c.63-.26 1.17-.59 1.69-.99l2.49 1.01c.22.08.49 0 .61-.22l2-3.46c.12-.22.07-.49-.12-.64l-2.11-1.66Z");

    public static readonly StreamGeometry Server = StreamGeometry.Parse(
        "M4 1h16a1 1 0 0 1 1 1v4a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1m0 8h16a1 1 0 0 1 1 1v4a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1v-4a1 1 0 0 1 1-1m0 8h16a1 1 0 0 1 1 1v4a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1v-4a1 1 0 0 1 1-1M9 5h1V3H9v2m0 8h1v-2H9v2m0 8h1v-2H9v2M5 3v2h2V3H5m0 8v2h2v-2H5m0 8v2h2v-2H5Z");

    public static readonly StreamGeometry Play = StreamGeometry.Parse(
        "M8 5v14l11-7z");

    public static readonly StreamGeometry Stop = StreamGeometry.Parse(
        "M6 6h12v12H6z");

    public static readonly StreamGeometry Back = StreamGeometry.Parse(
        "M20 11H7.83l5.59-5.59L12 4l-8 8 8 8 1.41-1.41L7.83 13H20v-2z");

    // === Audio ===
    public static readonly StreamGeometry VolumeHigh = StreamGeometry.Parse(
        "M14 3.23v2.06c2.89.86 5 3.54 5 6.71s-2.11 5.85-5 6.71v2.06c4.01-.91 7-4.49 7-8.77s-2.99-7.86-7-8.77M16.5 12c0-1.77-1-3.29-2.5-4.03V16c1.5-.71 2.5-2.24 2.5-4M3 9v6h4l5 5V4L7 9H3z");

    public static readonly StreamGeometry VolumeMute = StreamGeometry.Parse(
        "M16.5 12c0-1.77-1-3.29-2.5-4.03v2.21l2.45 2.45c.03-.2.05-.41.05-.63m2.5 0c0 .94-.2 1.82-.54 2.64l1.51 1.51C20.63 14.91 21 13.5 21 12c0-4.28-2.99-7.86-7-8.77v2.06c2.89.86 5 3.54 5 6.71M4.27 3 3 4.27 7.73 9H3v6h4l5 5v-6.73l4.25 4.25c-.67.52-1.42.93-2.25 1.18v2.06c1.38-.31 2.63-.95 3.69-1.81L19.73 21 21 19.73l-9-9L4.27 3M12 4 9.91 6.09 12 8.18V4z");

    public static readonly StreamGeometry VolumeUp = StreamGeometry.Parse(
        "M3 9v6h4l5 5V4L7 9H3m13.5 3c0-1.77-1-3.29-2.5-4.03v8.05c1.5-.71 2.5-2.24 2.5-4.02Z");

    public static readonly StreamGeometry VolumeDown = StreamGeometry.Parse(
        "M5 9v6h4l5 5V4L9 9H5m11.5 3c0-1.77-1-3.29-2.5-4.03v8.05c1.5-.71 2.5-2.24 2.5-4.02Z");

    public static readonly StreamGeometry Microphone = StreamGeometry.Parse(
        "M12 2a3 3 0 0 1 3 3v6a3 3 0 0 1-3 3 3 3 0 0 1-3-3V5a3 3 0 0 1 3-3m7 9c0 3.53-2.61 6.44-6 6.93V21h-2v-3.07c-3.39-.49-6-3.4-6-6.93h2a5 5 0 0 0 5 5 5 5 0 0 0 5-5h2Z");

    public static readonly StreamGeometry MicrophoneOff = StreamGeometry.Parse(
        "M19 11c0 1.19-.34 2.3-.9 3.28l-1.23-1.23c.27-.62.43-1.31.43-2.05H19m-4 .16L9 5.18V5a3 3 0 0 1 3-3 3 3 0 0 1 3 3v6c0 .06 0 .11-.01.16M4.27 3 3 4.27l6.01 6.01V11a3 3 0 0 0 3 3c.23 0 .44-.03.65-.08l1.66 1.66c-.71.33-1.5.52-2.31.52-3.03 0-5.5-2.47-5.5-5.5H5c0 3.53 2.61 6.44 6 6.93V21h2v-3.07c.86-.12 1.67-.39 2.41-.77l3.32 3.32 1.27-1.27L4.27 3Z");

    // === Acciones ===
    public static readonly StreamGeometry Plus = StreamGeometry.Parse(
        "M19 13h-6v6h-2v-6H5v-2h6V5h2v6h6v2z");

    public static readonly StreamGeometry Delete = StreamGeometry.Parse(
        "M19 4h-3.5l-1-1h-5l-1 1H5v2h14M6 19a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V7H6v12z");

    public static readonly StreamGeometry Edit = StreamGeometry.Parse(
        "M20.71 7.04c.39-.39.39-1.04 0-1.41l-2.34-2.34c-.37-.39-1.02-.39-1.41 0l-1.84 1.83 3.75 3.75M3 17.25V21h3.75L17.81 9.93l-3.75-3.75L3 17.25z");

    public static readonly StreamGeometry ChevronUp = StreamGeometry.Parse(
        "M7.41 15.41 12 10.83l4.59 4.58L18 14l-6-6-6 6 1.41 1.41z");

    public static readonly StreamGeometry ChevronDown = StreamGeometry.Parse(
        "M7.41 8.59 12 13.17l4.59-4.58L18 10l-6 6-6-6 1.41 1.59z");

    public static readonly StreamGeometry Eye = StreamGeometry.Parse(
        "M12 9a3 3 0 0 0-3 3 3 3 0 0 0 3 3 3 3 0 0 0 3-3 3 3 0 0 0-3-3m0 8a5 5 0 0 1-5-5 5 5 0 0 1 5-5 5 5 0 0 1 5 5 5 5 0 0 1-5 5m0-12.5C7 4.5 2.73 7.61 1 12c1.73 4.39 6 7.5 11 7.5s9.27-3.11 11-7.5c-1.73-4.39-6-7.5-11-7.5z");

    public static readonly StreamGeometry EyeOff = StreamGeometry.Parse(
        "M11.83 9 15 12.16V12a3 3 0 0 0-3-3h-.17m-4.3.8 1.55 1.55c-.05.21-.08.42-.08.65a3 3 0 0 0 3 3c.22 0 .44-.03.65-.08l1.55 1.55c-.67.33-1.41.53-2.2.53a5 5 0 0 1-5-5c0-.79.2-1.53.53-2.2M2 4.27l2.28 2.28.45.45C3.08 8.3 1.78 10 1 12c1.73 4.39 6 7.5 11 7.5 1.55 0 3.03-.3 4.38-.84l.43.42L19.73 22 21 20.73 3.27 3M12 7a5 5 0 0 1 5 5c0 .64-.13 1.26-.36 1.82l2.93 2.93c1.5-1.25 2.7-2.89 3.43-4.75-1.73-4.39-6-7.5-11-7.5-1.4 0-2.74.25-4 .7l2.17 2.15C10.74 7.13 11.35 7 12 7z");

    public static readonly StreamGeometry Check = StreamGeometry.Parse(
        "M21 7 9 19l-5.5-5.5 1.41-1.41L9 16.17 19.59 5.59 21 7Z");

    public static readonly StreamGeometry Close = StreamGeometry.Parse(
        "M19 6.41 17.59 5 12 10.59 6.41 5 5 6.41 10.59 12 5 17.59 6.41 19 12 13.41 17.59 19 19 17.59 13.41 12 19 6.41z");

    // === Conexión ===
    public static readonly StreamGeometry QrCode = StreamGeometry.Parse(
        "M3 11h2v2H3v-2m8-6h2v4h-2V5m-2 6h4v4h-2v-2H9v-2m6 0h2v2h2v-2h2v2h-2v2h2v4h-2v2h-2v-2h-4v2h-2v-4h4v-2h2v-2h-2v-2m4 8v-4h-2v4h2M15 3h6v6h-6V3m2 2v2h2V5h-2M3 3h6v6H3V3m2 2v2h2V5H5M3 15h6v6H3v-6m2 2v2h2v-2H5Z");

    public static readonly StreamGeometry Phone = StreamGeometry.Parse(
        "M17 19H7V5h10m-5 16a1 1 0 0 1-1-1 1 1 0 0 1 1-1 1 1 0 0 1 1 1 1 1 0 0 1-1 1m4-18H8a3 3 0 0 0-3 3v14a3 3 0 0 0 3 3h8a3 3 0 0 0 3-3V5a3 3 0 0 0-3-3Z");

    public static readonly StreamGeometry Link = StreamGeometry.Parse(
        "M3.9 12c0-1.71 1.39-3.1 3.1-3.1h4V7H7a5 5 0 0 0-5 5 5 5 0 0 0 5 5h4v-1.9H7c-1.71 0-3.1-1.39-3.1-3.1M8 13h8v-2H8v2m9-6h-4v1.9h4c1.71 0 3.1 1.39 3.1 3.1 0 1.71-1.39 3.1-3.1 3.1h-4V17h4a5 5 0 0 0 5-5 5 5 0 0 0-5-5Z");

    // === Media/Streaming ===
    public static readonly StreamGeometry Record = StreamGeometry.Parse(
        "M12 2a10 10 0 0 0-10 10 10 10 0 0 0 10 10 10 10 0 0 0 10-10A10 10 0 0 0 12 2m0 4a6 6 0 0 1 6 6 6 6 0 0 1-6 6 6 6 0 0 1-6-6 6 6 0 0 1 6-6Z");

    public static readonly StreamGeometry Camera = StreamGeometry.Parse(
        "M4 4h3l2-2h6l2 2h3a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2m8 3a5 5 0 0 0-5 5 5 5 0 0 0 5 5 5 5 0 0 0 5-5 5 5 0 0 0-5-5m0 2a3 3 0 0 1 3 3 3 3 0 0 1-3 3 3 3 0 0 1-3-3 3 3 0 0 1 3-3Z");

    public static readonly StreamGeometry Monitor = StreamGeometry.Parse(
        "M21 16V4H3v12h18m0-14a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2h-7v2h2v2H8v-2h2v-2H3a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h18Z");

    public static readonly StreamGeometry Gamepad = StreamGeometry.Parse(
        "M7.97 16 5 19c-.9.9-2.37.9-3.27 0-.9-.9-.9-2.37 0-3.27L5 12.46A6 6 0 0 1 7.97 16M19 5c-.9-.9-2.37-.9-3.27 0L12.46 8.27A6 6 0 0 1 16 11.24l3-3c.9-.9.9-2.37 0-3.27m-8.61 10.61c-1.17 1.17-3.07 1.17-4.24 0-1.17-1.17-1.17-3.07 0-4.24l4.24-4.24c1.17-1.17 3.07-1.17 4.24 0 1.17 1.17 1.17 3.07 0 4.24l-4.24 4.24Z");

    // === Misc ===
    public static readonly StreamGeometry Palette = StreamGeometry.Parse(
        "M17.5 12a1.5 1.5 0 0 1-1.5-1.5 1.5 1.5 0 0 1 1.5-1.5 1.5 1.5 0 0 1 1.5 1.5 1.5 1.5 0 0 1-1.5 1.5m-3-4A1.5 1.5 0 0 1 13 6.5 1.5 1.5 0 0 1 14.5 5 1.5 1.5 0 0 1 16 6.5 1.5 1.5 0 0 1 14.5 8m-5 0A1.5 1.5 0 0 1 8 6.5 1.5 1.5 0 0 1 9.5 5 1.5 1.5 0 0 1 11 6.5 1.5 1.5 0 0 1 9.5 8m-3 4A1.5 1.5 0 0 1 5 10.5 1.5 1.5 0 0 1 6.5 9 1.5 1.5 0 0 1 8 10.5 1.5 1.5 0 0 1 6.5 12M12 3a9 9 0 0 0-9 9 9 9 0 0 0 9 9c.83 0 1.5-.67 1.5-1.5 0-.39-.15-.74-.39-1.01-.23-.26-.38-.61-.38-.99 0-.83.67-1.5 1.5-1.5H16a5 5 0 0 0 5-5c0-4.42-4.03-8-9-8Z");

    public static readonly StreamGeometry Lightning = StreamGeometry.Parse(
        "M11 15H6l7-14v8h5l-7 14v-8Z");

    public static readonly StreamGeometry Keyboard = StreamGeometry.Parse(
        "M19 10h-2V8h2v2m0 4h-2v-2h2v2m-4-4h-2V8h2v2m0 4h-2v-2h2v2m0 4H9v-2h6v2m-8-8H5V8h2v2m0 4H5v-2h2v2m8-4h-2V8h2v2m0 4h-2v-2h2v2M5 18h2v-2H5v2M3 6h18v12H3V6m0-2a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h18a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2H3Z");

    public static readonly StreamGeometry Web = StreamGeometry.Parse(
        "M16.36 14c.08-.66.14-1.32.14-2 0-.68-.06-1.34-.14-2h3.38c.16.64.26 1.31.26 2s-.1 1.36-.26 2h-3.38m-5.15 5.56c.6-1.11 1.06-2.31 1.38-3.56h2.95a8.03 8.03 0 0 1-4.33 3.56M14.34 14H9.66c-.1-.66-.16-1.32-.16-2 0-.68.06-1.35.16-2h4.68c.09.65.16 1.32.16 2 0 .68-.07 1.34-.16 2M12 19.96c-.83-1.2-1.5-2.53-1.91-3.96h3.82c-.41 1.43-1.08 2.76-1.91 3.96M8 8H5.08A7.923 7.923 0 0 1 9.4 4.44C8.8 5.55 8.35 6.75 8 8m-2.92 8H8c.35 1.25.8 2.45 1.4 3.56A8.008 8.008 0 0 1 5.08 16m-.82-2C4.1 13.36 4 12.69 4 12s.1-1.36.26-2h3.38c-.08.66-.14 1.32-.14 2 0 .68.06 1.34.14 2H4.26M12 4.03c.83 1.2 1.5 2.54 1.91 3.97h-3.82c.41-1.43 1.08-2.77 1.91-3.97M18.92 8h-2.95a15.65 15.65 0 0 0-1.38-3.56c1.84.63 3.37 1.9 4.33 3.56M12 2C6.47 2 2 6.5 2 12a10 10 0 0 0 10 10 10 10 0 0 0 10-10A10 10 0 0 0 12 2Z");

    // === Apps & Files ===
    public static readonly StreamGeometry Folder = StreamGeometry.Parse(
        "M10 4H4c-1.1 0-1.99.9-1.99 2L2 18c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V8c0-1.1-.9-2-2-2h-8l-2-2z");

    public static readonly StreamGeometry Terminal = StreamGeometry.Parse(
        "M20 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zM4 18V6h16v12H4zM7.5 15l4.5-4.5L7.5 6l-1.4 1.4 3.1 3.1-3.1 3.1 1.4 1.4zM12 15h6v-2h-6v2z");

    public static readonly StreamGeometry Code = StreamGeometry.Parse(
        "M9.4 16.6L4.8 12l4.6-4.6L8 6l-6 6 6 6 1.4-1.4zm5.2 0l4.6-4.6-4.6-4.6L16 6l6 6-6 6-1.4-1.4z");

    public static readonly StreamGeometry Image = StreamGeometry.Parse(
        "M21 19V5c0-1.1-.9-2-2-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2zM8.5 13.5l2.5 3.01L14.5 12l4.5 6H5l3.5-4.5z");

    public static readonly StreamGeometry Music = StreamGeometry.Parse(
        "M12 3v10.55c-.59-.34-1.27-.55-2-.55-2.21 0-4 1.79-4 4s1.79 4 4 4 4-1.79 4-4V7h4V3h-6z");

    public static readonly StreamGeometry Discord = StreamGeometry.Parse(
        "M19.27 5.33C17.94 4.71 16.5 4.26 15 4a.09.09 0 0 0-.07.03c-.18.33-.39.76-.53 1.09a16.09 16.09 0 0 0-4.8 0c-.14-.34-.35-.76-.54-1.09c-.01-.02-.04-.03-.07-.03c-1.5.26-2.93.71-4.27 1.33c-.01 0-.02.01-.03.02c-2.72 4.07-3.47 8.03-3.1 11.95c0 .02.01.04.03.05c1.8 1.32 3.53 2.12 5.2 2.65c.03.01.06 0 .07-.02c.4-.55.76-1.13 1.07-1.74c.02-.04 0-.08-.04-.09c-.57-.22-1.11-.48-1.64-.78c-.04-.02-.04-.08.01-.11c.11-.08.22-.17.33-.25c.02-.02.05-.02.07-.01c3.44 1.57 7.15 1.57 10.55 0c.02-.01.05-.01.07.01c.11.09.22.17.33.26c.04.03.04.09-.01.11c-.52.31-1.07.56-1.64.78c-.04.01-.05.06-.04.09c.32.61.68 1.19 1.07 1.74c.03.01.06.02.09.01c1.72-.53 3.45-1.33 5.25-2.65c.02-.01.03-.03.03-.05c.44-4.53-.73-8.46-3.1-11.95c-.01-.01-.02-.02-.04-.02zM8.52 14.91c-1.03 0-1.89-.95-1.89-2.12s.84-2.12 1.89-2.12c1.06 0 1.9.96 1.89 2.12c0 1.17-.84 2.12-1.89 2.12zm6.97 0c-1.03 0-1.89-.95-1.89-2.12s.84-2.12 1.89-2.12c1.06 0 1.9.96 1.89 2.12c0 1.17-.83 2.12-1.89 2.12z");

    public static readonly StreamGeometry Spotify = StreamGeometry.Parse(
        "M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm4.59 14.42c-.18.3-.56.4-.86.22-2.36-1.44-5.33-1.76-8.83-.96-.33.07-.66-.14-.73-.47-.07-.33.14-.66.47-.73 3.86-.88 7.19-.51 9.9 1.15.3.18.4.56.22.86zm1.23-2.74c-.23.37-.71.49-1.08.26-2.7-1.66-6.81-2.14-9.99-1.17-.42.13-.87-.11-1-.53-.13-.42.11-.87.53-1 3.63-1.1 8.21-.56 11.28 1.33.37.23.49.71.26 1.08zm.11-2.88c-3.23-1.92-8.56-2.1-11.64-1.16-.5.15-1.02-.14-1.17-.64-.15-.5.14-1.02.64-1.17 3.55-1.08 9.44-.87 13.15 1.33.45.27.6.85.33 1.3-.27.45-.85.6-1.3.33z");
}
