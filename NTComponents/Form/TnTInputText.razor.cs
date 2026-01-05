using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NTComponents;

/// <summary>
///     Specifies the type of text input.
/// </summary>
public enum TextInputType {

    /// <summary>
    ///     Represents a standard text input field for general text entry.
    /// </summary>
    Text,

    /// <summary>
    ///     Represents an input field optimized for email address entry, typically providing email-specific validation and keyboard on mobile devices.
    /// </summary>
    Email,

    /// <summary>
    ///     Represents a password input field where entered characters are obscured for security purposes.
    /// </summary>
    Password,

    /// <summary>
    ///     Represents an input field optimized for telephone number entry, typically showing a numeric keypad on mobile devices.
    /// </summary>
    Tel,

    /// <summary>
    ///     Represents an input field optimized for URL entry, typically providing URL-specific validation and keyboard features.
    /// </summary>
    Url,

    /// <summary>
    ///     Represents a search input field, often styled differently and may include features like a clear button on supported browsers.
    /// </summary>
    Search
}

/// <summary>
///     Represents a text input component with various text input types.
/// </summary>
public partial class TnTInputText {

    /// <inheritdoc />
    [Parameter]
    public TextInputType InputType { get; set; } = TextInputType.Text;

    /// <inheritdoc />
    public override InputType Type => InputType.ToInputType();

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out string? result, [NotNullWhen(false)] out string? validationErrorMessage) {
        result = value;
        validationErrorMessage = null;
        return true;
    }
}

/// <summary>
///     Provides extension methods for the <see cref="TextInputType" /> enum.
/// </summary>
public static class TextInputTypeExt {

    /// <summary>
    ///     Converts a <see cref="TextInputType" /> to an <see cref="InputType" />.
    /// </summary>
    /// <param name="textInputType">The text input type to convert.</param>
    /// <returns>The corresponding <see cref="InputType" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <paramref name="textInputType" /> is not a valid value.</exception>
    public static InputType ToInputType(this TextInputType textInputType) {
        return textInputType switch {
            TextInputType.Text => InputType.Text,
            TextInputType.Email => InputType.Email,
            TextInputType.Password => InputType.Password,
            TextInputType.Tel => InputType.Tel,
            TextInputType.Url => InputType.Url,
            TextInputType.Search => InputType.Search,
            _ => throw new InvalidOperationException($"{textInputType} is not a valid value for {nameof(TextInputType)}")
        };
    }
}