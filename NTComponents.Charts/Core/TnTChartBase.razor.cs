using Microsoft.AspNetCore.Components;
using SkiaSharp.Views.Blazor;

namespace NTComponents.Charts.Core;

public abstract partial class TnTChartBase {
    [Parameter, EditorRequired]
    public string Title { get; set;  }
    protected abstract void Render(SKPaintGLSurfaceEventArgs e);


    private async void OnPaintSurface(SKPaintGLSurfaceEventArgs e) {
        Render(e);
    }

}
