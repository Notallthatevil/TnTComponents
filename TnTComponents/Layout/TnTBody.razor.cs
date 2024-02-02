using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTBody {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Background;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? CssClass => CssClassBuilder.Create()
                .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(DefaultTextColor)
        .Build();

    [Parameter]
    public TnTColor DefaultTextColor { get; set; } = TnTColor.OnBackground;

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetBody(this);
    }
}