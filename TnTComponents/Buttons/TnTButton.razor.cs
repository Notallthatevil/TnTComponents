using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;
using TnTComponents.Color;
using TnTComponents.Common;
using TnTComponents.Core;
using TnTComponents.Enum;

namespace TnTComponents;
public partial class TnTButton {

    private static readonly TnTColorEnum DefaultBg = TnTColorEnum.Primary;
    private static readonly TnTColorEnum DefaultTextColor = TnTColorEnum.OnPrimary;

    [Parameter]
    public ButtonAppearance Appearance { get; set; }

    [Parameter]
    public ButtonType Type { get; set; }

    [Parameter]
    public TnTColorEnum? BackgroundColor { get; set; } = DefaultBg;

    [Parameter]
    public TnTColorEnum? TextColor { get; set; } = DefaultTextColor;

    [Parameter]
    public int Elevation { get; set; } = 1;

    [Parameter]
    public TnTCornerRadius CornerRadius { get; set; } = new(10);

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    protected override bool RunIsolatedJsScript => true;

    public override string? Class => CssBuilder.Create()
        .AddClass(AdditionalAttributes)
        .AddElevation(Elevation)
        .AddBackgroundColor(BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddCornerRadius(CornerRadius)
        .AddRipple()
        .Build();


    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}

internal static class TnTButtonCssClassExt {
    public static CssBuilder AddButtonAppearance(this CssBuilder builder, ButtonAppearance buttonAppearance) {
        return builder.AddOutlined(buttonAppearance == ButtonAppearance.Outlined)
            .AddNoBackground(buttonAppearance == ButtonAppearance.Text);
    }
}
