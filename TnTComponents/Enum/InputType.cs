using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents;
/// <summary>
/// Represents the different types of input elements that can be used in a form.
/// </summary>
public enum InputType {
    /// <summary>
    /// Represents a button input type.
    /// </summary>
    Button = 1,
    /// <summary>
    /// Represents a checkbox input type.
    /// </summary>
    Checkbox,
    /// <summary>
    /// Represents a color input type.
    /// </summary>
    Color,
    /// <summary>
    /// Represents a date input type.
    /// </summary>
    Date,
    /// <summary>
    /// Represents a datetime input type.
    /// </summary>
    DateTime,
    /// <summary>
    /// Represents a datetime-local input type.
    /// </summary>
    DateTimeLocal = DateTime,
    /// <summary>
    /// Represents an email input type.
    /// </summary>
    Email,
    /// <summary>
    /// Represents a file input type.
    /// </summary>
    File,
    /// <summary>
    /// Represents a hidden input type.
    /// </summary>
    Hidden,
    /// <summary>
    /// Represents an image input type.
    /// </summary>
    Image,
    /// <summary>
    /// Represents a month input type.
    /// </summary>
    Month,
    /// <summary>
    /// Represents a number input type.
    /// </summary>
    Number,
    /// <summary>
    /// Represents a password input type.
    /// </summary>
    Password,
    /// <summary>
    /// Represents a radio input type.
    /// </summary>
    Radio,
    /// <summary>
    /// Represents a range input type.
    /// </summary>
    Range,
    /// <summary>
    /// Represents a search input type.
    /// </summary>
    Search,
    /// <summary>
    /// Represents a telephone input type.
    /// </summary>
    Tel,
    /// <summary>
    /// Represents a text input type.
    /// </summary>
    Text,
    /// <summary>
    /// Represents a time input type.
    /// </summary>
    Time,
    /// <summary>
    /// Represents a URL input type.
    /// </summary>
    Url,
    /// <summary>
    /// Represents a week input type.
    /// </summary>
    Week,
    /// <summary>
    /// Represents a textarea input type.
    /// </summary>
    TextArea,
    /// <summary>
    /// Represents a currency input type.
    /// </summary>
    Currency,
    /// <summary>
    /// Represents a select input type.
    /// </summary>
    Select
}

/// <summary>
/// Provides extension methods for the <see cref="InputType"/> enum.
/// </summary>
public static class InputTypeExt {
    /// <summary>
    /// Converts the <see cref="InputType"/> to its corresponding string representation.
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
