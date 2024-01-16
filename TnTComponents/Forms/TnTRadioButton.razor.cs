using Microsoft.AspNetCore.Components;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

public partial class TnTRadioButton<TInputType> {

    [CascadingParameter]
    internal TnTRadioButtonContext? _cascadedContext { get; set; } = default!;

    [Parameter]
    public string LabelClass { get; set; } = TnTInputBase<TInputType>.DefaultLabelClass;

    [Parameter]
    public string ContainerClass { get; set; } = TnTInputBase<TInputType>.DefaultContainerClass;

    [Parameter]
    public IconType IconType { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? StartIcon { get; set; }

    [Parameter]
    public string? EndIcon { get; set; }

    [Parameter]
    public bool Required { get; set; }

    [Parameter]
    public string LabelTextClass { get; set; } = TnTInputBase<TInputType>.DefaultLabelTextClass;

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public bool? Checked { get; set; }

    [Parameter]
    public TInputType? Value { get; set; }

    private TnTRadioButtonContext? _context;

    protected override void OnInitialized() {
        base.OnInitialized();

        _context = string.IsNullOrWhiteSpace(Name) ? _cascadedContext : _cascadedContext?.FindContextInAncestors(Name);

        if (_context is null) {
            throw new InvalidOperationException($"{nameof(TnTRadioButton<TInputType>)} must have an ancestor of type {nameof(TnTRadioGroup<TInputType>)} and have a matching or empty {nameof(Name)} parameter");
        }

        if (Checked.HasValue && Checked.Value == true) {
            _context.CurrentValue = Value;
        }
    }
}