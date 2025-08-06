using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
/// <summary>
///     Provides a base class for Blazor components that require both synchronous and asynchronous disposal. Implements the recommended disposal pattern for Blazor and WebAssembly scenarios.
/// </summary>
/// <remarks>
///     Inherit from this class to ensure proper cleanup of resources, such as JS interop objects or event handlers. Override <see cref="Dispose(bool)" /> for synchronous cleanup and <see
///     cref="DisposeAsyncCore" /> for asynchronous cleanup.
/// </remarks>
public abstract class TnTDisposableComponentBase : TnTComponentBase, IAsyncDisposable, IDisposable {

    /// <inheritdoc />
    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the component and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    /// <remarks>Override this method to dispose managed and unmanaged resources. This method is called by <see cref="Dispose()" /> and <see cref="DisposeAsync()" />.</remarks>
    protected virtual void Dispose(bool disposing) {
    }

    /// <summary>
    ///     Asynchronously releases the unmanaged resources used by the component.
    /// </summary>
    /// <returns>A <see cref="ValueTask" /> representing the asynchronous dispose operation.</returns>
    /// <remarks>Override this method to dispose asynchronous resources. This method is called by <see cref="DisposeAsync()" />.</remarks>
    protected virtual ValueTask DisposeAsyncCore() => ValueTask.CompletedTask;
}
