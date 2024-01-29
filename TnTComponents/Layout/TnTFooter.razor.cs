using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTFooter {

    public override string? Class => CssBuilder.Create()
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetHeader(this);
    }
}