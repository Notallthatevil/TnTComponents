using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TnTComponents;
using TnTComponents.Core;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTInputRadio<TInputType> : TnTInteractableComponentBase {
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTnTInteractable(this, enableDisabled: false)
        .AddDisabled(_disabled)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    [Parameter, EditorRequired]
    public object Value { get; set; } = default!;

    private bool _disabled => _group.FieldDisabled || Disabled;

    [CascadingParameter]
    public TnTInputRadioGroup<TInputType> _group { get; set; } = default!;

    private bool _readOnly => _group.FieldReadonly || ReadOnly;
    private bool _trueValueToggle;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "label");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(60, "id", ElementId);
        builder.AddAttribute(70, "name", ElementName);
        builder.AddElementReferenceCapture(80, e => Element = e);


        if (StartIcon is not null) {
            StartIcon.AdditionalClass = "tnt-start-icon";
            builder.AddContent(90, StartIcon.Render());
        }

        builder.OpenElement(100, "div");
        builder.AddAttribute(110, "class", "tnt-radio-button");
        builder.CloseElement();

        builder.OpenElement(120, "input");
        builder.AddAttribute(130, "title", ElementTitle);
        builder.AddAttribute(140, "type", _group.Type.ToInputTypeString());
        builder.AddMultipleAttributes(150, AdditionalAttributes);
        builder.AddAttribute(160, "name", _group.RadioGroupName);
        builder.AddAttribute(170, "style", ElementStyle);
        builder.AddAttribute(180, "readonly", _readOnly);
        builder.AddAttribute(190, "disabled", _disabled);
        builder.AddAttribute(200, "value", BindConverter.FormatValue(Value?.ToString()));
        builder.AddAttribute(210, "checked", _group.InternalCurrentValue?.Equals(Value) == true ? GetToggledTrueValue() : null);
        builder.AddAttribute(220, "onchange", _group.UpdateValue);
        builder.SetUpdatesAttributeName("checked");

        if (_group.InternalEditContext is not null) {
            builder.AddAttribute(230, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, args => {
                _group.NotifyStateChanged();
            }));
        }

        builder.CloseElement();

        if (!string.IsNullOrWhiteSpace(Label)) {
            builder.OpenElement(240, "span");
            builder.AddAttribute(250, "class", CssClassBuilder.Create().AddClass("tnt-label").Build());
            builder.AddContent(260, Label);
            builder.CloseElement();
        }

        if (EndIcon is not null) {
            EndIcon.AdditionalClass = "tnt-end-icon";
            builder.AddContent(270, EndIcon.Render());
        }

        builder.CloseElement();
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(_group, nameof(_group));
    }

    // This is an unfortunate hack, but is needed for the scenario described by test
    // InputRadioGroupWorksWithMutatingSetter. Radio groups are special in that modifying one <input
    // type=radio> instantly and implicitly also modifies the previously selected one in the same
    // group. As such, our SetUpdatesAttributeName mechanism isn't sufficient to stay in sync with
    // the DOM, because the 'change' event will fire on the new <input type=radio> you just
    // selected, not the previously-selected one, and so the previously-selected one doesn't get
    // notified to update its state in the old rendertree. So, if the setter reverts the incoming
    // value, the previously-selected one would produce an empty diff (because its .NET value hasn't
    // changed) and hence it would be left unselected in the DOM. If you don't understand why this
    // is a problem, try commenting out the line that toggles _trueValueToggle and see the E2E test fail.
    //
    // This hack works around that by causing InputRadio *always* to force its own 'checked' state
    // to be true in the DOM if it's true in .NET, whether or not it was true before, by continally
    // changing the value that represents 'true'. This doesn't really cause any significant increase
    // in traffic because if we're rendering this InputRadio at all, sending one more small
    // attribute value is inconsequential.
    //
    // Ultimately, a better solution would be to make SetUpdatesAttributeName smarter still so that
    // it knows about the special semantics of radio buttons so that, when one <input type="radio">
    // changes, it treats any previously-selected sibling as needing DOM sync as well. That's a more
    // sophisticated change and might not even be useful if the radio buttons aren't truly siblings
    // and are in different DOM subtrees (and especially if they were rendered by different components!)
    private string GetToggledTrueValue() {
        _trueValueToggle = !_trueValueToggle;
        return _trueValueToggle ? "a" : "b";
    }
}
