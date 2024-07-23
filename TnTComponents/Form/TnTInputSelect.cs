using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TnTComponents.Core;

namespace TnTComponents;

//public class TnTInputSelect<TInputType> : TnTInputBase<TInputType> {

    //[Parameter]
    //public bool AllowPlaceholderSelection { get; set; }

    //[Parameter]
    //public RenderFragment ChildContent { get; set; } = default!;

    //public override string FormCssClass => CssClassBuilder.Create(base.FormCssClass)
    //    .AddClass("tnt-select-placeholder", !string.IsNullOrWhiteSpace(Placeholder))
    //    .Build();

    //[Parameter]
    //public string? PlaceholderValue { get; set; } = string.Empty;

    //[Parameter]
    //public bool ShouldHavePlaceholderSelected { get; set; } = true;

    //public override InputType Type { get; }
    //private bool _multiple;

    //public TnTInputSelect() {
    //    _multiple = typeof(TInputType).IsArray;
    //}

    //protected override void BuildRenderTree(RenderTreeBuilder builder) {
    //    builder.OpenElement(0, "span");
    //    builder.AddAttribute(10, "class", FormCssClass);
    //    {
    //        {
    //            if (StartIcon is not null) {
    //                StartIcon.AdditionalClass = "tnt-start";
    //                builder.AddContent(20, StartIcon.Render());
    //            }
    //        }
    //        {
    //            builder.OpenElement(30, "select");
    //            builder.AddMultipleAttributes(40, AdditionalAttributes);
    //            builder.AddAttribute(50, "multiple", _multiple);
    //            builder.AddAttribute(60, "style", FormCssStyle);
    //            builder.AddAttribute(70, "disabled", (ParentFormDisabled ?? Disabled) || (ParentFormReadOnly ?? ReadOnly));
    //            builder.AddAttribute(80, "required", IsRequired());
    //            if (_multiple) {
    //                builder.AddAttribute(90, "value", BindConverter.FormatValue(CurrentValue)?.ToString());
    //                builder.AddAttribute(100, "onchange", EventCallback.Factory.CreateBinder<string?[]?>(this, SetCurrentValueAsStringArray, default));
    //                builder.SetUpdatesAttributeName("value");
    //            }
    //            else {
    //                builder.AddAttribute(110, "value", CurrentValueAsString);
    //                builder.AddAttribute(120, "onchange", EventCallback.Factory.CreateBinder<string?>(this, value => { CurrentValueAsString = value; BindAfter.InvokeAsync(CurrentValue); }, default));
    //                builder.SetUpdatesAttributeName("value");
    //            }

    //            if (EditContext is not null) {
    //                builder.AddAttribute(130, "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, args => {
    //                    EditContext.NotifyFieldChanged(FieldIdentifier);
    //                }));
    //            }

    //            builder.AddElementReferenceCapture(140, e => Element = e);
    //            if (!string.IsNullOrWhiteSpace(Placeholder)) {
    //                builder.OpenElement(150, "option");
    //                if (ShouldHavePlaceholderSelected) {
    //                    builder.AddAttribute(160, "selected", true);
    //                }
    //                if (!AllowPlaceholderSelection) {
    //                    builder.AddAttribute(170, "disabled");
    //                }
    //                else {
    //                    builder.AddAttribute(180, "value", PlaceholderValue);
    //                }
    //                builder.AddContent(190, Placeholder);
    //                builder.CloseElement();
    //            }
    //            builder.AddContent(200, ChildContent);
    //            builder.CloseElement();

    //            if (EditContext is not null && !DisableValidationMessage && ValueExpression is not null) {
    //                builder.OpenComponent<ValidationMessage<TInputType>>(210);
    //                builder.AddComponentParameter(220, nameof(ValidationMessage<TInputType>.For), ValueExpression);
    //                builder.AddAttribute(230, "class", "tnt-components tnt-validation-message tnt-body-small");
    //                builder.CloseComponent();
    //            }
    //        }
    //        {
    //            if (EndIcon is not null) {
    //                EndIcon.AdditionalClass = "tnt-end";
    //                builder.AddContent(240, EndIcon.Render());
    //            }
    //        }
    //    }

    //    builder.CloseElement();
    //}

    //protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TInputType result, [NotNullWhen(false)] out string? validationErrorMessage) {
    //    try {
    //        // We special-case bool values because BindConverter reserves bool conversion for
    //        // conditional attributes.
    //        if (typeof(TInputType) == typeof(bool)) {
    //            if (TryConvertToBool(value, out result)) {
    //                validationErrorMessage = null;
    //                return true;
    //            }
    //        }
    //        else if (typeof(TInputType) == typeof(bool?)) {
    //            if (TryConvertToNullableBool(value, out result)) {
    //                validationErrorMessage = null;
    //                return true;
    //            }
    //        }
    //        else if (BindConverter.TryConvertTo<TInputType>(value, CultureInfo.CurrentCulture, out var parsedValue)) {
    //            result = parsedValue;
    //            validationErrorMessage = null;
    //            return true;
    //        }

    //        result = default;
    //        validationErrorMessage = $"The {DisplayName ?? FieldIdentifier.FieldName} field is not valid.";
    //        return false;
    //    }
    //    catch (InvalidOperationException ex) {
    //        throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TInputType)}'.", ex);
    //    }
    //}

    //private static bool TryConvertToBool<TValue>(string? value, out TValue result) {
    //    if (bool.TryParse(value, out var @bool)) {
    //        result = (TValue)(object)@bool;
    //        return true;
    //    }

    //    result = default!;
    //    return false;
    //}

    //private static bool TryConvertToNullableBool<TValue>(string? value, out TValue result) {
    //    if (string.IsNullOrEmpty(value)) {
    //        result = default!;
    //        return true;
    //    }

    //    return TryConvertToBool(value, out result);
    //}

    //private async Task SetCurrentValueAsStringArray(string?[]? value) {
    //    CurrentValue = BindConverter.TryConvertTo<TInputType>(value, CultureInfo.CurrentCulture, out var result) ? result : default;
    //    await BindAfter.InvokeAsync(CurrentValue);
    //}
//}