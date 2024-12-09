namespace TnTComponents.Interfaces;

/// <summary>
///     Interface representing a flexible box layout container.
/// </summary>
public interface ITnTFlexBox {

    /// <summary>
    ///     Gets or sets the alignment of content within the flex container.
    /// </summary>
    AlignContent? AlignContent { get; set; }

    /// <summary>
    ///     Gets or sets the alignment of items within the flex container.
    /// </summary>
    AlignItems? AlignItems { get; set; }

    /// <summary>
    ///     Gets or sets the layout direction of the flex container.
    /// </summary>
    LayoutDirection? Direction { get; set; }

    /// <summary>
    ///     Gets or sets the justification of content within the flex container.
    /// </summary>
    JustifyContent? JustifyContent { get; set; }
}
