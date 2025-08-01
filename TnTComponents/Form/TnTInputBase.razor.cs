﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using TnTComponents.Core;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Base class for TnT input components.
/// </summary>
/// <typeparam name="TInputType">The type of the input value.</typeparam>
public abstract partial class TnTInputBase<TInputType> : InputBase<TInputType>, ITnTComponentBase {

    /// <summary>
    ///     Gets or sets the appearance of the form.
    /// </summary>
    [Parameter]
    public FormAppearance Appearance { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool? AutoFocus { get; set; }

    /// <summary>
    ///     Text that provides additional information about the input, such as usage instructions or validation hints.
    /// </summary>
    [Parameter]
    public string? SupportingText { get; set; }

    /// <summary>
    ///     Gets or sets the background color of the input.
    /// </summary>
    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainerHighest;

    /// <summary>
    ///     Gets or sets the event callback to be invoked after binding.
    /// </summary>
    [Parameter]
    public EventCallback<TInputType?> BindAfter { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to bind on input.
    /// </summary>
    [Parameter]
    public bool BindOnInput { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether to disable the validation message.
    /// </summary>
    [Parameter]
    public bool DisableValidationMessage { get; set; } = false;

    /// <inheritdoc />
    public ElementReference Element { get; protected set; }

#if NET9_0_OR_GREATER
    /// <summary>
    ///     Sets the color of the character length indicator.
    /// </summary>
    [Parameter]
    public TnTColor CharacterLengthColor { get; set; } = TnTColor.OnSurfaceVariant;
#endif

    /// <inheritdoc />
    public virtual string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass(CssClass)
        .AddClass("tnt-input")
        .AddClass("tnt-form-filled", _tntForm is not null ? _tntForm.Appearance == FormAppearance.Filled : Appearance == FormAppearance.Filled)
        .AddClass("tnt-form-outlined", _tntForm is not null ? _tntForm.Appearance == FormAppearance.Outlined : Appearance == FormAppearance.Outlined)
        .AddRipple(EnableRipple)
        .AddDisabled(FieldDisabled)
        .AddClass("tnt-placeholder", !string.IsNullOrEmpty(Placeholder))
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementId { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementLang { get; set; }

    /// <inheritdoc />
    public string? ElementName => NameAttributeValue;

    /// <inheritdoc />
    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("tnt-input-tint-color", TintColor.ToCssTnTColorVariable())
        .AddVariable("tnt-input-background-color", BackgroundColor.ToCssTnTColorVariable())
        .AddVariable("tnt-input-text-color", TextColor.ToCssTnTColorVariable())
        .AddVariable("tnt-input-error-color", ErrorColor.ToCssTnTColorVariable())
#if NET9_0_OR_GREATER
        .AddVariable("tnt-input-character-length-color", CharacterLengthColor.ToCssTnTColorVariable())
#endif
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementTitle { get; set; }

    /// <inheritdoc />
    public bool EnableRipple => false;

    /// <summary>
    ///     Gets or sets the end icon of the input.
    /// </summary>
    [Parameter]
    public TnTIcon? EndIcon { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the input field is disabled.
    /// </summary>
    public bool FieldDisabled => _tntForm?.Disabled is not null ? _tntForm.Disabled : Disabled;

    /// <summary>
    ///     Gets a value indicating whether the input field is read-only.
    /// </summary>
    public bool FieldReadonly => _tntForm?.ReadOnly is not null ? _tntForm.ReadOnly : ReadOnly;

    /// <summary>
    ///     Gets or sets the label of the input.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    ///     Gets or sets the placeholder text of the input.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the input is read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    ///     Gets or sets the start icon of the input.
    /// </summary>
    [Parameter]
    public TnTIcon? StartIcon { get; set; }

    /// <summary>
    ///     Gets or sets the text color of the input.
    /// </summary>
    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    /// <inheritdoc />
    [Parameter]
    public TnTColor TintColor { get; set; } = TnTColor.Primary;

    /// <summary>
    ///     Specifies the type of the input element, which determines how the input is rendered and validated.
    /// </summary>
    [Parameter]
    public string? AutoComplete { get; set; }

    /// <inheritdoc />
    public abstract InputType Type { get; }

    /// <summary>
    ///     The color used for the error state of the input.
    /// </summary>
    [Parameter]
    public TnTColor ErrorColor { get; set; } = TnTColor.Error;

    /// <summary>
    ///     Gets or sets the cascading parameter for the form.
    /// </summary>
    [CascadingParameter]
    private ITnTForm? _tntForm { get; set; }

    /// <summary>
    ///     Sets the focus to the input element.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask SetFocusAsync() => Element.FocusAsync();

    /// <summary>
    ///     Determines whether the input is required.
    /// </summary>
    /// <returns><c>true</c> if the input is required; otherwise, <c>false</c>.</returns>
    protected bool IsRequired() => AdditionalAttributes?.TryGetValue("required", out var _) == true || GetCustomAttributeIfExists<RequiredAttribute>() is not null;

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        if (StartIcon is not null) {
            StartIcon.AdditionalClass = "tnt-start-icon";
        }

        if (EndIcon is not null) {
            EndIcon.AdditionalClass = "tnt-end-icon";
        }

        if (string.IsNullOrWhiteSpace(NameAttributeValue)) {
            // Workaround, since for some reason NameValueAttribute is not being set when rendering in WebAssembly mode
            var shouldGenerateName = typeof(InputBase<TInputType>).GetField("_shouldGenerateFieldNames", BindingFlags.Instance | BindingFlags.NonPublic);
            shouldGenerateName?.SetValue(this, true);
        }
    }

    /// <summary>
    ///     Updates the current value of the input asynchronously.
    /// </summary>
    /// <param name="newValue">The new value</param>
    protected async Task OnInputAsync(string? newValue) {
        CurrentValueAsString = newValue;
        await BindAfter.InvokeAsync(CurrentValue);
    }

    /// <summary>
    ///     Updates the current value of the input asynchronously when a change event occurs.
    /// </summary>
    /// <param name="args">The change event args.</param>
    protected virtual async Task OnChangeAsync(ChangeEventArgs args) {
        CurrentValue = args.Value is TInputType value ? value : default;
        await BindAfter.InvokeAsync(CurrentValue);
    }

    /// <summary>
    ///     Gets the custom attribute if it exists.
    /// </summary>
    /// <typeparam name="TCustomAttr">The type of the custom attribute.</typeparam>
    /// <returns>The custom attribute if it exists; otherwise, <c>null</c>.</returns>
    private TCustomAttr? GetCustomAttributeIfExists<TCustomAttr>() where TCustomAttr : Attribute {
        if (ValueExpression is not null) {
            var body = ValueExpression.Body;

            // Unwrap casts to object
            if (body is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert
                && unaryExpression.Type == typeof(object)) {
                body = unaryExpression.Operand;
            }

            switch (body) {
                case MemberExpression memberExpression:
                    return Attribute.GetCustomAttribute(memberExpression.Member, typeof(TCustomAttr)) as TCustomAttr;

                case MethodCallExpression methodCallExpression:
                    return Attribute.GetCustomAttribute(methodCallExpression.Method, typeof(TCustomAttr)) as TCustomAttr;
            }
        }
        return null;
    }

    /// <summary>
    ///     Gets the maximum length of the input.
    /// </summary>
    /// <returns>The maximum length of the input.</returns>
    protected int? GetMaxLength() {
        if (AdditionalAttributes?.TryGetValue("maxlength", out var maxLength) == true && int.TryParse(maxLength?.ToString(), out var result)) {
            return result;
        }
        var maxLengthAttr = GetCustomAttributeIfExists<MaxLengthAttribute>();
        if (maxLengthAttr is not null) {
            return maxLengthAttr.Length;
        }

        if (typeof(TInputType) == typeof(string)) {
            var strLengthAttr = GetCustomAttributeIfExists<StringLengthAttribute>();
            if (strLengthAttr is not null) {
                return strLengthAttr.MaximumLength;
            }
        }

        return null;
    }

    /// <summary>
    ///     Gets the maximum value of the input.
    /// </summary>
    /// <returns>The maximum value of the input.</returns>
    protected string? GetMaxValue() {
        if (AdditionalAttributes?.TryGetValue("max", out var max) == true) {
            return max?.ToString();
        }
        var rangeAttr = GetCustomAttributeIfExists<RangeAttribute>();
        return rangeAttr?.Maximum.ToString();
    }

    /// <summary>
    ///     Gets the minimum length of the input.
    /// </summary>
    /// <returns>The minimum length of the input.</returns>
    protected int? GetMinLength() {
        if (AdditionalAttributes?.TryGetValue("minlength", out var minLength) == true && int.TryParse(minLength?.ToString(), out var result)) {
            return result;
        }
        var minLengthAttr = GetCustomAttributeIfExists<MinLengthAttribute>();
        if (minLengthAttr is not null) {
            return minLengthAttr.Length;
        }

        if (typeof(TInputType) == typeof(string)) {
            var strLengthAttr = GetCustomAttributeIfExists<StringLengthAttribute>();
            if (strLengthAttr is not null) {
                return strLengthAttr.MinimumLength;
            }
        }

        return null;
    }

    /// <summary>
    ///     Gets the minimum value of the input.
    /// </summary>
    /// <returns>The minimum value of the input.</returns>
    protected string? GetMinValue() {
        if (AdditionalAttributes?.TryGetValue("min", out var min) == true) {
            return min?.ToString();
        }
        var rangeAttr = GetCustomAttributeIfExists<RangeAttribute>();
        return rangeAttr?.Minimum.ToString();
    }
}

/// <summary>
///     Represents the different types of input elements that can be used in a form.
/// </summary>
public enum InputType {

    /// <summary>
    ///     Represents a button input type.
    /// </summary>
    Button = 1,

    /// <summary>
    ///     Represents a checkbox input type.
    /// </summary>
    Checkbox,

    /// <summary>
    ///     Represents a color input type.
    /// </summary>
    Color,

    /// <summary>
    ///     Represents a date input type.
    /// </summary>
    Date,

    /// <summary>
    ///     Represents a datetime input type.
    /// </summary>
    DateTime,

    /// <summary>
    ///     Represents a datetime-local input type.
    /// </summary>
    DateTimeLocal = DateTime,

    /// <summary>
    ///     Represents an email input type.
    /// </summary>
    Email,

    /// <summary>
    ///     Represents a file input type.
    /// </summary>
    File,

    /// <summary>
    ///     Represents a hidden input type.
    /// </summary>
    Hidden,

    /// <summary>
    ///     Represents an image input type.
    /// </summary>
    Image,

    /// <summary>
    ///     Represents a month input type.
    /// </summary>
    Month,

    /// <summary>
    ///     Represents a number input type.
    /// </summary>
    Number,

    /// <summary>
    ///     Represents a password input type.
    /// </summary>
    Password,

    /// <summary>
    ///     Represents a radio input type.
    /// </summary>
    Radio,

    /// <summary>
    ///     Represents a range input type.
    /// </summary>
    Range,

    /// <summary>
    ///     Represents a search input type.
    /// </summary>
    Search,

    /// <summary>
    ///     Represents a telephone input type.
    /// </summary>
    Tel,

    /// <summary>
    ///     Represents a text input type.
    /// </summary>
    Text,

    /// <summary>
    ///     Represents a time input type.
    /// </summary>
    Time,

    /// <summary>
    ///     Represents a URL input type.
    /// </summary>
    Url,

    /// <summary>
    ///     Represents a week input type.
    /// </summary>
    Week,

    /// <summary>
    ///     Represents a textarea input type.
    /// </summary>
    TextArea,

    /// <summary>
    ///     Represents a currency input type.
    /// </summary>
    Currency,

    /// <summary>
    ///     Represents a select input type.
    /// </summary>
    Select
}

/// <summary>
///     Provides extension methods for the <see cref="InputType" /> enum.
/// </summary>
public static class InputTypeExt {

    /// <summary>
    ///     Converts the <see cref="InputType" /> to its corresponding string representation.
    /// </summary>
    /// <param name="inputType">The input type to convert.</param>
    /// <returns>The string representation of the input type.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the input type is not valid.</exception>
    public static string ToInputTypeString(this InputType inputType) {
        return inputType switch {
            InputType.Button => "button",
            InputType.Checkbox => "checkbox",
            InputType.Color => "color",
            InputType.Date => "date",
            InputType.DateTime => "datetime-local",
            InputType.Email => "email",
            InputType.File => "file",
            InputType.Hidden => "hidden",
            InputType.Image => "image",
            InputType.Month => "month",
            InputType.Number => "number",
            InputType.Password => "password",
            InputType.Radio => "radio",
            InputType.Range => "range",
            InputType.Search => "search",
            InputType.Tel => "tel",
            InputType.Text => "text",
            InputType.Time => "time",
            InputType.Url => "url",
            InputType.Week => "week",
            InputType.Currency => "text",
            _ => throw new InvalidOperationException($"{inputType} is not a valid value of {nameof(InputType)}")
        };
    }
}