using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TnTComponents.Core;

public partial class TnTExternalClickHandler {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .AddClass("tnt-external-click-handler")
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
   .AddFromAdditionalAttributes(AdditionalAttributes)
   .Build();

    [Parameter, EditorRequired]
    public EventCallback ExternalClickCallback { get; set; }

    public override string? JsModulePath => "./_content/TnTComponents/Core/TnTExternalClickHandler.razor.js";

    public override async ValueTask DisposeAsync() {
        try {
            if (IsolatedJsModule is not null) {
                await IsolatedJsModule.InvokeVoidAsync("externalClickCallbackDeregister", DotNetObjectRef);
                await base.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
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