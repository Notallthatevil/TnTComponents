using Microsoft.AspNetCore.Components;

using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTTabChild {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
        .SetDisabled(Disabled)
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
       .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    [Parameter]
    public TnTIcon? Icon { get; set; }

    [Parameter]
    public RenderFragment? TabHeaderTemplate { get; set; }

    [Parameter, EditorRequired]
    public string Title { get; set; } = default!;

    [CascadingParameter]
    private TnTTabView _context { get; set; } = default!;

    private string? _style;

    protected override void OnAfterRender(bool firstRender) {
        base.OnAfterRender(firstRender);
        _style = CssStyle;
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_context is null) {
            throw new InvalidOperationException($"A {nameof(TnTTabChild)} must be a child of {nameof(TnTTabView)}");
        }
        _context.AddTabChild(this);
        _style = $"{CssStyle}; display:none;";
    }
}