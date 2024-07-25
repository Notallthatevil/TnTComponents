using TnTComponents.Core;

namespace TnTComponents;

public class TnTDialogOptions {
    public TnTBorderRadius? BorderRadius { get; init; } = new(3);
    public bool CloseOnExternalClick { get; init; } = true;
    public TnTColor DialogBackgroundColor { get; init; } = TnTColor.SurfaceContainer;
    public int Elevation { get; init; } = 2;
    public bool OverlayBlur { get; init; } = true;
    public TnTColor OverlayColor { get; init; } = TnTColor.OnSurfaceVariant;
    public double? OverlayOpacity { get; init; } = 0.5;
    public bool ShowClose { get; init; } = true;
    public bool ShowTitle { get; init; } = true;
    public string? Style { get; init; }
    public TnTColor TextColor { get; init; } = TnTColor.OnSurface;
    public string? Title { get; init; }
    internal bool Closing { get; set; }

    internal string GetDialogClass() => CssClassBuilder.Create()
        .AddClass("tnt-dialog-container")
        .AddClass("tnt-grow", !Closing)
        .AddBorderRadius(BorderRadius)
        .AddBackgroundColor(DialogBackgroundColor)
        .AddForegroundColor(TextColor)
        .AddElevation(Elevation)
        .Build();

    internal string GetOverlayClass() => CssClassBuilder.Create()
        .AddClass("tnt-dialog")
        .Build();
}