using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Enum;
using TnTComponents.Layout;

namespace TnTComponents.Core;
internal class CssBuilder {
    public static CssBuilder Create(string defaultClass = "tnt-components") => new CssBuilder().AddClass(defaultClass);

    private CssBuilder() { }

    private readonly HashSet<string> _classes = [];


    public CssBuilder AddClass(string className, bool? when = true) {
        if (!string.IsNullOrWhiteSpace(className) && when == true) {
            _classes.Add(className);
        }
        return this;
    }

    public CssBuilder AddClass(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("class", out var @class) == true) {
            var classes = @class.ToString();
            if (!string.IsNullOrWhiteSpace(classes)) {
                _classes.Add(classes);
            }
        }
        return this;
    }

    public CssBuilder AddElevation(int elevation) => AddClass($"tnt-elevation-{Math.Clamp(elevation, 0, 10)}", elevation >= 0);

    public CssBuilder AddRipple(bool enabled = true) => AddClass("tnt-ripple", enabled);

    public CssBuilder AddBackgroundColor(TnTColor? color) => AddClass($"tnt-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null);
    public CssBuilder AddForegroundColor(TnTColor? color) => AddClass($"tnt-fg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null);
    public CssBuilder AddOutlined(bool enabled = true) => enabled ? AddClass("tnt-outlined", enabled).AddBackgroundColor(TnTColor.Transparent) : this;
    public CssBuilder AddFilled(bool enabled = true) => AddClass("tnt-filled", enabled);
    public CssBuilder AddNoBackground(bool enabled = true) => AddClass("tnt-no-background", enabled);


    public CssBuilder AddFlexBox(LayoutDirection? direction = null,
        AlignItems? alignItems = null,
        JustifyContent? justifyContent = null,
        AlignContent? alignContent = null,
        bool enabled = true) => enabled ? AddClass("tnt-flex", enabled)
        .AddLayoutDirection(direction)
        .AddAlignItems(alignItems)
        .AddJustifyContent(justifyContent)
        .AddAlignContent(alignContent) :
        this;
    private CssBuilder AddLayoutDirection(LayoutDirection? direction) => AddClass($"tnt-direction-{direction?.ToCssString()}", direction != null);
    private CssBuilder AddAlignItems(AlignItems? alignItems) => AddClass($"tnt-align-item-{alignItems?.ToCssString()}", alignItems != null);
    private CssBuilder AddJustifyContent(JustifyContent? justifyContent) => AddClass($"tnt-justify-content-{justifyContent?.ToCssString()}", justifyContent != null);
    private CssBuilder AddAlignContent(AlignContent? alignContent) => AddClass($"tnt-align-content-{alignContent?.ToCssString()}", alignContent != null);


    public CssBuilder AddBorderRadius(TnTBorderRadius? tntCornerRadius) => tntCornerRadius.HasValue ?
        tntCornerRadius.Value.AllSame ? AddClass($"tnt-corner-radius-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0) :
            AddClass($"tnt-corner-radius-start-start-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0)
            .AddClass($"tnt-corner-radius-start-end-{Math.Clamp(tntCornerRadius.Value.StartEnd, 0, 10)}", tntCornerRadius.Value.StartEnd >= 0)
            .AddClass($"tnt-corner-radius-end-start-{Math.Clamp(tntCornerRadius.Value.EndStart, 0, 10)}", tntCornerRadius.Value.EndStart >= 0)
            .AddClass($"tnt-corner-radius-end-end-{Math.Clamp(tntCornerRadius.Value.EndEnd, 0, 10)}", tntCornerRadius.Value.EndEnd >= 0)
        : this;



    public string Build() => string.Join(' ', _classes).Trim();

}

