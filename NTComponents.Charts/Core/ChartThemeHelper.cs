using Microsoft.JSInterop;
using SkiaSharp;

namespace NTComponents.Charts.Core;

/// <summary>
/// A helper class for resolving CSS variables to SkiaSharp colors and managing the chart palette.
/// </summary>
public class ChartThemeHelper {
   private readonly IJSRuntime _jsRuntime;
   private readonly Dictionary<string, SKColor> _colorCache = [];

   private readonly string[] _defaultPaletteVariables = [
       "--tnt-color-primary",
        "--tnt-color-secondary",
        "--tnt-color-tertiary",
        "--tnt-color-error",
        "--tnt-color-success",
        "--tnt-color-warning",
        "--tnt-color-info",
        "--tnt-color-primary-container",
        "--tnt-color-secondary-container",
        "--tnt-color-tertiary-container"
   ];

   public ChartThemeHelper(IJSRuntime jsRuntime) {
      _jsRuntime = jsRuntime;
   }

   /// <summary>
   /// Gets the color associated with a CSS variable.
   /// </summary>
   /// <param name="variableName">The name of the CSS variable (e.g., "--tnt-color-primary").</param>
   /// <returns>The resolved <see cref="SKColor"/>.</returns>
   public async Task<SKColor> GetColorAsync(string variableName) {
      if (_colorCache.TryGetValue(variableName, out var cachedColor)) {
         return cachedColor;
      }

      var colorString = await _jsRuntime.InvokeAsync<string>("NTCharts.getCssVariable", variableName);
      var color = ParseCssColor(colorString);
      _colorCache[variableName] = color;
      return color;
   }

   /// <summary>
   /// Gets a color from the default palette by index.
   /// </summary>
   /// <param name="index">The index in the palette.</param>
   /// <returns>The resolved <see cref="SKColor"/>.</returns>
   public Task<SKColor> GetPaletteColorAsync(int index) {
      var variableName = _defaultPaletteVariables[index % _defaultPaletteVariables.Length];
      return GetColorAsync(variableName);
   }

   /// <summary>
   /// Clears the color cache. Use this if the theme changes.
   /// </summary>
   public void ClearCache() {
      _colorCache.Clear();
   }

   private static SKColor ParseCssColor(string cssColor) {
      if (string.IsNullOrWhiteSpace(cssColor)) return SKColors.Transparent;

      cssColor = cssColor.Trim();

      if (SKColor.TryParse(cssColor, out var skColor)) {
         return skColor;
      }

      // Handle rgb(r, g, b)
      if (cssColor.StartsWith("rgb", StringComparison.OrdinalIgnoreCase)) {
         try {
            var start = cssColor.IndexOf('(') + 1;
            var end = cssColor.IndexOf(')');
            var parts = cssColor[start..end].Split(',');

            byte r = byte.Parse(parts[0].Trim());
            byte g = byte.Parse(parts[1].Trim());
            byte b = byte.Parse(parts[2].Trim());
            byte a = 255;

            if (parts.Length > 3) {
               a = (byte)(float.Parse(parts[3].Trim()) * 255);
            }

            return new SKColor(r, g, b, a);
         }
         catch {
            return SKColors.Transparent;
         }
      }

      return SKColors.Transparent;
   }
}
