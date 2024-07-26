using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Interfaces;

namespace TnTComponents.Layout;

public abstract class TnTLayoutComponent : TnTComponentBase, ITnTStyleable {

    [Parameter]
    public virtual TnTColor BackgroundColor { get; set; } = TnTColor.Background;

    [Parameter]
    public TnTBorderRadius? BorderRadius { get; set; }

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTnTStyleable(this)
        .AddFilled()
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    [Parameter]
    public virtual int Elevation { get; set; }

    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    [Parameter]
    public virtual TnTColor TextColor { get; set; } = TnTColor.OnBackground;

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;


    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "div");
        builder.AddAttribute(10, "data-permanent");
        builder.OpenRegion(20);

        {
            builder.OpenElement(0, "div");
            builder.AddMultipleAttributes(10, AdditionalAttributes);
            builder.AddAttribute(20, "class", ElementClass);
            builder.AddAttribute(30, "style", ElementStyle);
            builder.AddAttribute(40, "lang", ElementLang);
            builder.AddAttribute(50, "id", ElementId);
            builder.AddAttribute(60, "title", ElementTitle);
            builder.AddElementReferenceCapture(70, e => Element = e);
            builder.AddContent(80, ChildContent);
            builder.CloseElement();
        }

        builder.CloseRegion();
        builder.CloseElement();
    }

    private void RenderContent(RenderTreeBuilder builder) {

    }
}