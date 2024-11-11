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
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public ButtonType Type { get; set; }

    [Parameter]
    public virtual ButtonAppearance Appearance { get; set; }

    [Parameter]
    public TnTColor BackgroundColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public override TnTColor? TintColor { get; set; } = TnTColor.SurfaceTint;

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

    public override string? JsModulePath => "./_content/TnTComponents/Buttons/TnTButton.razor.js";

    [Parameter]
    public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

    [Parameter]
    public bool StopPropagation { get; set; }

    private RenderFragment Button => b => {
        b.OpenElement(0, "button");
        b.AddMultipleAttributes(10, AdditionalAttributes);
        b.AddAttribute(20, "class", ElementClass);
        b.AddAttribute(30, "style", ElementStyle);
        b.AddAttribute(40, "type", Type.ToHtmlAttribute());
        b.AddAttribute(50, "name", ElementName);
        b.AddAttribute(60, "disabled", Disabled);
        b.AddAttribute(70, "autofocus", AutoFocus);
        b.AddAttribute(80, "title", ElementTitle);
        b.AddAttribute(90, "id", ElementId);
        if (OnClickCallback.HasDelegate) {
            b.AddAttribute(100, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnClickCallback));
        }

        if (StopPropagation) {
            b.AddAttribute(110, "onclick:stopPropagation", true);
        }

        b.AddElementReferenceCapture(120, __value => Element = __value);
        b.AddContent(130, ChildContent);

        b.CloseElement();
    };
}