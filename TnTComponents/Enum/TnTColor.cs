using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TnTComponents;

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

public static partial class TnTColorEnumExt {

    public static string ToCssClassName(this TnTColor? tnTColorEnum) {
        if (tnTColorEnum.HasValue) {
            return tnTColorEnum.Value.ToCssClassName();
        }
        else {
            return string.Empty;
        }
    }

    public static string ToCssClassName(this TnTColor tnTColorEnum) {
        return FindAllCapitalsExceptFirstLetter().Replace(tnTColorEnum.ToString(), @"-$1").ToLower();
    }

    [GeneratedRegex(@"(?<=.)([A-Z])")]
    private static partial Regex FindAllCapitalsExceptFirstLetter();
}