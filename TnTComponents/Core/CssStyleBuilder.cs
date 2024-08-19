using System.Text;

namespace TnTComponents.Core;

internal class CssStyleBuilder {
    private readonly Dictionary<string, string> _styles = [];

    private CssStyleBuilder() {
    }

    public static CssStyleBuilder Create() => new();

    public CssStyleBuilder AddFromAdditionalAttributes(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("style", out var style) == true && style is not null) {
            return AddStyle(style.ToString(), string.Empty);
        }
        return this;
    }

    public CssStyleBuilder AddStyle(string? key, string? value, bool enabled = true) {
        if (!string.IsNullOrWhiteSpace(key) && value is not null && enabled) {
            _styles[key] = value;
        }
        return this;
    }

    public CssStyleBuilder AddVariable(string varName, string varValue, bool enabled = true) {
        if (enabled) {
            return AddStyle($"--{varName}", varValue);
        }
        else {
            return this;
        }
    }

    public CssStyleBuilder AddVariable(string varName, TnTColor color, bool enabled = true) {
        if (enabled) {
            return AddStyle($"--{varName}", $"var(--tnt-color-{color.ToCssClassName()})");
        }
        else {
            return this;
        }
    }

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
                        sb.Append(";");
                    }
                }
            }
            return sb.ToString();
        }
        else {
            return null;
        }
    }
}