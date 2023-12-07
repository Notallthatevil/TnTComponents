using Microsoft.AspNetCore.Components;
using TnTComponents.Common;

namespace TnTComponents;
public partial class TnTAccordion {
    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [Parameter]
    public string TnTAccordionHeaderClass { get; set; } = "tnt-accordion-header";

    [Parameter]
    public string TnTAccordionContentClass { get; set; } = "tnt-accordion-content";
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public override string? Class { get; set; } = "tnt-accordion";

    private string _headerIdentifier = TnTComponentIdentifier.NewId();
    private string _contentIdentifier = TnTComponentIdentifier.NewId();

    private IReadOnlyDictionary<string, object> _headerAttribute => new Dictionary<string, object>() { { _headerIdentifier, string.Empty } };
    private IReadOnlyDictionary<string, object> _contentAttribute => new Dictionary<string, object>() { { _contentIdentifier, string.Empty } };
}
