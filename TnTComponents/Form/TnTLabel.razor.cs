using Microsoft.AspNetCore.Components;
using TnTComponents.Core;
using TnTComponents.Form;

namespace TnTComponents;

public partial class TnTLabel : IFormItem {

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public string Class => CssClassBuilder.Create()
        .AddOutlined((ParentFormAppearance ?? Appearance) == FormAppearance.Outlined)
        .AddFilled((ParentFormAppearance ?? Appearance) == FormAppearance.Filled)
        .AddBackgroundColor(TnTColor.Transparent)
        .AddForegroundColor(TextColor)
        .Build();

    [Parameter]
    public bool Disabled { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter, EditorRequired]
    public string Label { get; set; } = default!;

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }

    [CascadingParameter(Name = nameof(ParentFormAppearance))]
    public FormAppearance? ParentFormAppearance { get; set; }

    [CascadingParameter(Name = nameof(ParentFormDisabled))]
    public bool? ParentFormDisabled { get; set; }

    [CascadingParameter(Name = nameof(ParentFormReadOnly))]
    public bool? ParentFormReadOnly { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public TnTColor? TextColor { get; set; }

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

        if (Appearance == FormAppearance.Filled) {
            BackgroundColor ??= TnTColor.Transparent;
            TextColor ??= TnTColor.OnSurfaceVariant;
        }
        else if (Appearance == FormAppearance.Outlined) {
            BackgroundColor ??= TnTColor.Surface;
            TextColor ??= TnTColor.OnSurface;
        }
    }
}