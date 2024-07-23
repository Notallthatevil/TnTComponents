using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Reflection.Metadata;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents;

public partial class TnTButton {

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddOutlined(Appearance == ButtonAppearance.Outlined)
        .AddFilled(Appearance == ButtonAppearance.Filled)
        .AddTnTStyleable(this, enableElevation: Appearance != ButtonAppearance.Outlined && Appearance != ButtonAppearance.Text)
        .AddTnTInteractable(this)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .Build();

    [Parameter]
    public ButtonType Type { get; set; }

    [Parameter]
    public ButtonAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public TnTColor TintColor { get; set; } = TnTColor.SurfaceTint;

    [Parameter]
    public TnTColor TextColor { get; set; } = TnTColor.OnPrimary;

    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    [Parameter]
    public virtual int Elevation { get; set; } = 1;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public virtual TnTBorderRadius? BorderRadius { get; set; } = TnTBorderRadius.Full;

    [Parameter]
    public bool EnableRipple { get; set; } = true;

    public override string? JsModulePath => "./_content/TnTComponents/Buttons/TnTButton.razor.js";

    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }
}