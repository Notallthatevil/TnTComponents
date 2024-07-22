using TnTComponents.Interfaces;

namespace TnTComponents.Core;

internal class CssClassBuilder {
    private readonly HashSet<string> _classes = [];

    private CssClassBuilder() {
    }

    public static CssClassBuilder Create(string defaultClass = "tnt-components") => new CssClassBuilder().AddClass(defaultClass);

    public CssClassBuilder AddClass(string? className, bool? when = true) {
        if (!string.IsNullOrWhiteSpace(className) && when == true) {
            _classes.Add(className);
        }
        return this;
    }
    public CssClassBuilder AddOutlined(bool enabled = true) => enabled ? AddClass("tnt-outlined", enabled) : this;
    public CssClassBuilder AddFilled(bool enabled = true) => AddClass("tnt-filled", enabled);
    public CssClassBuilder AddBackgroundColor(TnTColor? color) => AddClass($"tnt-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);
    public CssClassBuilder AddTintColor(TnTColor? color) => AddClass($"tnt-tint-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);
    public CssClassBuilder AddForegroundColor(TnTColor? color) => AddClass($"tnt-fg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);
    public CssClassBuilder AddTextAlign(TextAlign? textAlign) => AddClass("tnt-text-align-" + textAlign.ToCssString(), textAlign != null);
    public CssClassBuilder AddDisabled(bool disabled) => AddClass("tnt-disabled", disabled);
    public CssClassBuilder AddElevation(int elevation) => AddClass($"tnt-elevation-{Math.Clamp(elevation, 0, 10)}", elevation >= 0);

    public CssClassBuilder AddBorderRadius(TnTBorderRadius? tntCornerRadius) => tntCornerRadius.HasValue ?
    tntCornerRadius.Value.AllSame ? AddClass($"tnt-corner-radius-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0) :
        AddClass($"tnt-corner-radius-start-start-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0)
        .AddClass($"tnt-corner-radius-start-end-{Math.Clamp(tntCornerRadius.Value.StartEnd, 0, 10)}", tntCornerRadius.Value.StartEnd >= 0)
        .AddClass($"tnt-corner-radius-end-start-{Math.Clamp(tntCornerRadius.Value.EndStart, 0, 10)}", tntCornerRadius.Value.EndStart >= 0)
        .AddClass($"tnt-corner-radius-end-end-{Math.Clamp(tntCornerRadius.Value.EndEnd, 0, 10)}", tntCornerRadius.Value.EndEnd >= 0)
    : this;
    public CssClassBuilder AddRipple(bool enabled = true) => AddClass("tnt-ripple", enabled);




    public CssClassBuilder AddTnTStyleable(ITnTStyleable styleable, bool enableBackground = true, bool enableForeground = true, bool enableTint = true, bool enableElevation = true, bool enableBorderRadius = true) {
        AddBackgroundColor(enableBackground ? styleable.BackgroundColor : null);
        AddForegroundColor(enableForeground ? styleable.TextColor : null);
        AddTintColor(enableTint ? styleable.TintColor : null);
        AddTextAlign(styleable.TextAlignment);
        if (enableElevation) {
            AddElevation(styleable.Elevation);
        }
        AddBorderRadius(enableBorderRadius ? styleable.BorderRadius : null);
        AddRipple(styleable.EnableRipple);
        return this;
    }

    public CssClassBuilder AddTnTInteractable(ITnTInteractable interactable) {
        AddDisabled(interactable.Disabled);
        AddClass("tnt-interactable");
        return this;
    }





    public CssClassBuilder AddActionableBackgroundColor(TnTColor? color) => AddClass($"tnt-actionable-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);







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


    public CssClassBuilder AddFromAdditionalAttributes(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("class", out var @class) == true && @class is not null) {
            return AddClass(@class.ToString());
        }
        return this;
    }

    public CssClassBuilder AddMargin(int margin) => AddClass($"tnt-margin-{Math.Clamp(margin, 1, 16)}", margin > 0);

    public CssClassBuilder AddNoBackground(bool enabled = true) => AddClass("tnt-bg-color-transparent", enabled);


    public CssClassBuilder AddPadding(int padding) => AddClass($"tnt-padding-{Math.Clamp(padding, 1, 16)}", padding > 0);


    public CssClassBuilder AddSize(Size? size) {
        if (size is null || size == Size.Default) {
            return this;
        }
        var sizeSuffix = size switch {
            Size.Smallest => "smallest",
            Size.Small => "small",
            Size.Large => "large",
            Size.Largest => "largest",
            _ => string.Empty
        };

        return AddClass($"tnt-size-{sizeSuffix}", size is not null && size != Size.Default);
    }


    public string Build() => string.Join(' ', _classes).Trim();

    public CssClassBuilder MakeGridContainer(bool enabled = true) => AddClass("tnt-grid-container", enabled);

    public CssClassBuilder MakeTextOnly(bool enabled = true) => AddClass("tnt-text-only", enabled);

    public CssClassBuilder SetAlternative(bool enabled = true) => AddClass("tnt-alternative", enabled);


    private CssClassBuilder AddAlignContent(AlignContent? alignContent) => AddClass($"tnt-align-content-{alignContent?.ToCssString()}", alignContent != null);

    private CssClassBuilder AddAlignItems(AlignItems? alignItems) => AddClass($"tnt-align-item-{alignItems?.ToCssString()}", alignItems != null);

    private CssClassBuilder AddJustifyContent(JustifyContent? justifyContent) => AddClass($"tnt-justify-content-{justifyContent?.ToCssString()}", justifyContent != null);

    private CssClassBuilder AddLayoutDirection(LayoutDirection? direction) => AddClass($"tnt-direction-{direction?.ToCssString()}", direction != null);
}