using TnTComponents.Core;

namespace TnTComponents.Dialog;

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

    internal string GetDialogClass() => CssBuilder.Create()
        .AddClass("tnt-dialog-container")
        .AddBorderRadius(BorderRadius)
        .AddBackgroundColor(DialogBackgroundColor)
        .AddForegroundColor(TextColor)
        .AddElevation(Elevation)
        .Build();

    internal string GetOverlayClass() => CssBuilder.Create()
        .AddClass("tnt-dialog")
        .Build();
}