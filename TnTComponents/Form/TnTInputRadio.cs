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

/// <summary>
///     Represents a radio input component.
/// </summary>
/// <typeparam name="TInputType">The type of the input value.</typeparam>
public class TnTInputRadio<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TInputType> : TnTComponentBase, ITnTInteractable {

    /// <summary>
    ///     Gets or sets the cascading parameter for the radio group.
    /// </summary>
    [CascadingParameter]
    public TnTInputRadioGroup<TInputType> _group { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTnTInteractable(this, enableDisabled: false)
        .AddDisabled(_disabled)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; }

    /// <summary>
    ///     Gets or sets the icon displayed at the end of the radio input.
    /// </summary>
    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    /// <summary>
    ///     Gets or sets the label for the radio input.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the radio input is read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    ///     Gets or sets the icon displayed at the start of the radio input.
    /// </summary>
    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; }

    /// <summary>
    ///     Gets or sets the value of the radio input.
    /// </summary>
    [Parameter, EditorRequired]
    public TInputType Value { get; set; } = default!;

    private bool _disabled => _group.FieldDisabled || Disabled;
    private bool _readOnly => _group.FieldReadonly || ReadOnly;
    private bool _trueValueToggle;

    /// <inheritdoc />
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
        builder.AddAttribute(220, "onchange",EventCallback.Factory.Create(_group, () => _group.InternalCurrentValue = Value));
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
            builder.AddContent(270, EndIcon.Render());
        }

        builder.CloseElement();
    }

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        if(_group is null) {
            throw new ArgumentNullException($"{nameof(TnTInputRadio<TInputType>)} must be a child of {nameof(TnTInputRadioGroup<TInputType>)}. If you still receive this error, try explicitly setting the {nameof(TInputType)} of {nameof(TnTInputRadio<TInputType>)} to a Nullable type. This is likely the cause if {nameof(TInputType)} is struct or enum, as {nameof(TnTInputRadioGroup<TInputType>)} could be using the Nullable version ({nameof(TInputType)}?).");
        }
    }

    private string GetToggledTrueValue() {
        _trueValueToggle = !_trueValueToggle;
        return _trueValueToggle ? "a" : "b";
    }
}