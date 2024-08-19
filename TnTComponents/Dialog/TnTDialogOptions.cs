using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTDialogOptions : ITnTStyleable {
    public TextAlign? TextAlignment { get; }
    public TnTColor BackgroundColor { get; init; } = TnTColor.SurfaceContainerHighest;
    public TnTColor TextColor { get; init; } = TnTColor.OnSurface;
    public int Elevation { get; init; } = 2;
    public TnTBorderRadius? BorderRadius { get; init; } = new(2);
    public string? ElementStyle { get; init; }
    public string? ElementClass { get; init; }
    public bool CloseOnExternalClick { get; init; } = true;
    public bool ShowCloseButton { get; init; } = true;
    public string? Title { get; init; }
    internal bool Closing { get; set; }
}