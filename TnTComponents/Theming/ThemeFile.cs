
namespace TnTComponents.Theming;

internal class ThemeFile {
    public string? Description { get; set; } = "TYPE: CUSTOM\nMaterial Theme Builder export 2024-07-03 09:43:27";
    public string? Seed { get; set; } = "#B33B15";
    public CoreColors? CoreColors { get; set; }
    public Scheme? Schemes { get; set; }
}

internal class CoreColors {
    public string? Primary { get; set; } = "#B33B15";
    public string? Secondary { get; set; }
    public string? Tertiary { get; set; }
    public string? Neutral { get; set; }
    public string? NeutralVariant { get; set; }
}

internal class Scheme {
    public ColorScheme? Light { get; set; } = new() {
        Primary = "#8F4C38",
        SurfaceTint = "#8F4C38",
        OnPrimary = "#FFFFFF",
        PrimaryContainer = "#FFDBD1",
        OnPrimaryContainer = "#3A0B01",
        Secondary = "#77574E",
        OnSecondary = "#FFFFFF",
        SecondaryContainer = "#FFDBD1",
        OnSecondaryContainer = "#2C150F",
        Tertiary = "#6C5D2F",
        OnTertiary = "#FFFFFF",
        TertiaryContainer = "#F5E1A7",
        OnTertiaryContainer = "#231B00",
        Error = "#BA1A1A",
        OnError = "#FFFFFF",
        ErrorContainer = "#FFDAD6",
        OnErrorContainer = "#410002",
        Background = "#FFF8F6",
        OnBackground = "#231917",
        Surface = "#FFF8F6",
        OnSurface = "#231917",
        SurfaceVariant = "#F5DED8",
        OnSurfaceVariant = "#53433F",
        Outline = "#85736E",
        OutlineVariant = "#D8C2BC",
        Shadow = "#ababab",
        Scrim = "#000000",
        InverseSurface = "#392E2B",
        InverseOnSurface = "#FFEDE8",
        InversePrimary = "#FFB5A0",
        PrimaryFixed = "#FFDBD1",
        OnPrimaryFixed = "#3A0B01",
        PrimaryFixedDim = "#FFB5A0",
        OnPrimaryFixedVariant = "#723523",
        SecondaryFixed = "#FFDBD1",
        OnSecondaryFixed = "#2C150F",
        SecondaryFixedDim = "#E7BDB2",
        OnSecondaryFixedVariant = "#5D4037",
        TertiaryFixed = "#F5E1A7",
        OnTertiaryFixed = "#231B00",
        TertiaryFixedDim = "#D8C58D",
        OnTertiaryFixedVariant = "#534619",
        SurfaceDim = "#E8D6D2",
        SurfaceBright = "#FFF8F6",
        SurfaceContainerLowest = "#FFFFFF",
        SurfaceContainerLow = "#FFF1ED",
        SurfaceContainer = "#FCEAE5",
        SurfaceContainerHigh = "#F7E4E0",
        SurfaceContainerHighest = "#F1DFDA",
        Success = "#2E6A44",
        OnSuccess = "#FFFFFF",
        SuccessContainer = "#B1F1C1",
        OnSuccessContainer = "#00210E",
        Info = "#3F5F90",
        OnInfo = "#FFFFFF",
        InfoContainer = "#D6E3FF",
        OnInfoContainer = "#001B3D",
        Warning = "#656015",
        OnWarning = "#FFFFFF",
        WarningContainer = "#ECE68D",
        OnWarningContainer = "#1E1C00"
    };
    public ColorScheme? Dark { get; set; } = new() {
        Primary = "#FFB5A0",
        SurfaceTint = "#FFB5A0",
        OnPrimary = "#561F0F",
        PrimaryContainer = "#723523",
        OnPrimaryContainer = "#FFDBD1",
        Secondary = "#E7BDB2",
        OnSecondary = "#442A22",
        SecondaryContainer = "#5D4037",
        OnSecondaryContainer = "#FFDBD1",
        Tertiary = "#D8C58D",
        OnTertiary = "#3B2F05",
        TertiaryContainer = "#534619",
        OnTertiaryContainer = "#F5E1A7",
        Error = "#FFB4AB",
        OnError = "#690005",
        ErrorContainer = "#93000A",
        OnErrorContainer = "#FFDAD6",
        Background = "#1A110F",
        OnBackground = "#F1DFDA",
        Surface = "#1A110F",
        OnSurface = "#F1DFDA",
        SurfaceVariant = "#53433F",
        OnSurfaceVariant = "#D8C2BC",
        Outline = "#A08C87",
        OutlineVariant = "#53433F",
        Shadow = "#dedede",
        Scrim = "#000000",
        InverseSurface = "#F1DFDA",
        InverseOnSurface = "#392E2B",
        InversePrimary = "#8F4C38",
        PrimaryFixed = "#FFDBD1",
        OnPrimaryFixed = "#3A0B01",
        PrimaryFixedDim = "#FFB5A0",
        OnPrimaryFixedVariant = "#723523",
        SecondaryFixed = "#FFDBD1",
        OnSecondaryFixed = "#2C150F",
        SecondaryFixedDim = "#E7BDB2",
        OnSecondaryFixedVariant = "#5D4037",
        TertiaryFixed = "#F5E1A7",
        OnTertiaryFixed = "#231B00",
        TertiaryFixedDim = "#D8C58D",
        OnTertiaryFixedVariant = "#534619",
        SurfaceDim = "#1A110F",
        SurfaceBright = "#423734",
        SurfaceContainerLowest = "#140C0A",
        SurfaceContainerLow = "#231917",
        SurfaceContainer = "#271D1B",
        SurfaceContainerHigh = "#322825",
        SurfaceContainerHighest = "#3D322F",
        Success = "#2E6A44",
        OnSuccess = "#FFFFFF",
        SuccessContainer = "#B1F1C1",
        OnSuccessContainer = "#00210E",
        Info = "#3F5F90",
        OnInfo = "#FFFFFF",
        InfoContainer = "#D6E3FF",
        OnInfoContainer = "#001B3D",
        Warning = "#656015",
        OnWarning = "#FFFFFF",
        WarningContainer = "#ECE68D",
        OnWarningContainer = "#1E1C00"
    };
}

