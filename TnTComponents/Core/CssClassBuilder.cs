using TnTComponents.Interfaces;

namespace TnTComponents.Core;

/// <summary>
/// A builder class for constructing CSS class strings.
/// </summary>
internal class CssClassBuilder {
    private readonly HashSet<string> _classes = new();

    private CssClassBuilder() {
    }

    /// <summary>
    /// Creates a new instance of <see cref="CssClassBuilder"/> with an optional default class.
    /// </summary>
    /// <param name="defaultClass">The default CSS class to add.</param>
    /// <returns>A new instance of <see cref="CssClassBuilder"/>.</returns>
    public static CssClassBuilder Create(string defaultClass = "tnt-components") => new CssClassBuilder().AddClass(defaultClass);

    /// <summary>
    /// Adds a CSS class for actionable background color.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddActionableBackgroundColor(TnTColor? color) => AddClass($"tnt-actionable-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    /// <summary>
    /// Adds a CSS class for background color.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddBackgroundColor(TnTColor? color) => AddClass($"tnt-bg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    /// <summary>
    /// Adds CSS classes for border radius.
    /// </summary>
    /// <param name="tntCornerRadius">The border radius to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddBorderRadius(TnTBorderRadius? tntCornerRadius) => tntCornerRadius.HasValue ?
    tntCornerRadius.Value.AllSame ? AddClass($"tnt-border-radius-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0) :
        AddClass($"tnt-border-radius-start-start-{Math.Clamp(tntCornerRadius.Value.StartStart, 0, 10)}", tntCornerRadius.Value.StartStart >= 0)
        .AddClass($"tnt-border-radius-start-end-{Math.Clamp(tntCornerRadius.Value.StartEnd, 0, 10)}", tntCornerRadius.Value.StartEnd >= 0)
        .AddClass($"tnt-border-radius-end-start-{Math.Clamp(tntCornerRadius.Value.EndStart, 0, 10)}", tntCornerRadius.Value.EndStart >= 0)
        .AddClass($"tnt-border-radius-end-end-{Math.Clamp(tntCornerRadius.Value.EndEnd, 0, 10)}", tntCornerRadius.Value.EndEnd >= 0)
    : this;

    /// <summary>
    /// Adds a CSS class if the specified condition is met.
    /// </summary>
    /// <param name="className">The CSS class to add.</param>
    /// <param name="when">The condition to check.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddClass(string? className, bool? when = true) {
        if (!string.IsNullOrWhiteSpace(className) && when == true) {
            _classes.Add(className);
        }
        return this;
    }

    /// <summary>
    /// Adds a CSS class for disabled state.
    /// </summary>
    /// <param name="disabled">Whether the component is disabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddDisabled(bool disabled) => AddClass("tnt-disabled", disabled);

    /// <summary>
    /// Adds a CSS class for elevation.
    /// </summary>
    /// <param name="elevation">The elevation level.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddElevation(int elevation) => AddClass($"tnt-elevation-{Math.Clamp(elevation, 0, 10)}", elevation >= 0);

    /// <summary>
    /// Adds a CSS class for filled state.
    /// </summary>
    /// <param name="enabled">Whether the filled state is enabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddFilled(bool enabled = true) => AddClass("tnt-filled", enabled);

    /// <summary>
    /// Adds CSS classes for flexbox layout.
    /// </summary>
    /// <param name="flexBox">The flexbox settings.</param>
    /// <param name="enabled">Whether the flexbox layout is enabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddFlexBox(ITnTFlexBox flexBox, bool enabled = true) => AddFlexBox(flexBox.Direction, flexBox.AlignItems, flexBox.JustifyContent, flexBox.AlignContent, enabled);

    /// <summary>
    /// Adds CSS classes for flexbox layout.
    /// </summary>
    /// <param name="direction">The layout direction.</param>
    /// <param name="alignItems">The alignment of items.</param>
    /// <param name="justifyContent">The justification of content.</param>
    /// <param name="alignContent">The alignment of content.</param>
    /// <param name="enabled">Whether the flexbox layout is enabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
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

