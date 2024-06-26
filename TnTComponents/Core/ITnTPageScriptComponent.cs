﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Core;

/// <summary>
/// Represents a component that has an isolated JavaScript module
/// </summary>
public interface ITnTPageScriptComponent<TComponent> where TComponent : ComponentBase {

    /// <summary>
    /// Gets the reference to the DotNet object associated with the component.
    /// </summary>
    DotNetObjectReference<TComponent>? DotNetObjectRef { get; }

    /// <summary>
    /// Gets the reference to the isolated JavaScript module.
    /// </summary>
    IJSObjectReference? IsolatedJsModule { get; }

    /// <summary>
    /// Gets the path of the JavaScript module.
    /// </summary>
    string? JsModulePath { get; }
}