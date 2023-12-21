using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Reflection;
using TnTComponents.Common.Ext;

namespace TnTComponents;
public partial class TnTDesignTheme {
    [Parameter]
    public bool AllowColorModeToggle { get; set; } = true;
    [Parameter]
    public bool IsDark { get; set; } = false;

    [Parameter]
    public Color PrimaryLight { get; set; } = ColorTranslator.FromHtml("#7547ad");
    [Parameter]
    public Color OnPrimaryLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color PrimaryContainerLight { get; set; } = ColorTranslator.FromHtml("#Eedbff");
    [Parameter]
    public Color OnPrimaryContainerLight { get; set; } = ColorTranslator.FromHtml("#2a0054");
    [Parameter]
    public Color SecondaryLight { get; set; } = ColorTranslator.FromHtml("#755b00");
    [Parameter]
    public Color OnSecondaryLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color SecondaryContainerLight { get; set; } = ColorTranslator.FromHtml("#Ffdf91");
    [Parameter]
    public Color OnSecondaryContainerLight { get; set; } = ColorTranslator.FromHtml("#241a00");
    [Parameter]
    public Color TertiaryLight { get; set; } = ColorTranslator.FromHtml("#005ac1");
    [Parameter]
    public Color OnTertiaryLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color TertiaryContainerLight { get; set; } = ColorTranslator.FromHtml("#D8e2ff");
    [Parameter]
    public Color OnTertiaryContainerLight { get; set; } = ColorTranslator.FromHtml("#001a41");
    [Parameter]
    public Color ErrorLight { get; set; } = ColorTranslator.FromHtml("#Ba1a1a");
    [Parameter]
    public Color ErrorContainerLight { get; set; } = ColorTranslator.FromHtml("#Ffdad6");
    [Parameter]
    public Color OnErrorLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color OnErrorContainerLight { get; set; } = ColorTranslator.FromHtml("#410002");
    [Parameter]
    public Color BackgroundLight { get; set; } = ColorTranslator.FromHtml("#Fef7ff");
    [Parameter]
    public Color OnBackgroundLight { get; set; } = ColorTranslator.FromHtml("#1d1b20");
    [Parameter]
    public Color SurfaceLight { get; set; } = ColorTranslator.FromHtml("#Fef7ff");
    [Parameter]
    public Color OnSurfaceLight { get; set; } = ColorTranslator.FromHtml("#1d1b20");
    [Parameter]
    public Color SurfaceVariantLight { get; set; } = ColorTranslator.FromHtml("#E8e0eb");
    [Parameter]
    public Color OnSurfaceVariantLight { get; set; } = ColorTranslator.FromHtml("#49454f");
    [Parameter]
    public Color SurfaceContainerHighestLight { get; set; } = ColorTranslator.FromHtml("#E7e1e5");
    [Parameter]
    public Color SurfaceContainerHighLight { get; set; } = ColorTranslator.FromHtml("#Ece6f0");
    [Parameter]
    public Color SurfaceContainerLight { get; set; } = ColorTranslator.FromHtml("#F3edf7");
    [Parameter]
    public Color SurfaceContainerLowLight { get; set; } = ColorTranslator.FromHtml("#F7f2fa");
    [Parameter]
    public Color SurfaceContainerLowestLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color OutlineLight { get; set; } = ColorTranslator.FromHtml("#7b757f");
    [Parameter]
    public Color InverseOnSurfaceLight { get; set; } = ColorTranslator.FromHtml("#F5eff4");
    [Parameter]
    public Color InverseSurfaceLight { get; set; } = ColorTranslator.FromHtml("#322f33");
    [Parameter]
    public Color InversePrimaryLight { get; set; } = ColorTranslator.FromHtml("#D9b9ff");
    [Parameter]
    public Color ShadowLight { get; set; } = ColorTranslator.FromHtml("#565656");
    [Parameter]
    public Color SurfaceTintLight { get; set; } = ColorTranslator.FromHtml("#7547ad");
    [Parameter]
    public Color OutlineVariantLight { get; set; } = ColorTranslator.FromHtml("#Ccc4cf");
    [Parameter]
    public Color ScrimLight { get; set; } = ColorTranslator.FromHtml("#000000");
    [Parameter]
    public Color WarningLight { get; set; } = ColorTranslator.FromHtml("#845400");
    [Parameter]
    public Color OnWarningLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color WarningContainerLight { get; set; } = ColorTranslator.FromHtml("#Ffddb6");
    [Parameter]
    public Color OnWarningContainerLight { get; set; } = ColorTranslator.FromHtml("#2a1800");
    [Parameter]
    public Color SuccessLight { get; set; } = ColorTranslator.FromHtml("#006c4c");
    [Parameter]
    public Color OnSuccessLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color SuccessContainerLight { get; set; } = ColorTranslator.FromHtml("#54febf");
    [Parameter]
    public Color OnSuccessContainerLight { get; set; } = ColorTranslator.FromHtml("#002115");
    [Parameter]
    public Color InfoLight { get; set; } = ColorTranslator.FromHtml("#0d60a8");
    [Parameter]
    public Color OnInfoLight { get; set; } = ColorTranslator.FromHtml("#Ffffff");
    [Parameter]
    public Color InfoContainerLight { get; set; } = ColorTranslator.FromHtml("#D3e3ff");
    [Parameter]
    public Color OnInfoContainerLight { get; set; } = ColorTranslator.FromHtml("#001c39");
    [Parameter]
    public Color PrimaryDark { get; set; } = ColorTranslator.FromHtml("#D9b9ff");
    [Parameter]
    public Color OnPrimaryDark { get; set; } = ColorTranslator.FromHtml("#440e7c");
    [Parameter]
    public Color PrimaryContainerDark { get; set; } = ColorTranslator.FromHtml("#5c2d94");
    [Parameter]
    public Color OnPrimaryContainerDark { get; set; } = ColorTranslator.FromHtml("#Eedbff");
    [Parameter]
    public Color SecondaryDark { get; set; } = ColorTranslator.FromHtml("#F3c000");
    [Parameter]
    public Color OnSecondaryDark { get; set; } = ColorTranslator.FromHtml("#3d2e00");
    [Parameter]
    public Color SecondaryContainerDark { get; set; } = ColorTranslator.FromHtml("#584400");
    [Parameter]
    public Color OnSecondaryContainerDark { get; set; } = ColorTranslator.FromHtml("#Ffdf91");
    [Parameter]
    public Color TertiaryDark { get; set; } = ColorTranslator.FromHtml("#Adc6ff");
    [Parameter]
    public Color OnTertiaryDark { get; set; } = ColorTranslator.FromHtml("#002e69");
    [Parameter]
    public Color TertiaryContainerDark { get; set; } = ColorTranslator.FromHtml("#004494");
    [Parameter]
    public Color OnTertiaryContainerDark { get; set; } = ColorTranslator.FromHtml("#D8e2ff");
    [Parameter]
    public Color ErrorDark { get; set; } = ColorTranslator.FromHtml("#Ffb4ab");
    [Parameter]
    public Color ErrorContainerDark { get; set; } = ColorTranslator.FromHtml("#93000a");
    [Parameter]
    public Color OnErrorDark { get; set; } = ColorTranslator.FromHtml("#690005");
    [Parameter]
    public Color OnErrorContainerDark { get; set; } = ColorTranslator.FromHtml("#Ffdad6");
    [Parameter]
    public Color BackgroundDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");
    [Parameter]
    public Color OnBackgroundDark { get; set; } = ColorTranslator.FromHtml("#E7e1e5");
    [Parameter]
    public Color SurfaceDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");
    [Parameter]
    public Color OnSurfaceDark { get; set; } = ColorTranslator.FromHtml("#E7e1e5");
    [Parameter]
    public Color SurfaceVariantDark { get; set; } = ColorTranslator.FromHtml("#4a454e");
    [Parameter]
    public Color OnSurfaceVariantDark { get; set; } = ColorTranslator.FromHtml("#Ccc4cf");
    [Parameter]
    public Color SurfaceContainerHighestDark { get; set; } = ColorTranslator.FromHtml("#36343b");
    [Parameter]
    public Color SurfaceContainerHighDark { get; set; } = ColorTranslator.FromHtml("#2b2930");
    [Parameter]
    public Color SurfaceContainerDark { get; set; } = ColorTranslator.FromHtml("#211f26");
    [Parameter]
    public Color SurfaceContainerLowDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");
    [Parameter]
    public Color SurfaceContainerLowestDark { get; set; } = ColorTranslator.FromHtml("#0f0d13");
    [Parameter]
    public Color OutlineDark { get; set; } = ColorTranslator.FromHtml("#958e99");
    [Parameter]
    public Color InverseOnSurfaceDark { get; set; } = ColorTranslator.FromHtml("#1d1b1e");
    [Parameter]
    public Color InverseSurfaceDark { get; set; } = ColorTranslator.FromHtml("#E7e1e5");
    [Parameter]
    public Color InversePrimaryDark { get; set; } = ColorTranslator.FromHtml("#7547ad");
    [Parameter]
    public Color ShadowDark { get; set; } = ColorTranslator.FromHtml("#Ababab");
    [Parameter]
    public Color SurfaceTintDark { get; set; } = ColorTranslator.FromHtml("#D9b9ff");
    [Parameter]
    public Color OutlineVariantDark { get; set; } = ColorTranslator.FromHtml("#4a454e");
    [Parameter]
    public Color ScrimDark { get; set; } = ColorTranslator.FromHtml("#000000");
    [Parameter]
    public Color WarningDark { get; set; } = ColorTranslator.FromHtml("#Ffb95a");
    [Parameter]
    public Color OnWarningDark { get; set; } = ColorTranslator.FromHtml("#462a00");
    [Parameter]
    public Color WarningContainerDark { get; set; } = ColorTranslator.FromHtml("#643f00");
    [Parameter]
    public Color OnWarningContainerDark { get; set; } = ColorTranslator.FromHtml("#Ffddb6");
    [Parameter]
    public Color SuccessDark { get; set; } = ColorTranslator.FromHtml("#28e0a4");
    [Parameter]
    public Color OnSuccessDark { get; set; } = ColorTranslator.FromHtml("#003826");
    [Parameter]
    public Color SuccessContainerDark { get; set; } = ColorTranslator.FromHtml("#005139");
    [Parameter]
    public Color OnSuccessContainerDark { get; set; } = ColorTranslator.FromHtml("#54febf");
    [Parameter]
    public Color InfoDark { get; set; } = ColorTranslator.FromHtml("#A3c9ff");
    [Parameter]
    public Color OnInfoDark { get; set; } = ColorTranslator.FromHtml("#00315c");
    [Parameter]
    public Color InfoContainerDark { get; set; } = ColorTranslator.FromHtml("#004883");
    [Parameter]
    public Color OnInfoContainerDark { get; set; } = ColorTranslator.FromHtml("#D3e3ff");


    private IReadOnlyDictionary<string, object> GetAttributes() {
        var dict = new Dictionary<string, object>();

        foreach (var prop in GetType().GetProperties()) {
            if (prop.GetCustomAttribute<ParameterAttribute>() is not null) {
                var propName = prop.Name.SplitPascalCase("-").ToLower();
                var value = prop.GetValue(this);

                if (value is Color color) {
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
