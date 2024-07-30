using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TnTComponents.Core;

namespace TnTComponents;
[CascadingTypeParameter(nameof(TInputType))]
public class TnTInputRadioGroup<TInputType> : TnTInputBase<TInputType> {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;


    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-radio-group")
        .AddClass("tnt-vertical", LayoutDirection == LayoutDirection.Vertical)
        .Build();

    [Parameter]
    public string RadioGroupName { get; set; } = TnTComponentIdentifier.NewId();

    public override InputType Type => InputType.Radio;
    internal TInputType? InternalCurrentValue { get => CurrentValue; set => CurrentValue = value; }
    internal EditContext InternalEditContext { get => EditContext; }

    [Parameter]
    public LayoutDirection LayoutDirection { get; set; }

    internal void NotifyStateChanged() {
        EditContext.NotifyFieldChanged(FieldIdentifier);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "fieldset");
        builder.AddMultipleAttributes(10, AdditionalAttributes);
        builder.AddAttribute(20, "class", ElementClass);
        builder.AddAttribute(30, "style", ElementStyle);
        builder.AddAttribute(40, "name", ElementName);
        builder.AddAttribute(50, "id", ElementId);
        builder.AddAttribute(70, "title", ElementTitle);
        builder.AddAttribute(80, "lang", ElementLang);
        builder.AddElementReferenceCapture(90, e => Element = e);

        if (StartIcon is not null) {
            StartIcon.AdditionalClass = "tnt-start-icon";
            builder.AddContent(100, StartIcon.Render());
        }

        if(!string.IsNullOrWhiteSpace(Label)){
            builder.OpenElement(110, "legend");
            builder.AddContent(120, Label);
            builder.CloseElement();
        }

        {
            builder.OpenComponent<CascadingValue<TnTInputRadioGroup<TInputType>>>(130);
            builder.AddComponentParameter(140, nameof(CascadingValue<TnTInputRadioGroup<TInputType>>.Value), this);
            builder.AddComponentParameter(150, nameof(CascadingValue<TnTInputRadioGroup<TInputType>>.IsFixed), true);
            builder.AddComponentParameter(160, nameof(CascadingValue<TnTInputRadioGroup<TInputType>>.ChildContent), ChildContent);
            builder.CloseComponent();
        }

        if (EditContext is not null && !DisableValidationMessage && ValueExpression is not null) {
            builder.OpenComponent<ValidationMessage<TInputType>>(170);
            builder.AddComponentParameter(180, nameof(ValidationMessage<TInputType>.For), ValueExpression);
            builder.AddAttribute(190, "class", "tnt-components tnt-validation-message tnt-body-small");
            builder.CloseComponent();
        }

        if (EndIcon is not null) {
            EndIcon.AdditionalClass = "tnt-end-icon";
            builder.AddContent(200, EndIcon.Render());
        }

        builder.CloseElement();
    }

    internal void UpdateValue(ChangeEventArgs args) {
        CurrentValueAsString = args.Value?.ToString();
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TInputType result, [NotNullWhen(false)] out string? validationErrorMessage) {
        try {
            // We special-case bool values because BindConverter reserves bool conversion for
            // conditional attributes.
            if (typeof(TInputType) == typeof(bool)) {
                if (TryConvertToBool(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (typeof(TInputType) == typeof(bool?)) {
                if (TryConvertToNullableBool(value, out result)) {
                    validationErrorMessage = null;
                    return true;
                }
            }
            else if (BindConverter.TryConvertTo<TInputType>(value, CultureInfo.CurrentCulture, out var parsedValue)) {
                result = parsedValue;
                validationErrorMessage = null;
                return true;
            }

            result = default;
            validationErrorMessage = $"The {DisplayName ?? FieldIdentifier.FieldName} field is not valid.";
            return false;
        }
        catch (InvalidOperationException ex) {
            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TInputType)}'.", ex);
        }
    }

    private static bool TryConvertToBool<TValue>(string? value, out TValue result) {
        if (bool.TryParse(value, out var @bool)) {
            result = (TValue)(object)@bool;
            return true;
        }

        result = default!;
        return false;
    }

    private static bool TryConvertToNullableBool<TValue>(string? value, out TValue result) {
        if (string.IsNullOrEmpty(value)) {
            result = default!;
            return true;
        }

        return TryConvertToBool(value, out result);
    }
}
