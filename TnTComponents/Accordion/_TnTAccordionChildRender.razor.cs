using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace TnTComponents.Accordion;

/// <summary>
/// Provides rendering logic for a child of the TnTAccordion component.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public partial class _TnTAccordionChildRender {

    /// <summary>
    ///     The child to render.
    /// </summary>
    [Parameter, EditorRequired]
    public TnTAccordionChild Child { get; set; } = default!;

    [CascadingParameter]
    private TnTAccordion _parent { get;set; } = default!;
}