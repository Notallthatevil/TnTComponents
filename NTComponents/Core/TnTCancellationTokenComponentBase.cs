namespace NTComponents.Core;

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

    /// <inheritdoc />
    protected override void Dispose(bool disposing) {
        if (disposing && !_disposed) {
            try {
                var cts = Interlocked.Exchange(ref _cancellationTokenSource, null);
                if (cts is not null) {
                    cts.Cancel();
                    cts.Dispose();
                }
            }
            catch (ObjectDisposedException) { }
            finally {
                _disposed = true;
            }
        }
        base.Dispose(disposing);
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsyncCore() {
        if (!_disposed) {
            try {
                var cts = Interlocked.Exchange(ref _cancellationTokenSource, null);
                if (cts is not null) {
                    await cts.CancelAsync().ConfigureAwait(false);
                    cts.Dispose();
                }
            }
            catch (ObjectDisposedException) { }
            finally {
                _disposed = true;
            }
        }
        await base.DisposeAsyncCore();
    }
}