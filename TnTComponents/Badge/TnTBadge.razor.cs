using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Security.Cryptography.X509Certificates;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTBadge {

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Error;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; } = TnTBorderRadius.Full;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-badge")
        .AddFilled()
        .AddTnTStyleable(this)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public int Elevation { get; set; } = 2;

    [Parameter]
    public TextAlign? TextAlignment { get; set; } = TextAlign.Center;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnError;

    public static TnTBadge CreateBadge(string content, TnTColor backgroundColor = TnTColor.Error, int elevation = 2, TextAlign? textAlignment = TextAlign.Center, TnTColor textColor = TnTColor.OnError, string? cssClass = null, string? cssStyle = null) {
        return CreateBadge(content, TnTBorderRadius.Full, backgroundColor, elevation, textAlignment, textColor, cssClass, cssStyle);

    }
    public static TnTBadge CreateBadge(string content, TnTBorderRadius? borderRadius, TnTColor backgroundColor = TnTColor.Error, int elevation = 2, TextAlign? textAlignment = TextAlign.Center, TnTColor textColor = TnTColor.OnError, string? cssClass = null, string? cssStyle = null) {
        Dictionary<string, object> additionalAttributes = [];
        if (!string.IsNullOrWhiteSpace(cssClass)) {
            additionalAttributes.Add("class", cssClass);
        }

        if (!string.IsNullOrWhiteSpace(cssStyle)) {
            additionalAttributes.Add("style", cssStyle);
        }
        return new TnTBadge {
            BackgroundColor = backgroundColor,
            BorderRadius = borderRadius,
            ChildContent = new RenderFragment(builder => {
                builder.AddContent(0, content);
            }),
            Elevation = elevation,
            TextAlignment = textAlignment,
            TextColor = textColor,
            AdditionalAttributes = additionalAttributes
        };
    }

    internal RenderFragment Render => BuildRenderTree;
}