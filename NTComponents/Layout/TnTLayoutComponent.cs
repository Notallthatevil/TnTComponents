using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTComponents.Core;
using NTComponents.Interfaces;

namespace NTComponents.Layout;

/// <summary>
///     A base layout class
/// </summary>
public abstract class TnTLayoutComponent : TnTComponentBase{

    /// <inheritdoc />
    [Parameter]
    public virtual TnTColor BackgroundColor { get; set; } = TnTColor.Background;

    /// <summary>
    ///     The child content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddTextAlign(TextAlignment)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddVariable("tnt-layout-background-color", BackgroundColor)
        .AddVariable("tnt-layout-text-color", TextColor)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public virtual int Elevation { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TextAlign? TextAlignment { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual TnTColor TextColor { get; set; } = TnTColor.OnBackground;

}