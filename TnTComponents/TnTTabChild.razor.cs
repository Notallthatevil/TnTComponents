using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents;
public partial class TnTTabChild {
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public string TabButtonClass { get; set; } = "tnt-tab-view-button";

    [Parameter]
    public override string? Class { get; set; } = "tnt-tab-view-content";

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public bool Active { get; set; } = false;

    [Parameter]
    public bool Disabled { get; set; }

    private IReadOnlyDictionary<string, object> ComponentIdentifierAttribute => new Dictionary<string, object>() {
            { TnTCustomIdentifier, ComponentIdentifier }
        };
}
