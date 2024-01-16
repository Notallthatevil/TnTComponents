using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Reflection;
using TnTComponents.Common.Ext;

namespace TnTComponents;

public partial class TnTDesignTheme {

    [Parameter]
    public bool AllowColorModeToggle { get; set; } = true;

    [Parameter]
    public bool IsDark { get; set; } = false;

    public Color Transparent => Color.Transparent;

    public Color White => Color.White;

    public Color Black => Color.Black;

    [Parameter]
    public System.Drawing.Color PrimaryLight { get; set; } = ColorTranslator.FromHtml("#7547ad");

    [Parameter]
    public System.Drawing.Color OnPrimaryLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color PrimaryContainerLight { get; set; } = ColorTranslator.FromHtml("#Eedbff");

    [Parameter]
    public System.Drawing.Color OnPrimaryContainerLight { get; set; } = ColorTranslator.FromHtml("#2a0054");

    [Parameter]
    public System.Drawing.Color SecondaryLight { get; set; } = ColorTranslator.FromHtml("#755b00");

    [Parameter]
    public System.Drawing.Color OnSecondaryLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color SecondaryContainerLight { get; set; } = ColorTranslator.FromHtml("#Ffdf91");

    [Parameter]
    public System.Drawing.Color OnSecondaryContainerLight { get; set; } = ColorTranslator.FromHtml("#241a00");

    [Parameter]
    public System.Drawing.Color TertiaryLight { get; set; } = ColorTranslator.FromHtml("#005ac1");

    [Parameter]
    public System.Drawing.Color OnTertiaryLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color TertiaryContainerLight { get; set; } = ColorTranslator.FromHtml("#D8e2ff");

    [Parameter]
    public System.Drawing.Color OnTertiaryContainerLight { get; set; } = ColorTranslator.FromHtml("#001a41");

    [Parameter]
    public System.Drawing.Color ErrorLight { get; set; } = ColorTranslator.FromHtml("#Ba1a1a");

    [Parameter]
    public System.Drawing.Color ErrorContainerLight { get; set; } = ColorTranslator.FromHtml("#Ffdad6");

    [Parameter]
    public System.Drawing.Color OnErrorLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color OnErrorContainerLight { get; set; } = ColorTranslator.FromHtml("#410002");

    [Parameter]
    public System.Drawing.Color BackgroundLight { get; set; } = ColorTranslator.FromHtml("#Fef7ff");

    [Parameter]
    public System.Drawing.Color OnBackgroundLight { get; set; } = ColorTranslator.FromHtml("#1d1b20");

    [Parameter]
    public System.Drawing.Color SurfaceLight { get; set; } = ColorTranslator.FromHtml("#Fef7ff");

    [Parameter]
    public System.Drawing.Color OnSurfaceLight { get; set; } = ColorTranslator.FromHtml("#1d1b20");

    [Parameter]
    public System.Drawing.Color SurfaceVariantLight { get; set; } = ColorTranslator.FromHtml("#E8e0eb");

    [Parameter]
    public System.Drawing.Color OnSurfaceVariantLight { get; set; } = ColorTranslator.FromHtml("#49454f");

    [Parameter]
    public System.Drawing.Color SurfaceContainerHighestLight { get; set; } = ColorTranslator.FromHtml("#E7e1e5");

    [Parameter]
    public System.Drawing.Color SurfaceContainerHighLight { get; set; } = ColorTranslator.FromHtml("#Ece6f0");

    [Parameter]
    public System.Drawing.Color SurfaceContainerLight { get; set; } = ColorTranslator.FromHtml("#F3edf7");

    [Parameter]
    public System.Drawing.Color SurfaceContainerLowLight { get; set; } = ColorTranslator.FromHtml("#F7f2fa");

    [Parameter]
    public System.Drawing.Color SurfaceContainerLowestLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color OutlineLight { get; set; } = ColorTranslator.FromHtml("#7b757f");

    [Parameter]
    public System.Drawing.Color InverseOnSurfaceLight { get; set; } = ColorTranslator.FromHtml("#F5eff4");

    [Parameter]
    public System.Drawing.Color InverseSurfaceLight { get; set; } = ColorTranslator.FromHtml("#322f33");

    [Parameter]
    public System.Drawing.Color InversePrimaryLight { get; set; } = ColorTranslator.FromHtml("#D9b9ff");

    [Parameter]
    public System.Drawing.Color ShadowLight { get; set; } = ColorTranslator.FromHtml("#565656");

