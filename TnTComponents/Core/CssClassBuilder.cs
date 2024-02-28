namespace TnTComponents.Core;

internal class CssClassBuilder {
    private readonly HashSet<string> _classes = [];

    private CssClassBuilder() {
    }

    public static CssClassBuilder Create(string defaultClass = "tnt-components") => new CssClassBuilder().AddClass(defaultClass);

    public CssClassBuilder AddActionableBackgroundColor(TnTColor? color) => AddClass($"tnt-actionable-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    public CssClassBuilder AddBackgroundColor(TnTColor? color) => AddClass($"tnt-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    public CssClassBuilder AddBorderRadius(TnTBorderRadius? tntCornerRadius) => tntCornerRadius.HasValue ?
        tntCornerRadius.Value.AllSame ? AddClass($"tnt-corner-radius-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0) :
            AddClass($"tnt-corner-radius-start-start-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0)
            .AddClass($"tnt-corner-radius-start-end-{Math.Clamp(tntCornerRadius.Value.StartEnd, 0, 10)}", tntCornerRadius.Value.StartEnd >= 0)
            .AddClass($"tnt-corner-radius-end-start-{Math.Clamp(tntCornerRadius.Value.EndStart, 0, 10)}", tntCornerRadius.Value.EndStart >= 0)
            .AddClass($"tnt-corner-radius-end-end-{Math.Clamp(tntCornerRadius.Value.EndEnd, 0, 10)}", tntCornerRadius.Value.EndEnd >= 0)
        : this;

    public CssClassBuilder AddClass(string? className, bool? when = true) {
        if (!string.IsNullOrWhiteSpace(className) && when == true) {
            _classes.Add(className);
        }
        return this;
    }

    public CssClassBuilder AddClass(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("class", out var @class) == true) {
            var classes = @class.ToString();
            if (!string.IsNullOrWhiteSpace(classes)) {
                _classes.Add(classes);
            }
        }
        return this;
    }

    public CssClassBuilder AddElevation(int elevation) => AddClass($"tnt-elevation-{Math.Clamp(elevation, 0, 10)}", elevation >= 0);

    public CssClassBuilder AddFilled(bool enabled = true) => AddClass("tnt-filled", enabled);

    public CssClassBuilder AddFlexBox(LayoutDirection? direction = null,
        AlignItems? alignItems = null,
        JustifyContent? justifyContent = null,
        AlignContent? alignContent = null,
        bool enabled = true) => enabled ? AddClass("tnt-flex", enabled && direction != null && alignItems != null && justifyContent != null && alignContent != null)
        .AddLayoutDirection(direction)
        .AddAlignItems(alignItems)
        .AddJustifyContent(justifyContent)
        .AddAlignContent(alignContent) :
        this;

    public CssClassBuilder AddFlexBox(ITnTFlexBox flexBox, bool enabled = true) => AddFlexBox(flexBox.Direction, flexBox.AlignItems, flexBox.JustifyContent, flexBox.AlignContent, enabled);

    public CssClassBuilder AddForegroundColor(TnTColor? color) => AddClass($"tnt-fg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    public CssClassBuilder AddMargin(int margin) => AddClass($"tnt-margin-{Math.Clamp(margin, 1, 16)}", margin > 0);

    public CssClassBuilder AddNoBackground(bool enabled = true) => AddClass("tnt-bg-color-transparent", enabled);

    public CssClassBuilder AddOutlined(bool enabled = true) => enabled ? AddClass("tnt-outlined", enabled).AddBackgroundColor(TnTColor.Transparent) : this;

    public CssClassBuilder AddPadding(int padding) => AddClass($"tnt-padding-{Math.Clamp(padding, 1, 16)}", padding > 0);

    public CssClassBuilder AddRipple(bool enabled = true) => AddClass("tnt-ripple", enabled);

    public CssClassBuilder AddFromAdditionalAttributes(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("class", out var @class) == true) {
            return AddClass(@class.ToString());
        }
        return this;
    }
    public string Build() => string.Join(' ', _classes).Trim();

    public CssClassBuilder SetAlternative(bool enabled = true) => AddClass("tnt-alternative", enabled);

    public CssClassBuilder SetDisabled(bool disabled) => AddClass("tnt-disabled", disabled);

    private CssClassBuilder AddAlignContent(AlignContent? alignContent) => AddClass($"tnt-align-content-{alignContent?.ToCssString()}", alignContent != null);

    private CssClassBuilder AddAlignItems(AlignItems? alignItems) => AddClass($"tnt-align-item-{alignItems?.ToCssString()}", alignItems != null);

    private CssClassBuilder AddJustifyContent(JustifyContent? justifyContent) => AddClass($"tnt-justify-content-{justifyContent?.ToCssString()}", justifyContent != null);

    private CssClassBuilder AddLayoutDirection(LayoutDirection? direction) => AddClass($"tnt-direction-{direction?.ToCssString()}", direction != null);

}