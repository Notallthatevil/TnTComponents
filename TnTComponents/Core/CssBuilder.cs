﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Color;
using TnTComponents.Layout;

namespace TnTComponents.Core;
internal class CssBuilder {
    public static CssBuilder Create(string defaultClass = "tnt-components") => new CssBuilder().AddClass(defaultClass);

    private CssBuilder() { }

    private readonly StringBuilder _stringBuilder = new();


    public CssBuilder AddClass(string className, bool? when = true) {
        if (!string.IsNullOrWhiteSpace(className) && when == true) {
            _stringBuilder.Append(' ').Append(className);
        }
        return this;
    }

    public CssBuilder AddClass(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("class", out var @class) == true) {
            _stringBuilder.AppendJoin(' ', @class);
        }
        return this;
    }

    public CssBuilder AddElevation(int elevation) => AddClass($"tnt-elevation-{Math.Clamp(elevation, 0, 10)}", elevation >= 0);

    public CssBuilder AddRipple(bool enabled = true) => AddClass("tnt-ripple", enabled);

    public CssBuilder AddBackgroundColor(TnTColorEnum? color) => AddClass($"tnt-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null);
    public CssBuilder AddForegroundColor(TnTColorEnum? color) => AddClass($"tnt-fg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null);

    public CssBuilder AddOutlined(bool enabled = true) => AddClass("tnt-outlined", enabled);
    public CssBuilder AddNoBackground(bool enabled = true) => AddClass("tnt-no-background", enabled);

    public CssBuilder AddCornerRadius(TnTCornerRadius? tntCornerRadius) => tntCornerRadius.HasValue ?
        tntCornerRadius.Value.AllSame ? AddClass($"tnt-corner-radius-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0) :
            AddClass($"tnt-corner-radius-start-start-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0)
            .AddClass($"tnt-corner-radius-start-end-{Math.Clamp(tntCornerRadius.Value.StartEnd, 0, 10)}", tntCornerRadius.Value.StartEnd >= 0)
            .AddClass($"tnt-corner-radius-end-start-{Math.Clamp(tntCornerRadius.Value.EndStart, 0, 10)}", tntCornerRadius.Value.EndStart >=0)
            .AddClass($"tnt-corner-radius-end-end-{Math.Clamp(tntCornerRadius.Value.EndEnd, 0, 10)}", tntCornerRadius.Value.EndEnd >= 0)
        : this;



    public string Build() => _stringBuilder.ToString();

}

