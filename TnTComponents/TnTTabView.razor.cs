using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.JSInterop;
using TnTComponents.Common;
using TnTComponents.Common.Ext;
using TnTComponents.Enum;

namespace TnTComponents;

public partial class TnTTabView {
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


    public override string GetClass() => $"{base.GetClass()} {Appearance.ToString().ToLower()}";
}