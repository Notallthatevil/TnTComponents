using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Common.Ext;
using TnTComponents.Enum;
using TnTComponents.Infrastructure;

namespace TnTComponents;

public partial class TnTTabView {

    [Parameter, EditorRequired]
    public string Name { get; set; } = default!;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string TabHeaderClass { get; set; } = "tnt-tab-header";
    [Parameter]
    public string TabActiveIndicatorClass { get; set; } = "tnt-tab-active-indicator";

    [Parameter]
    public string TabContentClass { get; set; } = "tnt-tab-content";

    [Parameter]
    public override string? Class { get; set; } = "tnt-tab-view";

    [Parameter]
    public TabViewAppearance Appearance { get; set; }

    private readonly TabViewContext _context;

    public TnTTabView() {
        _context = new(this);
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
    }

    public override string GetClass() => $"{base.GetClass()} {Appearance.ToString().ToLower()}";
}