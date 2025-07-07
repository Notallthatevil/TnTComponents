using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     Represents a radio group input component.
/// </summary>
/// <typeparam name="TInputType">The type of the input value.</typeparam>
[CascadingTypeParameter(nameof(TInputType))]
public partial class TnTInputRadioGroup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TInputType> {

    /// <summary>
    ///     Gets or sets the child content to be rendered inside the radio group.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create(base.ElementClass!)
        .AddClass("tnt-radio-group")
        .AddClass("tnt-vertical", LayoutDirection == LayoutDirection.Vertical)
        .Build();

    /// <summary>
    ///     Gets or sets the layout direction of the radio group.
    /// </summary>
    [Parameter]
    public LayoutDirection LayoutDirection { get; set; }

    /// <inheritdoc />
    public override InputType Type => InputType.Radio;

    /// <summary>
    ///     Gets or sets the current value of the radio group.
    /// </summary>
    internal TInputType? InternalCurrentValue { get => CurrentValue; set => CurrentValue = value; }

    /// <summary>
    ///     Gets the edit context of the radio group.
    /// </summary>
    internal EditContext InternalEditContext { get => EditContext; }

    /// <summary>
    ///     Notifies that the state of the radio group has changed.
    /// </summary>
    internal void NotifyStateChanged() {
        EditContext.NotifyFieldChanged(FieldIdentifier);
    }

    /// <summary>
    ///     Updates the value of the radio group based on the change event arguments.
    /// </summary>
    /// <param name="args">The change event arguments.</param>
    internal async Task UpdateValueAsync(ChangeEventArgs args) {
        CurrentValueAsString = args.Value?.ToString();
        await BindAfter.InvokeAsync(CurrentValue);
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
    ///     Tries to convert a string value to a boolean.
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
    ///     Tries to convert a string value to a nullable boolean.
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