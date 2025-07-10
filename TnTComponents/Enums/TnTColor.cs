using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TnTComponents;

/// <summary>
///     Represents the various color options available in the TnTComponents.
/// </summary>
public enum TnTColor {

    /// <summary>
    ///     No color specified, represents the default color state.
    /// </summary>
    None,

    /// <summary>
    ///     Transparent color, allows underlying elements to show through.
    /// </summary>
    Transparent,

    /// <summary>
    ///     Black color, represents the darkest color in the palette.
    /// </summary>
    Black,

    /// <summary>
    ///     White color, represents the lightest color in the palette.
    /// </summary>
    White,

    /// <summary>
    ///     Primary color, used for key UI elements and branding.
    /// </summary>
    Primary,

    /// <summary>
    ///     Surface tint color, used to tint surfaces to indicate elevation.
    /// </summary>
    SurfaceTint,

    /// <summary>
    ///     Color used for content that appears on the primary color.
    /// </summary>
    OnPrimary,

    /// <summary>
    ///     Container color for the primary color, used for less prominent components.
    /// </summary>
    PrimaryContainer,

    /// <summary>
    ///     Color used for content that appears on primary containers.
    /// </summary>
    OnPrimaryContainer,

    /// <summary>
    ///     Secondary color, used for less prominent UI elements.
    /// </summary>
    Secondary,

    /// <summary>
    ///     Color used for content that appears on the secondary color.
    /// </summary>
    OnSecondary,

    /// <summary>
    ///     Container color for the secondary color, used for less prominent components.
    /// </summary>
    SecondaryContainer,

    /// <summary>
    ///     Color used for content that appears on secondary containers.
    /// </summary>
    OnSecondaryContainer,

    /// <summary>
    ///     Tertiary color, used for balancing primary and secondary colors or for accent elements.
    /// </summary>
    Tertiary,

    /// <summary>
    ///     Color used for content that appears on the tertiary color.
    /// </summary>
    OnTertiary,

    /// <summary>
    ///     Container color for the tertiary color, used for less prominent components.
    /// </summary>
    TertiaryContainer,

    /// <summary>
    ///     Color used for content that appears on tertiary containers.
    /// </summary>
    OnTertiaryContainer,

    /// <summary>
    ///     Error color, used for error states and messages.
    /// </summary>
    Error,

    /// <summary>
    ///     Color used for content that appears on the error color.
    /// </summary>
    OnError,

    /// <summary>
    ///     Container color for the error color, used for less prominent error indicators.
    /// </summary>
    ErrorContainer,

    /// <summary>
    ///     Color used for content that appears on error containers.
    /// </summary>
    OnErrorContainer,

    /// <summary>
    ///     Background color, used for the main background of the application.
    /// </summary>
    Background,

    /// <summary>
    ///     Color used for content that appears on the background.
    /// </summary>
    OnBackground,

    /// <summary>
    ///     Surface color, used for cards, sheets, and other surface elements.
    /// </summary>
    Surface,

    /// <summary>
    ///     Color used for content that appears on surfaces.
    /// </summary>
    OnSurface,

    /// <summary>
    ///     Surface variant color, used as an alternative to the main surface color.
    /// </summary>
    SurfaceVariant,

    /// <summary>
    ///     Color used for content that appears on surface variants.
    /// </summary>
    OnSurfaceVariant,

    /// <summary>
    ///     Outline color, used for decorative or dividing elements and borders.
    /// </summary>
    Outline,

    /// <summary>
    ///     Outline variant color, used as an alternative to the main outline color for decorative elements.
    /// </summary>
    OutlineVariant,

    /// <summary>
    ///     Shadow color, used for shadows to indicate elevation.
    /// </summary>
    Shadow,

    /// <summary>
    ///     Scrim color, used for dimming backgrounds for modals and dialogs.
    /// </summary>
    Scrim,

    /// <summary>
    ///     Inverse surface color, used for surfaces that contrast with the main surfaces.
    /// </summary>
    InverseSurface,

    /// <summary>
    ///     Color used for content that appears on inverse surfaces.
    /// </summary>
    InverseOnSurface,

    /// <summary>
    ///     Inverse primary color, used for primary elements on inverse surfaces.
    /// </summary>
    InversePrimary,

    /// <summary>
    ///     Fixed primary color that doesn't change with theme mode.
    /// </summary>
    PrimaryFixed,

    /// <summary>
    ///     Color used for content that appears on fixed primary color.
    /// </summary>
    OnPrimaryFixed,

    /// <summary>
    ///     Dimmer variant of the fixed primary color.
    /// </summary>
    PrimaryFixedDim,

    /// <summary>
    ///     Variant color for content that appears on fixed primary colors.
    /// </summary>
    OnPrimaryFixedVariant,

    /// <summary>
    ///     Fixed secondary color that doesn't change with theme mode.
    /// </summary>
    SecondaryFixed,

    /// <summary>
    ///     Color used for content that appears on fixed secondary color.
    /// </summary>
    OnSecondaryFixed,

    /// <summary>
    ///     Dimmer variant of the fixed secondary color.
    /// </summary>
    SecondaryFixedDim,

    /// <summary>
    ///     Variant color for content that appears on fixed secondary colors.
    /// </summary>
    OnSecondaryFixedVariant,

