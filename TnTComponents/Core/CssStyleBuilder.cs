using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
internal class CssStyleBuilder {
    private readonly Dictionary<string, string> _styles = [];

    private CssStyleBuilder() {
    }

    public static CssStyleBuilder Create() => new();

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

    public CssStyleBuilder AddFromAdditionalAttributes(IReadOnlyDictionary<string, object>? additionalAttributes) {
        if (additionalAttributes?.TryGetValue("style", out var style) == true && style is not null) {
            return AddStyle(style.ToString(), string.Empty);
        }
        return this;
    }

    public string? Build() {
        var styles = _styles.Where(kv => !string.IsNullOrWhiteSpace(kv.Key));
        if (styles.Any()) {
            var sb = new StringBuilder();
            foreach (var (key, value) in styles) {
                sb.Append(key);
                if (!key.Trim().EndsWith(';')) {
                    sb.Append(": ").Append(value).Append("; ");
                }
            }
            return sb.ToString().Trim();
        }
        else {
            return null;
        }
    }
}

