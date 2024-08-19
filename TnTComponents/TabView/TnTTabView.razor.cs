using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTTabView {

    [Parameter]
    public TnTColor ActiveIndicatorColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-tab-view")
        .AddClass("tnt-tab-view-secondary", Appearance == TabViewAppearance.Secondary)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("active-text-color", ActiveIndicatorColor)
        .Build();

    [Parameter]
    public TnTColor HeaderBackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTColor HeaderTextColor { get; set; } = TnTColor.OnSurface;

    [Parameter]
    public TabViewAppearance Appearance { get; set; } = TabViewAppearance.Primary;

    public override string? JsModulePath => "./_content/TnTComponents/TabView/TnTTabView.razor.js";

    private readonly List<TnTTabChild> _tabChildren = [];

    public void AddTabChild(TnTTabChild tabChild) {
        _tabChildren.Add(tabChild);
    }
}