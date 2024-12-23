namespace TnTComponents;

/// <summary>
///     Specifies the type of text input.
/// </summary>
public enum TextInputType {
    Text,
    Email,
    Password,
    Tel,
    Url,
    Search
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
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the <paramref name="textInputType" /> is not a valid value.
    /// </exception>
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