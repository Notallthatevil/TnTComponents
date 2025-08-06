using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
/// <summary>
///     Provides a Blazor component base class that manages a <see cref="CancellationTokenSource" /> and ensures proper cancellation and disposal.
/// </summary>
/// <remarks>
///     Inherit from this class to enable cancellation support for asynchronous operations in your Blazor components. The cancellation token is automatically cancelled and disposed when the component
///     is disposed.
/// </remarks>
public abstract class TnTCancellationTokenComponentBase : TnTDisposableComponentBase {

    /// <summary>
    ///     Gets the <see cref="CancellationToken" /> associated with this component, which is cancelled when the component is disposed.
    /// </summary>
    protected CancellationToken CancellationToken => _cancellationTokenSource?.Token ?? CancellationToken.None;

    private CancellationTokenSource? _cancellationTokenSource = new();
    private bool _disposed;

#if NET9_0_OR_GREATER
    private readonly Lock _lock = new();
#else
    private readonly object _lock = new();
#endif
    /// <inheritdoc />
    protected override void Dispose(bool disposing) {
        base.Dispose(disposing);
        if (disposing) {
            lock (_lock) {
                if (!_disposed) {
                    try {
                        _cancellationTokenSource?.Cancel();
                        _cancellationTokenSource?.Dispose();
                        _cancellationTokenSource = null;
                    }
                    catch (ObjectDisposedException) { }
                    catch (OperationCanceledException) { }
                    _disposed = true;
                }
            }
        }
    }
}
