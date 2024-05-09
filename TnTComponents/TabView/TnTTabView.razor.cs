using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTTabView {

    [Parameter]
    public TnTColor ActiveIndicatorColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .SetAlternative(SecondaryTabView)
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
                   .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    [Parameter]
    public TnTColor HeaderBackgroundColor { get; set; } = TnTColor.Surface;

    [Parameter]
    public TnTColor HeaderTextColor { get; set; } = TnTColor.OnSurface;

    public override string? JsModulePath => "./_content/TnTComponents/TabView/TnTTabView.razor.js";

    [Parameter]
    public bool SecondaryTabView { get; set; }

    private List<TnTTabChild> _tabChildren = [];

    public void AddTabChild(TnTTabChild tabChild) {
        _tabChildren.Add(tabChild);
    }
}