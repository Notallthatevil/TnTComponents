using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TnTComponents;

public enum TnTColor {
    Transparent,
    Black,
    White,
    Primary,
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
    ErrorContainer,
    OnError,
    OnErrorContainer,
    Background,
    OnBackground,
    Surface,
    OnSurface,
    SurfaceVariant,
    OnSurfaceVariant,
    SurfaceContainerHighest,
    SurfaceContainerHigh,
    SurfaceContainer,
    SurfaceContainerLow,
    SurfaceContainerLowest,
    Outline,
    InverseOnSurface,
    InverseSurface,
    InversePrimary,
    Shadow,
    SurfaceTint,
    OutlineVariant,
    Scrim,
    Warning,
    OnWarning,
    WarningContainer,
    OnWarningContainer,
    Success,
    OnSuccess,
    SuccessContainer,
    OnSuccessContainer,
    Info,
    OnInfo,
    InfoContainer,
    OnInfoContainer,
}

public static partial class TnTColorEnumExt {

    public static string ToCssClassName(this TnTColor tnTColorEnum) {
        return FindAllCapitalsExceptFirstLetter().Replace(tnTColorEnum.ToString(), @"-$1").ToLower();
    }

    [GeneratedRegex(@"(?<=.)([A-Z])")]
    private static partial Regex FindAllCapitalsExceptFirstLetter();
}