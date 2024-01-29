using Microsoft.AspNetCore.Components;

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

    public override string? Class => null;
}