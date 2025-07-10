namespace TnTComponents;

/// <summary>
///     Represents the types of buttons that can be used in HTML forms.
/// </summary>
public enum ButtonType {

    /// <summary>
    ///     Represents a standard button with no default behavior. Corresponds to type="button" in HTML.
    /// </summary>
    Button,

    /// <summary>
    ///     Represents a submit button that submits the form data. Corresponds to type="submit" in HTML.
    /// </summary>
    Submit,

    /// <summary>
    ///     Represents a reset button that resets all form controls to their initial values. Corresponds to type="reset" in HTML.
    /// </summary>
    Reset
}

/// <summary>
///     Provides extension methods for the <see cref="ButtonType" /> enum.
/// </summary>
public static class ButtonTypeExt {

    /// <summary>
    ///     Converts the <see cref="ButtonType" /> to its corresponding HTML attribute value.
    /// </summary>
    /// <param name="buttonType">The <see cref="ButtonType" /> to convert.</param>
    /// <returns>A string representing the HTML attribute value for the specified <see cref="ButtonType" />.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <see cref="ButtonType" /> is not valid.</exception>
    public static string ToHtmlAttribute(this ButtonType buttonType) {
        return buttonType switch {
            ButtonType.Button => "button",
            ButtonType.Submit => "submit",
            ButtonType.Reset => "reset",
            _ => throw new InvalidOperationException($"{buttonType} is not a valid for the enum {nameof(ButtonType)}")
        };
    }
}