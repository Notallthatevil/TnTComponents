using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTHeader {

    public override string? Class => CssBuilder.Create()
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

    //[Parameter]
    //public TnTColor ScrolledBackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetHeader(this);
    }
}