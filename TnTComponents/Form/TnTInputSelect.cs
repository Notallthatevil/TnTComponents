using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a custom input select component.
/// </summary>
/// <typeparam name="TInputType">The type of the input value.</typeparam>
public class TnTInputSelect<TInputType> : TnTInputBase<TInputType> {

    /// <summary>
    ///     Gets or sets a value indicating whether the placeholder selection is allowed.
    /// </summary>
    [Parameter]
    public bool AllowPlaceholderSelection { get; set; } = true;

    /// <summary>
    ///     Gets or sets the child content to be rendered inside the select element.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the value of the placeholder.
    /// </summary>
    [Parameter]
    public object? PlaceholderValue { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the placeholder should be selected.
    /// </summary>
    [Parameter]
    public bool ShouldHavePlaceholderSelected { get; set; } = true;

    /// <inheritdoc />
    public override InputType Type => InputType.Select;

    /// <inheritdoc />
    protected override void RenderChildContent(RenderTreeBuilder builder) {
        if (!string.IsNullOrWhiteSpace(Placeholder)) {
            builder.OpenElement(0, "option");
            if (ShouldHavePlaceholderSelected) {
                builder.AddAttribute(10, "selected", true);
            }
            if (!AllowPlaceholderSelection) {
                builder.AddAttribute(20, "disabled");
            }
            else {
                builder.AddAttribute(30, "value", PlaceholderValue ?? (object)string.Empty);
            }
            builder.AddContent(40, Placeholder);
            builder.CloseElement();
        }
        builder.AddContent(50, ChildContent);
    }

    /// <inheritdoc />
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

    /// <summary>
    ///     Tries to convert a string to a boolean value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value"> The string value to convert.</param>
    /// <param name="result">The converted result.</param>
    /// <returns><c>true</c> if conversion is successful; otherwise, <c>false</c>.</returns>
    private static bool TryConvertToBool<TValue>(string? value, out TValue result) {
        if (bool.TryParse(value, out var @bool)) {
            result = (TValue)(object)@bool;
            return true;
        }

        result = default!;
        return false;
    }

    /// <summary>
    ///     Tries to convert a string to a nullable boolean value.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="value"> The string value to convert.</param>
    /// <param name="result">The converted result.</param>
    /// <returns><c>true</c> if conversion is successful; otherwise, <c>false</c>.</returns>
    private static bool TryConvertToNullableBool<TValue>(string? value, out TValue result) {
        if (string.IsNullOrEmpty(value)) {
            result = default!;
            return true;
        }

        return TryConvertToBool(value, out result);
    }
}