    /// <summary>
    ///     Fixed tertiary color that doesn't change with theme mode.
    /// </summary>
    TertiaryFixed,

    /// <summary>
    ///     Color used for content that appears on fixed tertiary color.
    /// </summary>
    OnTertiaryFixed,

    /// <summary>
    ///     Dimmer variant of the fixed tertiary color.
    /// </summary>
    TertiaryFixedDim,

    /// <summary>
    ///     Variant color for content that appears on fixed tertiary colors.
    /// </summary>
    OnTertiaryFixedVariant,

    /// <summary>
    ///     Dimmed surface color, used for lower emphasis surfaces.
    /// </summary>
    SurfaceDim,

    /// <summary>
    ///     Bright surface color, used for higher emphasis surfaces.
    /// </summary>
    SurfaceBright,

    /// <summary>
    ///     Lowest container surface color in the elevation hierarchy.
    /// </summary>
    SurfaceContainerLowest,

    /// <summary>
    ///     Low container surface color in the elevation hierarchy.
    /// </summary>
    SurfaceContainerLow,

    /// <summary>
    ///     Standard container surface color.
    /// </summary>
    SurfaceContainer,

    /// <summary>
    ///     High container surface color in the elevation hierarchy.
    /// </summary>
    SurfaceContainerHigh,

    /// <summary>
    ///     Highest container surface color in the elevation hierarchy.
    /// </summary>
    SurfaceContainerHighest,

    /// <summary>
    ///     Success color, used for successful states and confirmations.
    /// </summary>
    Success,

    /// <summary>
    ///     Color used for content that appears on the success color.
    /// </summary>
    OnSuccess,

    /// <summary>
    ///     Container color for the success color, used for less prominent success indicators.
    /// </summary>
    SuccessContainer,

    /// <summary>
    ///     Color used for content that appears on success containers.
    /// </summary>
    OnSuccessContainer,

    /// <summary>
    ///     Warning color, used for warnings and cautionary messages.
    /// </summary>
    Warning,

    /// <summary>
    ///     Color used for content that appears on the warning color.
    /// </summary>
    OnWarning,

    /// <summary>
    ///     Container color for the warning color, used for less prominent warning indicators.
    /// </summary>
    WarningContainer,

    /// <summary>
    ///     Color used for content that appears on warning containers.
    /// </summary>
    OnWarningContainer,

    /// <summary>
    ///     Information color, used for informational messages and elements.
    /// </summary>
    Info,

    /// <summary>
    ///     Color used for content that appears on the information color.
    /// </summary>
    OnInfo,

    /// <summary>
    ///     Container color for the information color, used for less prominent information indicators.
    /// </summary>
    InfoContainer,

    /// <summary>
    ///     Color used for content that appears on information containers.
    /// </summary>
    OnInfoContainer
}

/// <summary>
///     Provides extension methods for the <see cref="TnTColor" /> enum.
/// </summary>
public static partial class TnTColorEnumExt {

    /// <summary>
    ///     Converts a nullable <see cref="TnTColor" /> enum value to its corresponding CSS class name.
    /// </summary>
    /// <param name="tnTColorEnum">The nullable <see cref="TnTColor" /> enum value.</param>
    /// <returns>The CSS class name as a string, or an empty string if the value is null.</returns>
    public static string ToCssClassName(this TnTColor? tnTColorEnum) => tnTColorEnum.HasValue ? tnTColorEnum.Value.ToCssClassName() : string.Empty;

    /// <summary>
    ///     Converts a <see cref="TnTColor" /> enum value to its corresponding CSS class name.
    /// </summary>
    /// <param name="tnTColorEnum">The <see cref="TnTColor" /> enum value.</param>
    /// <returns>The CSS class name as a string.</returns>
    public static string ToCssClassName(this TnTColor tnTColorEnum) => FindAllCapitalsExceptFirstLetter().Replace(tnTColorEnum.ToString(), "-$1").ToLower();

    /// <summary>
    ///     Converts a <see cref="TnTColor" /> enum value to its corresponding CSS variable name.
    /// </summary>
    /// <param name="tnTColorEnum">The <see cref="TnTColor" /> enum value.</param>
    /// <returns>The css variable</returns>
    public static string ToCssTnTColorVariable(this TnTColor? tnTColorEnum) {
        return tnTColorEnum.HasValue
            ? $"var(--tnt-color-{tnTColorEnum.Value.ToCssClassName()})"
            : string.Empty;
    }

    /// <summary>
    ///     Converts a <see cref="TnTColor" /> enum value to its corresponding CSS variable name.
    /// </summary>
    /// <param name="tnTColorEnum">The <see cref="TnTColor" /> enum value.</param>
    /// <returns>The css variable</returns>
    public static string ToCssTnTColorVariable(this TnTColor tnTColorEnum) => $"var(--tnt-color-{tnTColorEnum.ToCssClassName()})";

    /// <summary>
    ///     Finds all capital letters in a string except the first letter.
    /// </summary>
    /// <returns>A <see cref="Regex" /> object that matches all capital letters except the first letter.</returns>
    [GeneratedRegex(@"(?<=.)([A-Z])")]
    private static partial Regex FindAllCapitalsExceptFirstLetter();
}