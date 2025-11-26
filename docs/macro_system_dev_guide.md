# AerionDeck Advanced Macro System - Developer Guide

This guide explains the architecture of the AerionDeck Macro System and how developers can extend it.

## Architecture Overview

The Macro System is designed to be modular and decoupled from the main Action system.

### Core Components

1.  **`Macro` Model**: Represents a sequence of actions.
    *   `Id`: Unique identifier (GUID).
    *   `Name`: User-friendly name.
    *   `Steps`: List of `MacroStep` objects.

2.  **`MacroStep` Model**: Represents a single unit of work within a macro.
    *   `Type`: The type of action (e.g., `RunCommand`, `Delay`, `OBS_SwitchScene`).
    *   `Value`: The parameter for the action (e.g., path to exe, delay in ms, scene name).

3.  **`MacroManagerService`**: The central repository for macros.
    *   Loads/Saves macros from `macros.json`.
    *   Provides CRUD operations.

4.  **`MacroExecutionService`**: The engine that runs the macros.
    *   Iterates through steps.
    *   Delegates execution to specific handlers (or handles them inline for now).

## Creating a New Macro Action Type

To add a new capability to the macro system (e.g., "Control Philips Hue"), follow these steps:

### 1. Update the Enum
Add a new entry to `MacroActionType` in `MacroModels.cs`:

```csharp
public enum MacroActionType
{
    // ... existing types
    PhilipsHue_ToggleLight
}
```

### 2. Update the Execution Logic
Modify `MacroExecutionService.cs` to handle the new type.

*   **Dependency Injection**: If your action requires a new service (e.g., `HueService`), inject it into the constructor.
*   **Switch Case**: Add a new `case` in `ExecuteMacroAsync`:

```csharp
case MacroActionType.PhilipsHue_ToggleLight:
    await _hueService.ToggleLightAsync(step.Value); // step.Value could be Light ID
    break;
```

### 3. Update the UI (Future)
*   Update the Macro Editor to allow selecting this new type and inputting the value.

## Plugin Architecture (Future Vision)

In the future, we aim to support dynamic loading of action types.

*   **`IMacroStepExecutor` Interface**:
    ```csharp
    public interface IMacroStepExecutor
    {
        MacroActionType SupportedType { get; }
        Task ExecuteAsync(string value);
    }
    ```
*   **Discovery**: The `MacroExecutionService` would discover all implementations of `IMacroStepExecutor` and register them.
*   **Plugins**: DLLs containing new `IMacroStepExecutor` implementations could be dropped into a `Plugins` folder.

## Best Practices

*   **Asynchronous Execution**: Always use `async/await` for I/O bound operations (network, file system, delays).
*   **Error Handling**: Wrap step execution in `try/catch` blocks so one failed step doesn't crash the entire application (unless it's critical).
*   **Validation**: Validate `step.Value` before attempting execution.
