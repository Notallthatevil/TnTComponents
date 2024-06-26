using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTAccordion {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTColor BodyBackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public TnTColor BodyTextColor { get; set; } = TnTColor.OnSurfaceVariant;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .AddRipple(Ripple)
        .AddElevation(Elevation)
        .AddActionableBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public int Elevation { get; set; } = 1;

    public override string? JsModulePath => "./_content/TnTComponents/Core/TnTAccordion.razor.js";

    [Parameter]
    public bool? OpenByDefault { get; set; }

    [Parameter]
    public bool Ripple { get; set; } = true;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    public ValueTask Close() {
        return IsolatedJsModule?.InvokeVoidAsync("closeAccordion", Element, DotNetObjectRef) ?? ValueTask.CompletedTask;
    }

    public ValueTask Open() {
        return IsolatedJsModule?.InvokeVoidAsync("openAccordion", Element, DotNetObjectRef) ?? ValueTask.CompletedTask;
    }

    public ValueTask Resize() {
        return IsolatedJsModule?.InvokeVoidAsync("resizeAccordion", Element, DotNetObjectRef) ?? ValueTask.CompletedTask;
    }

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        if (firstRender) {
            OpenByDefault = null;
        }
    }
}