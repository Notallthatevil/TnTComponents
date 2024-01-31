using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTSideNav {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? Class => CssBuilder.Create()
                .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddElevation(Elevation)
        .Build();

    [Parameter]
    public int Elevation { get; set; } = 2;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSecondaryContainer;

    [Parameter]
    public string? Title { get; set; }

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetSideNav(this);
    }
}