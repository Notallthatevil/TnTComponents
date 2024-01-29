using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Form;

namespace TnTComponents;
[CascadingTypeParameter(nameof(TInputType))]
public class TnTInputRadioGroup<TInputType> : TnTInputSelect<TInputType> {

    [Parameter, EditorRequired]
    public string RadioGroupName { get; set; } = default!;
    [Parameter]
    public LayoutDirection Direction { get; set; } = LayoutDirection.Vertical;

    public override InputType Type { get; }
    internal RadioContext _radioContext = new();

    protected override void OnInitialized() {
        base.OnInitialized();
        _radioContext.ChangeEventCallback = EventCallback.Factory.CreateBinder(this, async value => { CurrentValue = value; await BindAfter.InvokeAsync(CurrentValue); }, CurrentValue);
        if (string.IsNullOrWhiteSpace(RadioGroupName)) {
            throw new InvalidOperationException("Must provide a valid name for a radio group");
        }
        _radioContext.GroupName = RadioGroupName;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {

        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "class", "tnt-radio-group " + Class);
        builder.AddAttribute(2, "style", $"flex-direction: {(Direction == LayoutDirection.Vertical ? "column" : "row")}");

        {
            builder.OpenElement(3, "span");
            builder.AddAttribute(4, "class", "tnt-radio-group-name");
            builder.AddContent(5, RadioGroupName);
            builder.CloseElement();
        }

        {
            builder.OpenComponent<CascadingValue<RadioContext>>(10);
            builder.AddComponentParameter(20, "Value", _radioContext);
            builder.AddComponentParameter(30, "ChildContent", ChildContent);
            builder.AddComponentParameter(40, "IsFixed", true);
            builder.CloseComponent();
        }
        builder.CloseElement();
    }
}

internal class RadioContext {
    public EventCallback<ChangeEventArgs> ChangeEventCallback { get; internal set; }
    public object? CurrentValue { get; internal set; }
    public string GroupName { get; internal set; } = default!;
    public LayoutDirection Direction { get; internal set; }

}

public class TnTInputRadio<TInputType> : ComponentBase, IFormField {
    [CascadingParameter]
    private RadioContext _context { get; set; } = default!;

    [CascadingParameter]
    private TnTLabel _label { get; set; } = default!;

    [CascadingParameter]
    private bool Bool { get; set; }

    public ElementReference Element { get; protected set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public TInputType? Value { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    public InputType Type => InputType.Checkbox;
    public string? Placeholder => string.Empty;
    [Parameter]
    public FormAppearance Appearance { get; set; }
    public string Class => string.Empty;
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
    [Parameter]
    public bool ReadOnly { get; set; }
    [Parameter]
    public string? Style { get; set; }
    [Parameter]
    public TnTColor? BackgroundColor { get; set; }
    [Parameter]
    public TnTColor? TextColor { get; set; }

    private bool _toggle;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_context is null) {
            throw new InvalidOperationException($"{nameof(TnTInputRadio<TInputType>)} must be a child of {nameof(TnTInputRadioGroup<TInputType>)}");
        }
        if (_label is null) {
            throw new InvalidOperationException($"{nameof(TnTInputRadio<TInputType>)} must be a child of {nameof(TnTLabel)}");
        }
        _label.SetChildField(this);

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

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");

        if (StartIcon is not null) {
            builder.AddContent(10, StartIcon.Render());
        }
        {
            builder.OpenElement(20, "input");
            builder.AddMultipleAttributes(30, AdditionalAttributes);
            builder.AddAttribute(40, "type", InputType.Radio.ToInputTypeString());
            builder.AddAttribute(50, "name", _context.GroupName);
            builder.AddAttribute(60, "value", BindConverter.FormatValue(Value?.ToString()));
            builder.AddAttribute(70, "checked", _context.CurrentValue?.Equals(Value) == true ? GetToggledTrueValue() : null);
            builder.AddAttribute(80, "onchange", _context.ChangeEventCallback);
            builder.SetUpdatesAttributeName("checked");
            builder.AddElementReferenceCapture(90, e => Element = e);
            builder.CloseElement();
        }

        if (EndIcon is not null) {
            builder.AddContent(100, EndIcon.Render());
        }

        builder.CloseElement();
    }

    // This is an unfortunate hack, but is needed for the scenario described by test InputRadioGroupWorksWithMutatingSetter.
    // Radio groups are special in that modifying one <input type=radio> instantly and implicitly also modifies the previously
    // selected one in the same group. As such, our SetUpdatesAttributeName mechanism isn't sufficient to stay in sync with the
    // DOM, because the 'change' event will fire on the new <input type=radio> you just selected, not the previously-selected
    // one, and so the previously-selected one doesn't get notified to update its state in the old rendertree. So, if the setter
    // reverts the incoming value, the previously-selected one would produce an empty diff (because its .NET value hasn't changed)
    // and hence it would be left unselected in the DOM. If you don't understand why this is a problem, try commenting out the
    // line that toggles _trueValueToggle and see the E2E test fail.
    //
    // This hack works around that by causing InputRadio *always* to force its own 'checked' state to be true in the DOM if it's
    // true in .NET, whether or not it was true before, by continally changing the value that represents 'true'. This doesn't
    // really cause any significant increase in traffic because if we're rendering this InputRadio at all, sending one more small
    // attribute value is inconsequential.
    //
    // Ultimately, a better solution would be to make SetUpdatesAttributeName smarter still so that it knows about the special
    // semantics of radio buttons so that, when one <input type="radio"> changes, it treats any previously-selected sibling
    // as needing DOM sync as well. That's a more sophisticated change and might not even be useful if the radio buttons
    // aren't truly siblings and are in different DOM subtrees (and especially if they were rendered by different components!)
    private string GetToggledTrueValue() {
        _toggle = !_toggle;
        return _toggle ? "a" : "b";
    }


}

