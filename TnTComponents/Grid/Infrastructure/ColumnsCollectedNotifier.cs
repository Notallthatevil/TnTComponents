using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace TnTComponents.Grid.Infrastructure;

/// <summary>
///     For internal use only. Do not use.
/// </summary>
/// <typeparam name="TGridItem">For internal use only. Do not use.</typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ColumnsCollectedNotifier<TGridItem> : Microsoft.AspNetCore.Components.IComponent {

    [CascadingParameter]
    internal TnTInternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    private bool _isFirstRender = true;

    /// <inheritdoc />
    public void Attach(RenderHandle renderHandle) { }

    /// <inheritdoc />
    public Task SetParametersAsync(ParameterView parameters) {
        if (_isFirstRender) {
            _isFirstRender = false;
            parameters.SetParameterProperties(this);
            return InternalGridContext.ColumnsFirstCollected.InvokeCallbacksAsync(null);
        }
        else {
            return Task.CompletedTask;
        }
    }
}