    [Parameter]
    public System.Drawing.Color SurfaceTintLight { get; set; } = ColorTranslator.FromHtml("#7547ad");

    [Parameter]
    public System.Drawing.Color OutlineVariantLight { get; set; } = ColorTranslator.FromHtml("#Ccc4cf");

    [Parameter]
    public System.Drawing.Color ScrimLight { get; set; } = ColorTranslator.FromHtml("#000000");

    [Parameter]
    public System.Drawing.Color WarningLight { get; set; } = ColorTranslator.FromHtml("#845400");

    [Parameter]
    public System.Drawing.Color OnWarningLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color WarningContainerLight { get; set; } = ColorTranslator.FromHtml("#Ffddb6");

    [Parameter]
    public System.Drawing.Color OnWarningContainerLight { get; set; } = ColorTranslator.FromHtml("#2a1800");

    [Parameter]
    public System.Drawing.Color SuccessLight { get; set; } = ColorTranslator.FromHtml("#006c4c");

    [Parameter]
    public System.Drawing.Color OnSuccessLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color SuccessContainerLight { get; set; } = ColorTranslator.FromHtml("#54febf");

    [Parameter]
    public System.Drawing.Color OnSuccessContainerLight { get; set; } = ColorTranslator.FromHtml("#002115");

    [Parameter]
    public System.Drawing.Color InfoLight { get; set; } = ColorTranslator.FromHtml("#0d60a8");

    [Parameter]
    public System.Drawing.Color OnInfoLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");

    [Parameter]
    public System.Drawing.Color InfoContainerLight { get; set; } = ColorTranslator.FromHtml("#D3e3ff");

    [Parameter]
    public System.Drawing.Color OnInfoContainerLight { get; set; } = ColorTranslator.FromHtml("#001c39");

    [Parameter]
    public System.Drawing.Color PrimaryDark { get; set; } = ColorTranslator.FromHtml("#D9b9ff");

    [Parameter]
    public System.Drawing.Color OnPrimaryDark { get; set; } = ColorTranslator.FromHtml("#440e7c");

    [Parameter]
    public System.Drawing.Color PrimaryContainerDark { get; set; } = ColorTranslator.FromHtml("#5c2d94");

    [Parameter]
    public System.Drawing.Color OnPrimaryContainerDark { get; set; } = ColorTranslator.FromHtml("#Eedbff");

    [Parameter]
    public System.Drawing.Color SecondaryDark { get; set; } = ColorTranslator.FromHtml("#F3c000");

    [Parameter]
    public System.Drawing.Color OnSecondaryDark { get; set; } = ColorTranslator.FromHtml("#3d2e00");

    [Parameter]
    public System.Drawing.Color SecondaryContainerDark { get; set; } = ColorTranslator.FromHtml("#584400");

    [Parameter]
    public System.Drawing.Color OnSecondaryContainerDark { get; set; } = ColorTranslator.FromHtml("#Ffdf91");

    [Parameter]
    public System.Drawing.Color TertiaryDark { get; set; } = ColorTranslator.FromHtml("#Adc6ff");

    [Parameter]
    public System.Drawing.Color OnTertiaryDark { get; set; } = ColorTranslator.FromHtml("#002e69");

    [Parameter]
    public System.Drawing.Color TertiaryContainerDark { get; set; } = ColorTranslator.FromHtml("#004494");

    [Parameter]
    public System.Drawing.Color OnTertiaryContainerDark { get; set; } = ColorTranslator.FromHtml("#D8e2ff");

    [Parameter]
    public System.Drawing.Color ErrorDark { get; set; } = ColorTranslator.FromHtml("#Ffb4ab");

    [Parameter]
    public System.Drawing.Color ErrorContainerDark { get; set; } = ColorTranslator.FromHtml("#93000a");

    [Parameter]
    public System.Drawing.Color OnErrorDark { get; set; } = ColorTranslator.FromHtml("#690005");

    [Parameter]
    public System.Drawing.Color OnErrorContainerDark { get; set; } = ColorTranslator.FromHtml("#Ffdad6");

    [Parameter]
    public System.Drawing.Color BackgroundDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");

    [Parameter]
    public System.Drawing.Color OnBackgroundDark { get; set; } = ColorTranslator.FromHtml("#E7e1e5");

    [Parameter]
    public System.Drawing.Color SurfaceDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");

