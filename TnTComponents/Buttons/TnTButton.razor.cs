using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection.Metadata;
using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTButton {

    [Parameter]
    public ButtonAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor? BackgroundColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public TnTBorderRadius CornerRadius { get; set; } = new(10);

    public override string? CssClass => CssClassBuilder.Create()
            .AddClass(AdditionalAttributes)
        .AddElevation(Elevation)
        .AddActionableBackgroundColor(Appearance == ButtonAppearance.Text || Appearance == ButtonAppearance.Outlined ? TnTColor.Transparent : BackgroundColor)
        .AddForegroundColor(TextColor)
        .AddBorderRadius(CornerRadius)
        .AddOutlined(Appearance == ButtonAppearance.Outlined)
        .MakeTextOnly(Appearance == ButtonAppearance.Text)
        .AddClass("tnt-button")
        .AddRipple()
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
       .AddFromAdditionalAttributes(AdditionalAttributes)
       .Build();

    [Parameter]
    public int Elevation { get; set; } = 1;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    [Parameter]
    public TnTColor? TextColor { get; set; } = TnTColor.OnPrimary;

    [Parameter]
    public ButtonType Type { get; set; }

    protected override string? JsModulePath => "./_content/TnTComponents/Buttons/TnTButton.razor.js";
    protected override bool RunIsolatedJsScript => true;

    protected override void OnInitialized() {
        base.OnInitialized();
        Name ??= ComponentIdentifier;
    }
}

internal static class TnTButtonCssClassExt {

    public static CssClassBuilder AddButtonAppearance(this CssClassBuilder builder, ButtonAppearance buttonAppearance) {
        return builder.AddOutlined(buttonAppearance == ButtonAppearance.Outlined)
            .AddNoBackground(buttonAppearance == ButtonAppearance.Text);
    }
}