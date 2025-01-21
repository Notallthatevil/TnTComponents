using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TnTComponents;

/// <summary>
/// Represents the various color options available in the TnTComponents.
/// </summary>
public enum TnTColor {
    None,
    Transparent,
    Black,
    White,
    Primary,
    SurfaceTint,
    OnPrimary,
    PrimaryContainer,
    OnPrimaryContainer,
    Secondary,
    OnSecondary,
    SecondaryContainer,
    OnSecondaryContainer,
    Tertiary,
    OnTertiary,
    TertiaryContainer,
    OnTertiaryContainer,
    Error,
    OnError,
    ErrorContainer,
    OnErrorContainer,
    Background,
    OnBackground,
    Surface,
    OnSurface,
    SurfaceVariant,
    OnSurfaceVariant,
    Outline,
    OutlineVariant,
    Shadow,
    Scrim,
    InverseSurface,
    InverseOnSurface,
    InversePrimary,
    PrimaryFixed,
    OnPrimaryFixed,
    PrimaryFixedDim,
    OnPrimaryFixedVariant,
    SecondaryFixed,
    OnSecondaryFixed,
    SecondaryFixedDim,
    OnSecondaryFixedVariant,
    TertiaryFixed,
    OnTertiaryFixed,
    TertiaryFixedDim,
    OnTertiaryFixedVariant,
    SurfaceDim,
    SurfaceBright,
    SurfaceContainerLowest,
    SurfaceContainerLow,
    SurfaceContainer,
    SurfaceContainerHigh,
    SurfaceContainerHighest,
    Success,
    OnSuccess,
    SuccessContainer,
    OnSuccessContainer,
    Warning,
    OnWarning,
    WarningContainer,
    OnWarningContainer,
    Info,
    OnInfo,
    InfoContainer,
    OnInfoContainer
}

/// <summary>
/// Provides extension methods for the <see cref="TnTColor"/> enum.
/// </summary>
public static partial class TnTColorEnumExt {

    /// <summary>
    /// Converts a nullable <see cref="TnTColor"/> enum value to its corresponding CSS class name.
    /// </summary>
    /// <param name="tnTColorEnum">The nullable <see cref="TnTColor"/> enum value.</param>
    /// <returns>The CSS class name as a string, or an empty string if the value is null.</returns>
    public static string ToCssClassName(this TnTColor? tnTColorEnum) {
        if (tnTColorEnum.HasValue) {
            return tnTColorEnum.Value.ToCssClassName();
        }
        else {
            return string.Empty;
        }
    }

    /// <summary>
    /// Converts a <see cref="TnTColor"/> enum value to its corresponding CSS class name.
    /// </summary>
    /// <param name="tnTColorEnum">The <see cref="TnTColor"/> enum value.</param>
    /// <returns>The CSS class name as a string.</returns>
    public static string ToCssClassName(this TnTColor tnTColorEnum) {
        return FindAllCapitalsExceptFirstLetter().Replace(tnTColorEnum.ToString(), @"-$1").ToLower();
    }

    /// <summary>
    /// Finds all capital letters in a string except the first letter.
    /// </summary>
    /// <returns>A <see cref="Regex"/> object that matches all capital letters except the first letter.</returns>
    [GeneratedRegex(@"(?<=.)([A-Z])")]
    private static partial Regex FindAllCapitalsExceptFirstLetter();
}
