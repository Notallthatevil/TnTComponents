using Microsoft.AspNetCore.Components;
using TnTComponents.Form;

namespace TnTComponents;

public abstract class TnTInputBase<TInputType> : ComponentBase, IFormField {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public FormAppearance Appearance { get; set; }

    public abstract string Class { get; }

    [Parameter]
    public bool Disabled { get; set; }

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }

    [CascadingParameter(Name = nameof(ParentFormAppearance))]
    public FormAppearance? ParentFormAppearance { get; }

    [CascadingParameter(Name = nameof(ParentFormDisabled))]
    public bool? ParentFormDisabled { get; }

    [CascadingParameter(Name = nameof(ParentFormReadOnly))]
    public bool? ParentFormReadOnly { get; }

    [CascadingParameter]
    public TnTLabel? Label { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string? Style { get; set; }

    public abstract InputType Type { get; }

    public TInputType? Value { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        if (ParentFormAppearance.HasValue) {
            Appearance = ParentFormAppearance.Value;
        }

        if (ParentFormReadOnly.HasValue) {
            ReadOnly = ParentFormReadOnly.Value;
        }

        if (ParentFormDisabled.HasValue) {
            Disabled = ParentFormDisabled.Value;
        }

        Label?.SetChildField(this);
    }
}