using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTBody {

    public override string? Class => CssBuilder.Create()
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(DefaultTextColor)
        .Build();

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Background;

    [Parameter]
    public TnTColor DefaultTextColor { get; set; } = TnTColor.OnBackground;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetBody(this);
    }
}