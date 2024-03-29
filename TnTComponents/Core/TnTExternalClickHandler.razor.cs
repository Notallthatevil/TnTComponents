using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Ext;

namespace TnTComponents.Core;

public partial class TnTExternalClickHandler {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => ExternalClickCssClass;

    public override string? JsModulePath => "./_content/TnTComponents/Core/TnTExternalClickHandler.razor.js";

    public override string? CssStyle => CssStyleBuilder.Create()
   .AddFromAdditionalAttributes(AdditionalAttributes)
   .Build();

    [Parameter, EditorRequired]
    public EventCallback ExternalClickCallback { get; set; }

    [Parameter]
    public string? ExternalClickCssClass { get; set; }

    public new async ValueTask DisposeAsync() {
        try {
            if (IsolatedJsModule is not null) {
                await IsolatedJsModule.InvokeVoidAsync("externalClickCallbackDeregister", Element, DotNetObjectRef);
                await IsolatedJsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }

        await base.DisposeAsync();
    }

    [JSInvokable]
    public async Task OnClick() {
        await ExternalClickCallback.InvokeAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender && IsolatedJsModule is not null) {
            await IsolatedJsModule.InvokeVoidAsync("externalClickCallbackRegister", Element, DotNetObjectRef);
        }
    }
}