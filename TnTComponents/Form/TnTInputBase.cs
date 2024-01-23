using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using TnTComponents.Form;

namespace TnTComponents;

public abstract partial class TnTInputBase<TInputType> : ComponentBase, IFormField {

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
    public FormAppearance? ParentFormAppearance { get; set; }

    [CascadingParameter(Name = nameof(ParentFormDisabled))]
    public bool? ParentFormDisabled { get; set; }

    [CascadingParameter(Name = nameof(ParentFormReadOnly))]
    public bool? ParentFormReadOnly { get; set; }

    [CascadingParameter]
    public TnTLabel? Label { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string? Style { get; set; }

    [Parameter]
    public MaterialIcons? StartIcon { get; set; }
    [Parameter]
    public MaterialIcons? EndIcon { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    public ElementReference Element { get; protected set; }

    public abstract InputType Type { get; protected set; }

    [Parameter]
    public TInputType? Value { get; set; }


    [Parameter]
    public EventCallback<ChangeEventArgs> OnChanged { get; set; }

    [Parameter]
    public bool CallbackOnInput { get; set; }

    [Parameter]
    public EventCallback<TInputType?> BindAfter { get; set; }

    protected abstract string? CurrentValueAsString { get; }

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

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        // TODO add start icon

        {
            builder.OpenElement(100, "input");
            builder.AddMultipleAttributes(110, AdditionalAttributes);
            builder.AddAttribute(120, "type", Type.ToInputTypeString());
            builder.AddAttribute(130, "class", Class);
            builder.AddAttribute(140, "style", Style);
            builder.AddAttribute(150, "readonly", ReadOnly);
            builder.AddAttribute(160, "placeholder", Placeholder);
            builder.AddAttribute(170, "disabled", Disabled);
            if (CallbackOnInput) {
                builder.AddAttribute(180, "oninput", EventCallback.Factory.Create(this, async (ChangeEventArgs args) => { await OnChanged.InvokeAsync(args); await BindAfter.InvokeAsync((TInputType?)args.Value); }));
            }
            else {
                builder.AddAttribute(180, "onchange", EventCallback.Factory.Create(this, async (ChangeEventArgs args) => { await OnChanged.InvokeAsync(args); await BindAfter.InvokeAsync((TInputType?)args.Value); }));
            }

            builder.AddAttribute(190, "value", CurrentValueAsString);

            builder.AddElementReferenceCapture(200, (e) => Element = e);
            builder.CloseElement();
        }
        // TODO add end icon
    }

}