internal class ColorScheme {
    public string? Primary { get; set; } = "#8F4C38";
    public string? SurfaceTint { get; set; } = "#8F4C38";
    public string? OnPrimary { get; set; } = "#FFFFFF";
    public string? PrimaryContainer { get; set; } = "#FFDBD1";
    public string? OnPrimaryContainer { get; set; } = "#3A0B01";
    public string? Secondary { get; set; } = "#77574E";
    public string? OnSecondary { get; set; } = "#FFFFFF";
    public string? SecondaryContainer { get; set; } = "#FFDBD1";
    public string? OnSecondaryContainer { get; set; } = "#2C150F";
    public string? Tertiary { get; set; } = "#6C5D2F";
    public string? OnTertiary { get; set; } = "#FFFFFF";
    public string? TertiaryContainer { get; set; } = "#F5E1A7";
    public string? OnTertiaryContainer { get; set; } = "#231B00";
    public string? Error { get; set; } = "#BA1A1A";
    public string? OnError { get; set; } = "#FFFFFF";
    public string? ErrorContainer { get; set; } = "#FFDAD6";
    public string? OnErrorContainer { get; set; } = "#410002";
    public string? Background { get; set; } = "#FFF8F6";
    public string? OnBackground { get; set; } = "#231917";
    public string? Surface { get; set; } = "#FFF8F6";
    public string? OnSurface { get; set; } = "#231917";
    public string? SurfaceVariant { get; set; } = "#F5DED8";
    public string? OnSurfaceVariant { get; set; } = "#53433F";
    public string? Outline { get; set; } = "#85736E";
    public string? OutlineVariant { get; set; } = "#D8C2BC";
    public string? Shadow { get; set; } = "#ababab";
    public string? Scrim { get; set; } = "#000000";
    public string? InverseSurface { get; set; } = "#392E2B";
    public string? InverseOnSurface { get; set; } = "#FFEDE8";
    public string? InversePrimary { get; set; } = "#FFB5A0";
    public string? PrimaryFixed { get; set; } = "#FFDBD1";
    public string? OnPrimaryFixed { get; set; } = "#3A0B01";
    public string? PrimaryFixedDim { get; set; } = "#FFB5A0";
    public string? OnPrimaryFixedVariant { get; set; } = "#723523";
    public string? SecondaryFixed { get; set; } = "#FFDBD1";
    public string? OnSecondaryFixed { get; set; } = "#2C150F";
    public string? SecondaryFixedDim { get; set; } = "#E7BDB2";
    public string? OnSecondaryFixedVariant { get; set; } = "#5D4037";
    public string? TertiaryFixed { get; set; } = "#F5E1A7";
    public string? OnTertiaryFixed { get; set; } = "#231B00";
    public string? TertiaryFixedDim { get; set; } = "#D8C58D";
    public string? OnTertiaryFixedVariant { get; set; } = "#534619";
    public string? SurfaceDim { get; set; } = "#E8D6D2";
    public string? SurfaceBright { get; set; } = "#FFF8F6";
    public string? SurfaceContainerLowest { get; set; } = "#FFFFFF";
    public string? SurfaceContainerLow { get; set; } = "#FFF1ED";
    public string? SurfaceContainer { get; set; } = "#FCEAE5";
    public string? SurfaceContainerHigh { get; set; } = "#F7E4E0";
    public string? SurfaceContainerHighest { get; set; } = "#F1DFDA";
    public string? Success { get; set; } = "#2E6A44";
    public string? OnSuccess { get; set; } = "#FFFFFF";
    public string? SuccessContainer { get; set; } = "#B1F1C1";
    public string? OnSuccessContainer { get; set; } = "#00210E";
    public string? Info { get; set; } = "#3F5F90";
    public string? OnInfo { get; set; } = "#FFFFFF";
    public string? InfoContainer { get; set; } = "#D6E3FF";
    public string? OnInfoContainer { get; set; } = "#001B3D";
    public string? Warning { get; set; } = "#656015";
    public string? OnWarning { get; set; } = "#FFFFFF";
    public string? WarningContainer { get; set; } = "#ECE68D";
    public string? OnWarningContainer { get; set; } = "#1E1C00";

}