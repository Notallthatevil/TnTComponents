using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents the options for configuring a TnT dialog component.
/// </summary>
public class TnTDialogOptions {

    /// <inheritdoc />
    public TnTColor BackgroundColor { get; init; } = TnTColor.SurfaceContainerHighest;

    /// <summary>
    ///     Gets a value indicating whether the dialog should close when clicking outside of it.
    /// </summary>
    public bool CloseOnExternalClick { get; init; } = true;

    /// <summary>
    ///     Gets the custom CSS class for the dialog element.
    /// </summary>
    public string? ElementClass { get; init; }

    /// <summary>
    ///     Gets the custom style for the dialog element.
    /// </summary>
    public string? ElementStyle { get; init; }

    /// <summary>
    ///     Gets a value indicating whether the dialog should show a close button.
    /// </summary>
    public bool ShowCloseButton { get; init; } = true;

    /// <inheritdoc />
    public TextAlign? TextAlignment { get; }

    /// <inheritdoc />
    public TnTColor TextColor { get; init; } = TnTColor.OnSurface;

    /// <summary>
    ///     Gets the title of the dialog.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    ///     Gets or sets a value indicating whether the dialog is in the process of closing.
    /// </summary>
    internal bool Closing { get; set; }

    internal string? DialogCssClass => CssClassBuilder.Create()
        .AddClass("tnt-dialog")
        .AddClass("tnt-closing", Closing)
        .AddClass(ElementClass)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddTextAlign(TextAlignment)
        .Build();

    internal string? DialogCssStyle => CssStyleBuilder.Create()
        .Add(ElementStyle!)
        .AddVariable("tnt-dialog-bg-color", BackgroundColor)
        .AddVariable("tnt-dialog-fg-color", TextColor)
        .Build();
}