using Microsoft.AspNetCore.Components;

namespace TnTComponents.Form;

/// <summary>
///     Represents a form item with various properties for appearance, state, and behavior.
/// </summary>
public interface IFormItem {

    /// <summary>
    ///     Gets or sets additional attributes for the form item.
    /// </summary>
    IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    ///     Gets or sets the appearance of the form item.
    /// </summary>
    FormAppearance Appearance { get; set; }

    /// <summary>
    ///     Gets or sets the background color of the form item.
    /// </summary>
    TnTColor? BackgroundColor { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the form item is disabled.
    /// </summary>
    bool Disabled { get; set; }

    /// <summary>
    ///     Gets the element reference of the form item.
    /// </summary>
    ElementReference Element { get; }

    /// <summary>
    ///     Gets the CSS class for the form item.
    /// </summary>
    string FormCssClass { get; }

    /// <summary>
    ///     Gets the CSS style for the form item.
    /// </summary>
    string? FormCssStyle { get; }

    /// <summary>
    ///     Gets or sets the parent form of the form item.
    /// </summary>
    TnTForm? ParentForm { get; set; }

    /// <summary>
    ///     Gets the appearance of the parent form.
    /// </summary>
    FormAppearance? ParentFormAppearance { get; }

    /// <summary>
    ///     Gets a value indicating whether the parent form is disabled.
    /// </summary>
    bool? ParentFormDisabled { get; }

    /// <summary>
    ///     Gets a value indicating whether the parent form is read-only.
    /// </summary>
    bool? ParentFormReadOnly { get; }

    /// <summary>
    ///     Gets or sets a value indicating whether the form item is read-only.
    /// </summary>
    bool ReadOnly { get; set; }

    /// <summary>
    ///     Gets or sets the text color of the form item.
    /// </summary>
    TnTColor? TextColor { get; set; }
}