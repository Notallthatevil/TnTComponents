using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using TnTComponents.Ext;
using TnTComponents.Theming;

namespace TnTComponents;

/// <summary>
///     Represents the theme design component for TnT.
/// </summary>
public class TnTThemeDesign : IComponent {

    /// <summary>
    ///     Gets or sets a value indicating whether color mode toggle is allowed.
    /// </summary>
    [Parameter]
    public bool AllowColorModeToggle { get; set; } = true;

    /// <summary>
    ///     Gets the black color.
    /// </summary>
    public Color Black => Color.Black;

    /// <summary>
    ///     Gets or sets the footer height.
    /// </summary>
    [Parameter]
    public double FooterHeight { get; set; } = 64;

    /// <summary>
    ///     Gets or sets the header height.
    /// </summary>
    [Parameter]
    public double HeaderHeight { get; set; } = 64;

    /// <summary>
    ///     Gets or sets the side navigation width.
    /// </summary>
    [Parameter]
    public double SideNavWidth { get; set; } = 256;

    /// <summary>
    ///     Gets or sets the theme.
    /// </summary>
    [Parameter]
    public Theme Theme { get; set; }

    /// <summary>
    ///     Gets or sets the theme file path.
    /// </summary>
    [Parameter, EditorRequired]
    public string ThemeFile { get; set; } = default!;

    /// <summary>
    ///     Gets the transparent color.
    /// </summary>
    public Color Transparent => Color.Transparent;

    /// <summary>
    ///     Gets the white color.
    /// </summary>
    public Color White => Color.White;

    /// <summary>
    ///     Gets or sets the logger.
    /// </summary>
    [Inject]
    private ILogger<TnTThemeDesign> _logger { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the service provider.
    /// </summary>
    [Inject]
    private IServiceProvider _serviceProvider { get; set; } = default!;

    private RenderHandle _renderHandle;
    private ThemeFile? _themeFile;

    public void Attach(RenderHandle renderHandle) {
        _renderHandle = renderHandle;
    }

    public async Task SetParametersAsync(ParameterView parameters) {
        foreach (var entry in parameters) {
            GetType().GetProperties().FirstOrDefault(p => p.Name == entry.Name)?.SetValue(this, entry.Value);
        }

        var allProperties = GetType().GetProperties().Select(p => new KeyValuePair<string, object?>(p.Name, p.GetValue(this)));

        if (!string.IsNullOrWhiteSpace(ThemeFile)) {
            var themeFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ThemeFile);

            if (File.Exists(themeFile)) {
                await using var fs = File.OpenRead(themeFile);
                _themeFile = await JsonSerializer.DeserializeAsync<ThemeFile>(fs, new JsonSerializerOptions() {
                    PropertyNameCaseInsensitive = true
                });
            }
            else {
                HttpClient httpClient = (HttpClient?)_serviceProvider.GetService(typeof(HttpClient)) ?? new HttpClient();

                if (httpClient is not null) {
                    try {
                        var content = await httpClient.GetStreamAsync(ThemeFile);
                        _themeFile = await JsonSerializer.DeserializeAsync<ThemeFile>(content, new JsonSerializerOptions() {
                            PropertyNameCaseInsensitive = true
                        });
                    }
                    catch { }
                }
            }
        }

        if (_themeFile is null) {
            _themeFile = new();
            _logger.LogWarning("No theme file was found, using default theme.");
        }

        IEnumerable<KeyValuePair<string, object?>> properties = [];

        const string themeColor = "ThemeColor";
        if (_themeFile is not null) {
            if (_themeFile.Schemes is not null) {
                if (_themeFile.Schemes.Light is not null) {
                    properties = properties.Concat(_themeFile.Schemes.Light.GetType().GetProperties().Select(p => new KeyValuePair<string, object?>($"{themeColor}{p.Name}Light", p.GetValue(_themeFile.Schemes.Light))));
                }

                if (_themeFile.Schemes.Dark is not null) {
                    properties = properties.Concat(_themeFile.Schemes.Dark.GetType().GetProperties().Select(p => new KeyValuePair<string, object?>($"{themeColor}{p.Name}Dark", p.GetValue(_themeFile.Schemes.Dark))));
                }
            }
        }

        allProperties = allProperties.Concat(properties)
            .Where(p => p.Value is not null)
            .Where(p => p.Key != nameof(ThemeFile));

        var other = new StringBuilder(":root{");
        var darkTheme = new StringBuilder(":root{");
        var lightTheme = new StringBuilder(":root{");

        foreach (var (name, value) in allProperties) {
            if (name.StartsWith(themeColor, StringComparison.OrdinalIgnoreCase)) {
                var darkModeColor = name.EndsWith("dark", StringComparison.OrdinalIgnoreCase);
                var propName = name.Replace($"{themeColor}", string.Empty);
                propName = propName.Replace("Dark", string.Empty, StringComparison.OrdinalIgnoreCase);
                propName = propName.Replace("Light", string.Empty, StringComparison.OrdinalIgnoreCase);
                propName = propName.SplitPascalCase("-").ToLower();
                var color = ColorTranslator.FromHtml(value as string ?? $"000000");
                if (darkModeColor) {
                    darkTheme.Append("--tnt-color-").Append(propName).Append(':').Append($"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}").Append(';');
                }
                else {
                    lightTheme.Append("--tnt-color-").Append(propName).Append(':').Append($"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}").Append(';');
                }
            }
            else if (value is Color color) {
                var propName = name.SplitPascalCase("-").ToLower();
                darkTheme.Append("--tnt-color-").Append(propName).Append(':').Append($"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}").Append(';');
                lightTheme.Append("--tnt-color-").Append(propName).Append(':').Append($"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}").Append(';');
            }
            else if (value is double d) {
                var propName = name.SplitPascalCase("-").ToLower();
                other.Append("--tnt-").Append(propName).Append(':').Append(d).Append("px;");
            }
            else {
                var propName = name.SplitPascalCase("-").ToLower();
                other.Append("--tnt-").Append(propName).Append(":'").Append(value?.ToString()?.ToLower()).Append("';");
            }
        }

        other.Append('}');
        darkTheme.Append('}');
        lightTheme.Append('}');

        _renderHandle.Render(new RenderFragment(builder => {
            builder.OpenElement(0, "style");
            builder.AddContent(1, other.ToString());
            builder.CloseElement();

            builder.OpenElement(2, "style");
            builder.AddAttribute(4, "id", "tnt-theme-design-dark");
            builder.AddContent(5, darkTheme.ToString());
            builder.CloseElement();

            builder.OpenElement(6, "style");
            builder.AddAttribute(8, "id", "tnt-theme-design-light");
            builder.AddContent(9, lightTheme.ToString());
            builder.CloseElement();
        }));
    }
}