using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTAccordion {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public bool LimitToOneExpanded { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-accordion")
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();


    [Parameter]
    public TnTColor HeaderTextColor { get; set; } = TnTColor.OnPrimaryContainer;
    [Parameter]
    public TnTColor HeaderBodyColor { get; set; } = TnTColor.PrimaryContainer;
    [Parameter]
    public TnTColor HeaderTintColor { get; set; } = TnTColor.SurfaceTint;
    [Parameter]
    public TnTColor ContentTextColor { get; set; } = TnTColor.OnSurfaceVariant;
    [Parameter]
    public TnTColor ContentBodyColor { get; set; } = TnTColor.SurfaceVariant;

    public override string? JsModulePath => "./_content/TnTComponents/Accordion/TnTAccordion.razor.js";

    private readonly List<TnTAccordionChild> _children = [];

    public void RegisterChild(TnTAccordionChild child) {
        if (child is not null) {
            _children.Add(child);
            StateHasChanged();
        }
    }

    public void RemoveChild(TnTAccordionChild child) {
        if (child is not null) {
            _children.Remove(child);
            StateHasChanged();
        }
    }

    //public ValueTask Close() {
    //    return IsolatedJsModule?.InvokeVoidAsync("closeAccordion", Element, DotNetObjectRef) ?? ValueTask.CompletedTask;
    //}

    //public ValueTask Open() {
    //    return IsolatedJsModule?.InvokeVoidAsync("openAccordion", Element, DotNetObjectRef) ?? ValueTask.CompletedTask;
    //}

    //public ValueTask Resize() {
    //    return IsolatedJsModule?.InvokeVoidAsync("resizeAccordion", Element, DotNetObjectRef) ?? ValueTask.CompletedTask;
    //}

    //protected override void OnAfterRender(bool firstRender) {
    //    base.OnAfterRender(firstRender);
    //    if (firstRender) {
    //        OpenByDefault = null;
    //    }
    //}
}