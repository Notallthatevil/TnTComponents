using TnTComponents.Core;

namespace TnTComponents.Snackbar;

public class TnTSnackbarOptions {
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;
    public TnTBorderRadius? BorderRadius { get; set; } = new(2);
    public int Elevation { get; set; } = 2;
    public string Message { get; set; } = default!;
    public bool ShowClose { get; set; } = true;
    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;
    public int Timeout { get; set; } = 30;
    public string Title { get; set; } = default!;

    internal string GetClass() => CssClassBuilder.Create()
        .AddClass("tnt-snackbar")
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddBorderRadius(BorderRadius)
        .AddElevation(Elevation)
        .Build();
}