    /// <summary>
    /// Adds a CSS class for foreground color.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddForegroundColor(TnTColor? color) => AddClass($"tnt-fg-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    /// <summary>
    /// Adds CSS classes from additional attributes.
    /// </summary>
    /// <param name="additionalAttributes">The additional attributes.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddFromAdditionalAttributes(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("class", out var @class) == true && @class is not null) {
            return AddClass(@class.ToString());
        }
        return this;
    }

    /// <summary>
    /// Adds a CSS class for on-tint color.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddOnTintColor(TnTColor? color) => AddClass($"tnt-on-tint-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    /// <summary>
    /// Adds a CSS class for outlined state.
    /// </summary>
    /// <param name="enabled">Whether the outlined state is enabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddOutlined(bool enabled = true) => enabled ? AddClass("tnt-outlined", enabled) : this;

    /// <summary>
    /// Adds a CSS class for ripple effect.
    /// </summary>
    /// <param name="enabled">Whether the ripple effect is enabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddRipple(bool enabled = true) => AddClass("tnt-ripple", enabled);

    /// <summary>
    /// Adds a CSS class for size.
    /// </summary>
    /// <param name="size">The size to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
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

    /// <summary>
    /// Adds a CSS class for text alignment.
    /// </summary>
    /// <param name="textAlign">The text alignment to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddTextAlign(TextAlign? textAlign) => AddClass("tnt-text-align-" + textAlign.ToCssString(), textAlign != null);

    /// <summary>
    /// Adds a CSS class for tint color.
    /// </summary>
    /// <param name="color">The color to use.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddTintColor(TnTColor? color) => AddClass($"tnt-tint-color-{color?.ToCssClassName() ?? string.Empty}", color is not null && color != TnTColor.None);

    /// <summary>
    /// Adds CSS classes for an interactable component.
    /// </summary>
    /// <param name="interactable">The interactable component.</param>
    /// <param name="enableDisabled">Whether to enable the disabled state.</param>
    /// <param name="enableTint">Whether to enable the tint color.</param>
    /// <param name="enableOnTintColor">Whether to enable the on-tint color.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddTnTInteractable(ITnTInteractable interactable, bool enableDisabled = true, bool enableTint = true, bool enableOnTintColor = true) {
        if (enableDisabled) {
            AddDisabled(interactable.Disabled);
        }
        AddClass("tnt-interactable");
        AddRipple(interactable.EnableRipple);
        AddTintColor(enableTint ? interactable.TintColor : null);
        AddOnTintColor(enableOnTintColor ? interactable.OnTintColor : null);
        return this;
    }

    /// <summary>
    /// Adds CSS classes for a styleable component.
    /// </summary>
    /// <param name="styleable">The styleable component.</param>
    /// <param name="enableBackground">Whether to enable the background color.</param>
    /// <param name="enableForeground">Whether to enable the foreground color.</param>
    /// <param name="enableElevation">Whether to enable the elevation.</param>
    /// <param name="enableBorderRadius">Whether to enable the border radius.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddTnTStyleable(ITnTStyleable styleable, bool enableBackground = true, bool enableForeground = true, bool enableElevation = true, bool enableBorderRadius = true) {
        AddBackgroundColor(enableBackground ? styleable.BackgroundColor : null);
        AddForegroundColor(enableForeground ? styleable.TextColor : null);
        AddTextAlign(styleable.TextAlignment);
        if (enableElevation) {
            AddElevation(styleable.Elevation);
        }
        AddBorderRadius(enableBorderRadius ? styleable.BorderRadius : null);
        return this;
    }

    /// <summary>
    /// Adds a CSS class for underlined state.
    /// </summary>
    /// <param name="enabled">Whether the underlined state is enabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder AddUnderlined(bool enabled = true) => AddClass("tnt-underlined", enabled);

    /// <summary>
    /// Builds the CSS class string.
    /// </summary>
    /// <returns>The constructed CSS class string.</returns>
    public string Build() => string.Join(' ', _classes).Trim();

    /// <summary>
    /// Adds a CSS class for alternative state.
    /// </summary>
    /// <param name="enabled">Whether the alternative state is enabled.</param>
    /// <returns>The current instance of <see cref="CssClassBuilder"/>.</returns>
    public CssClassBuilder SetAlternative(bool enabled = true) => AddClass("tnt-alternative", enabled);

    private CssClassBuilder AddAlignContent(AlignContent? alignContent) => AddClass($"tnt-align-content-{alignContent?.ToCssString()}", alignContent != null);

    private CssClassBuilder AddAlignItems(AlignItems? alignItems) => AddClass($"tnt-align-item-{alignItems?.ToCssString()}", alignItems != null);

    private CssClassBuilder AddJustifyContent(JustifyContent? justifyContent) => AddClass($"tnt-justify-content-{justifyContent?.ToCssString()}", justifyContent != null);

    private CssClassBuilder AddLayoutDirection(LayoutDirection? direction) => AddClass($"tnt-direction-{direction?.ToCssString()}", direction != null);
}
