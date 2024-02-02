using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TnTComponents.Core;

namespace TnTComponents;

public class TnTInputSelect<TInputType> : TnTInputBase<TInputType> {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override InputType Type { get; }
    private bool _multiple;

    public override string FormCssClass => CssClassBuilder.Create(base.FormCssClass)
        .AddClass("tnt-select-placeholder", !string.IsNullOrWhiteSpace(Placeholder))
        .Build();

    public TnTInputSelect() {
        _multiple = typeof(TInputType).IsArray;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddAttribute(10, "class", FormCssClass);
        {
            {
                if (StartIcon is not null) {
                    StartIcon.AdditionalClass = "tnt-start";
                    builder.AddContent(20, StartIcon.Render());
                }
            }
            {
                builder.OpenElement(100, "select");
                builder.AddMultipleAttributes(110, AdditionalAttributes);
                builder.AddAttribute(120, "multiple", _multiple);
                builder.AddAttribute(140, "style", FormCssStyle);
                builder.AddAttribute(170, "disabled", ParentFormDisabled ?? Disabled);
                builder.AddAttribute(171, "required", IsRequired());
                if (_multiple) {
                    builder.AddAttribute(5, "value", BindConverter.FormatValue(CurrentValue)?.ToString());
                    builder.AddAttribute(6, "onchange", EventCallback.Factory.CreateBinder<string?[]?>(this, SetCurrentValueAsStringArray, default));
                    builder.SetUpdatesAttributeName("value");
                }
                else {
                    builder.AddAttribute(7, "value", CurrentValueAsString);
                    builder.AddAttribute(8, "onchange", EventCallback.Factory.CreateBinder<string?>(this, value => { CurrentValueAsString = value; BindAfter.InvokeAsync(CurrentValue); }, default));
                    builder.SetUpdatesAttributeName("value");
                }

                builder.AddElementReferenceCapture(200, e => Element = e);
                if(!string.IsNullOrWhiteSpace(Placeholder)) {
                    builder.OpenElement(210, "option");
                    builder.AddAttribute(221, "selected", true);
                    builder.AddContent(230, Placeholder);
                    builder.CloseElement();
                }
                builder.AddContent(240, ChildContent);
                builder.CloseElement();
            }
            {
                if (EndIcon is not null) {
                    EndIcon.AdditionalClass = "tnt-end";
                    builder.AddContent(250, EndIcon.Render());
                }
            }
        }

        builder.CloseElement();
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

    private async Task SetCurrentValueAsStringArray(string?[]? value) {
        CurrentValue = BindConverter.TryConvertTo<TInputType>(value, CultureInfo.CurrentCulture, out var result) ? result : default;
        await BindAfter.InvokeAsync(CurrentValue);
    }
}