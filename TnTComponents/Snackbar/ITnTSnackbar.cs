using TnTComponents.Core;

namespace TnTComponents.Snackbar;

public interface ITnTSnackbar {
    TnTColor BackgroundColor { get; set; }
    TnTBorderRadius? BorderRadius { get; set; }
    int Elevation { get; set; }
    string? Message { get; set; }
    bool ShowClose { get; set; }
    TnTColor TextColor { get; set; }
    int Timeout { get; set; }
    string Title { get; set; }
}

internal static class ITnTSnackbarExt {

    internal static string GetClass(this ITnTSnackbar snackbar) => CssBuilder.Create()
        .AddClass("tnt-snackbar")
        .AddBackgroundColor(snackbar.BackgroundColor)
        .AddForegroundColor(snackbar.TextColor)
        .AddBorderRadius(snackbar.BorderRadius)
        .AddElevation(snackbar.Elevation)
        .Build();
}