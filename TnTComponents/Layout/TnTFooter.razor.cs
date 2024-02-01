using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTFooter {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceVariant;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? Class => CssClassBuilder.Create()
                .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurfaceVariant;

    [CascadingParameter]
    private TnTLayout _layout { get; set; } = default!;

    protected override void OnInitialized() {
        base.OnInitialized();
        if (_layout is null) {
            throw new InvalidOperationException($"{nameof(TnTHeader)} must be a descendant of {nameof(TnTLayout)}");
        }
        _layout.SetHeader(this);
    }
}