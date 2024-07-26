using Microsoft.AspNetCore.Components;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTHeader {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.SurfaceContainer;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
                .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
           .AddFromAdditionalAttributes(AdditionalAttributes)
           .Build();

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnSurface;

}