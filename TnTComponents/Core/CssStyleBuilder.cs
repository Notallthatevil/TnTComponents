using System.Text;

namespace TnTComponents.Core;

/// <summary>
///     A builder class for constructing CSS style strings.
/// </summary>
internal sealed class CssStyleBuilder {
    private readonly Dictionary<string, string> _styles = [];
    private string? _styleString = string.Empty;
    private CssStyleBuilder() {
    }

    /// <summary>
    ///     Creates a new instance of <see cref="CssStyleBuilder" />.
    /// </summary>
    /// <returns>A new instance of <see cref="CssStyleBuilder" />.</returns>
    public static CssStyleBuilder Create() => new();

    /// <summary>
    ///     Adds styles from additional attributes if they contain a "style" key.
    /// </summary>
    /// <param name="additionalAttributes">A dictionary of additional attributes.</param>
    /// <returns>The current instance of <see cref="CssStyleBuilder" />.</returns>
    public CssStyleBuilder AddFromAdditionalAttributes(IReadOnlyDictionary<string, object>? additionalAttributes) {
        return additionalAttributes?.TryGetValue("style", out var style) == true && style is not null
            ? AddStyle(style.ToString(), string.Empty)
            : this;
    }

    /// <summary>
    /// Adds a CSS style to the builder.
    /// </summary>
    /// <param name="style">The style string to add</param>
    /// <returns>The current instance of <see cref="CssStyleBuilder" />.</returns>
    public CssStyleBuilder Add(string style) {
        _styleString += style ?? string.Empty;
        return this;
    }

    /// <summary>
    ///     Adds a CSS style to the builder.
    /// </summary>
    /// <param name="key">    The CSS property name.</param>
    /// <param name="value">  The CSS property value.</param>
    /// <param name="enabled">A flag indicating whether the style should be added.</param>
    /// <returns>The current instance of <see cref="CssStyleBuilder" />.</returns>
    public CssStyleBuilder AddStyle(string? key, string? value, bool enabled = true) {
        if (!string.IsNullOrWhiteSpace(key) && value is not null && enabled) {
            _styles[key] = value;
        }
        return this;
    }

    /// <summary>
    ///     Adds a CSS variable to the builder.
    /// </summary>
    /// <param name="varName"> The name of the CSS variable.</param>
    /// <param name="varValue">The value of the CSS variable.</param>
    /// <param name="enabled"> A flag indicating whether the variable should be added.</param>
    /// <returns>The current instance of <see cref="CssStyleBuilder" />.</returns>
    public CssStyleBuilder AddVariable(string varName, string varValue, bool enabled = true) => enabled ? AddStyle($"--{varName}", varValue) : this;

    /// <summary>
    ///     Adds a CSS variable to the builder with a color value.
    /// </summary>
    /// <param name="varName">The name of the CSS variable.</param>
    /// <param name="color">  The color value for the CSS variable.</param>
    /// <param name="enabled">A flag indicating whether the variable should be added.</param>
    /// <returns>The current instance of <see cref="CssStyleBuilder" />.</returns>
    public CssStyleBuilder AddVariable(string varName, TnTColor color, bool enabled = true) => enabled ? AddStyle($"--{varName}", $"var(--tnt-color-{color.ToCssClassName()})") : this;

    public CssStyleBuilder AddBackgroundColor(TnTColor? color, bool enabled = true) => enabled && color.HasValue ? AddStyle("background-color", color?.ToCssTnTColorVariable()) : this;

    public CssStyleBuilder AddForegroundColor(TnTColor? color, bool enabled = true) => enabled && color.HasValue ? AddStyle("color", color?.ToCssTnTColorVariable()) : this;

    /// <summary>
    ///     Builds the CSS style string.
    /// </summary>
    /// <returns>A CSS style string or null if no styles were added.</returns>
    public string? Build() {
        var styles = _styles.Where(kv => !string.IsNullOrWhiteSpace(kv.Key));
        if (styles.Any()) {
            var sb = new StringBuilder();
            foreach (var (key, value) in styles) {
                var trimmedKey = key.Trim();
                var trimmedValue = value.Trim();
                sb.Append(trimmedKey);
                if (!string.IsNullOrWhiteSpace(trimmedValue)) {
                    if (!trimmedKey.EndsWith(':')) {
                        sb.Append(':');
                    }
                    sb.Append(trimmedValue);

                    if (!trimmedValue.EndsWith(';')) {
                        sb.Append(';');
                    }
                }
                else {
                    if (!trimmedKey.EndsWith(';')) {
                        sb.Append(';');
                    }
                }
            }
            sb.Append(_styleString);
            return sb.ToString();
        }
        else {
            return null;
        }
    }
}