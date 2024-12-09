using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace TnTComponents.Core;

/// <summary>
/// A component that handles external click events.
/// </summary>
public partial class TnTExternalClickHandler {

    /// <summary>
    /// Gets or sets the child content to be rendered inside this component.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddClass("tnt-external-click-handler")
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// Gets or sets the callback to be invoked when an external click is detected.
    /// </summary>
    [Parameter, EditorRequired]
    public EventCallback ExternalClickCallback { get; set; }

    public override string? JsModulePath => "./_content/TnTComponents/Core/TnTExternalClickHandler.razor.js";

    /// <summary>
    /// Initializes a new instance of the <see cref="TnTExternalClickHandler"/> class.
    /// </summary>
    [DynamicDependency(nameof(OnClick))]
    public TnTExternalClickHandler() { }

    /// <summary>
    /// Invoked by JavaScript to handle an external click event.
    /// </summary>
    [JSInvokable]
    public async Task OnClick() => await ExternalClickCallback.InvokeAsync();

    /// <summary>
    /// Disposes the component and deregisters the external click callback.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected override async ValueTask DisposeAsyncCore() {
        if (IsolatedJsModule is not null) {
            await IsolatedJsModule.InvokeVoidAsync("externalClickCallbackDeregister", DotNetObjectRef).ConfigureAwait(false);
        }
        await base.DisposeAsyncCore().ConfigureAwait(false);
    }

    /// <summary>
    /// Called after the component has been rendered.
    /// </summary>
    /// <param name="firstRender">True if this is the first time the component is being rendered.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender && IsolatedJsModule is not null) {
            await IsolatedJsModule.InvokeVoidAsync("externalClickCallbackRegister", Element, DotNetObjectRef);
        }
    }
}
