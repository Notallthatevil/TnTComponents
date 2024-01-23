using Microsoft.AspNetCore.Components;
using TnTComponents.Form;

namespace TnTComponents;

public partial class TnTLabel : IFormItem {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public string Class { get; }

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

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;
    public ElementReference Element { get; protected set; }

    private IFormField? _childField;

    public void SetChildField(IFormField formField) {
        if (_childField is not null) {
            throw new InvalidOperationException($"{nameof(TnTLabel)}s can only have one child field added!");
        }

        _childField = formField;
    }

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
    }
}