    [Parameter]
    public System.Drawing.Color OnSurfaceDark { get; set; } = ColorTranslator.FromHtml("#E7e1e5");

    [Parameter]
    public System.Drawing.Color SurfaceVariantDark { get; set; } = ColorTranslator.FromHtml("#4a454e");

    [Parameter]
    public System.Drawing.Color OnSurfaceVariantDark { get; set; } = ColorTranslator.FromHtml("#Ccc4cf");

    [Parameter]
    public System.Drawing.Color SurfaceContainerHighestDark { get; set; } = ColorTranslator.FromHtml("#36343b");

    [Parameter]
    public System.Drawing.Color SurfaceContainerHighDark { get; set; } = ColorTranslator.FromHtml("#2b2930");

    [Parameter]
    public System.Drawing.Color SurfaceContainerDark { get; set; } = ColorTranslator.FromHtml("#211f26");

    [Parameter]
    public System.Drawing.Color SurfaceContainerLowDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");

    [Parameter]
    public System.Drawing.Color SurfaceContainerLowestDark { get; set; } = ColorTranslator.FromHtml("#0f0d13");

    [Parameter]
    public System.Drawing.Color OutlineDark { get; set; } = ColorTranslator.FromHtml("#958e99");

    [Parameter]
    public System.Drawing.Color InverseOnSurfaceDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");

    [Parameter]
    public System.Drawing.Color InverseSurfaceDark { get; set; } = ColorTranslator.FromHtml("#E7e1e5");

    [Parameter]
    public System.Drawing.Color InversePrimaryDark { get; set; } = ColorTranslator.FromHtml("#7547ad");

    [Parameter]
    public System.Drawing.Color ShadowDark { get; set; } = ColorTranslator.FromHtml("#Ababab");

    [Parameter]
    public System.Drawing.Color SurfaceTintDark { get; set; } = ColorTranslator.FromHtml("#D9b9ff");

    [Parameter]
    public System.Drawing.Color OutlineVariantDark { get; set; } = ColorTranslator.FromHtml("#4a454e");

    [Parameter]
    public System.Drawing.Color ScrimDark { get; set; } = ColorTranslator.FromHtml("#000000");

    [Parameter]
    public System.Drawing.Color WarningDark { get; set; } = ColorTranslator.FromHtml("#Ffb95a");

    [Parameter]
    public System.Drawing.Color OnWarningDark { get; set; } = ColorTranslator.FromHtml("#462a00");

    [Parameter]
    public System.Drawing.Color WarningContainerDark { get; set; } = ColorTranslator.FromHtml("#643f00");

    [Parameter]
    public System.Drawing.Color OnWarningContainerDark { get; set; } = ColorTranslator.FromHtml("#Ffddb6");

    [Parameter]
    public System.Drawing.Color SuccessDark { get; set; } = ColorTranslator.FromHtml("#28e0a4");

    [Parameter]
    public System.Drawing.Color OnSuccessDark { get; set; } = ColorTranslator.FromHtml("#003826");

    [Parameter]
    public System.Drawing.Color SuccessContainerDark { get; set; } = ColorTranslator.FromHtml("#005139");

    [Parameter]
    public System.Drawing.Color OnSuccessContainerDark { get; set; } = ColorTranslator.FromHtml("#54febf");

    [Parameter]
    public System.Drawing.Color InfoDark { get; set; } = ColorTranslator.FromHtml("#A3c9ff");

    [Parameter]
    public System.Drawing.Color OnInfoDark { get; set; } = ColorTranslator.FromHtml("#00315c");

    [Parameter]
    public System.Drawing.Color InfoContainerDark { get; set; } = ColorTranslator.FromHtml("#004883");

    [Parameter]
    public System.Drawing.Color OnInfoContainerDark { get; set; } = ColorTranslator.FromHtml("#D3e3ff");

    private IReadOnlyDictionary<string, object> GetAttributes() {
        var dict = new Dictionary<string, object>();

        foreach (var prop in GetType().GetProperties()) {
            if (prop.GetCustomAttribute<ParameterAttribute>() is not null) {
                var propName = prop.Name.SplitPascalCase("-").ToLower();
                var value = prop.GetValue(this);

                if (value is System.Drawing.Color color) {
                    dict.Add(propName, $"#{color.R:X2}{color.G:X2}{color.B:X2}".ToLower());
                }
                else {
                    dict.Add(propName, value!);
                }
            }
        }
        return dict;
    }
}