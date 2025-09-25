using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Interfaces;

/// <summary>
///     Represents a component that has an isolated JavaScript module
/// </summary>
public interface ITnTPageScriptComponent< TComponent> : IAsyncDisposable, IDisposable, ITnTComponentBase where TComponent : ComponentBase {

    /// <summary>
    ///     Gets the reference to the DotNet object associated with the component.
    /// </summary>
    DotNetObjectReference<TComponent>? DotNetObjectRef { get; }

    /// <summary>
    ///     Gets the reference to the isolated JavaScript module.
    /// </summary>
    IJSObjectReference? IsolatedJsModule { get; }

    /// <summary>
    ///     Gets the path of the JavaScript module.
    /// </summary>
    string? JsModulePath { get; }
}
