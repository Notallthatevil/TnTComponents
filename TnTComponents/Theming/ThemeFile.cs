
namespace TnTComponents.Theming;

internal class ThemeFile {
    public string? Description { get; set; } 
    public string? Seed { get; set; } 
    public CoreColors? CoreColors { get; set; } 
    public Scheme? Schemes { get; set; } 
}

internal class CoreColors {
    public string? Primary { get; set; } 
    public string? Secondary { get; set; } 
    public string? Tertiary { get; set; } 
    public string? Neutral { get; set; } 
    public string? NeutralVariant { get; set; } 
}

internal class Scheme {
    public ColorScheme? Light { get; set; } 
    public ColorScheme? Dark { get; set; } 
}

internal class ColorScheme {
    public string? Primary { get; set; } 
    public string? SurfaceTint { get; set; } 
    public string? OnPrimary { get; set; } 
    public string? PrimaryContainer { get; set; } 
    public string? OnPrimaryContainer { get; set; } 
    public string? Secondary { get; set; } 
    public string? OnSecondary { get; set; } 
    public string? SecondaryContainer { get; set; } 
    public string? OnSecondaryContainer { get; set; } 
    public string? Tertiary { get; set; } 
    public string? OnTertiary { get; set; } 
    public string? TertiaryContainer { get; set; } 
    public string? OnTertiaryContainer { get; set; } 
    public string? Error { get; set; } 
    public string? OnError { get; set; } 
    public string? ErrorContainer { get; set; } 
    public string? OnErrorContainer { get; set; } 
    public string? Background { get; set; } 
    public string? OnBackground { get; set; } 
    public string? Surface { get; set; } 
    public string? OnSurface { get; set; } 
    public string? SurfaceVariant { get; set; } 
    public string? OnSurfaceVariant { get; set; } 
    public string? Outline { get; set; } 
    public string? OutlineVariant { get; set; } 
    public string? Shadow { get; set; } 
    public string? Scrim { get; set; } 
    public string? InverseSurface { get; set; } 
    public string? InverseOnSurface { get; set; } 
    public string? InversePrimary { get; set; } 
    public string? PrimaryFixed { get; set; } 
    public string? OnPrimaryFixed { get; set; } 
    public string? PrimaryFixedDim { get; set; } 
    public string? OnPrimaryFixedVariant { get; set; } 
    public string? SecondaryFixed { get; set; } 
    public string? OnSecondaryFixed { get; set; } 
    public string? SecondaryFixedDim { get; set; } 
    public string? OnSecondaryFixedVariant { get; set; } 
    public string? TertiaryFixed { get; set; } 
    public string? OnTertiaryFixed { get; set; } 
    public string? TertiaryFixedDim { get; set; } 
    public string? OnTertiaryFixedVariant { get; set; } 
    public string? SurfaceDim { get; set; } 
    public string? SurfaceBright { get; set; } 
    public string? SurfaceContainerLowest { get; set; } 
    public string? SurfaceContainerLow { get; set; } 
    public string? SurfaceContainer { get; set; } 
    public string? SurfaceContainerHigh { get; set; } 
    public string? SurfaceContainerHighest { get; set; } 
    public string? Success { get; set; } 
    public string? OnSuccess { get; set; } 
    public string? SuccessContainer { get; set; } 
    public string? OnSuccessContainer { get; set; } 
    public string? Warning { get; set; } 
    public string? OnWarning { get; set; } 
    public string? WarningContainer { get; set; } 
    public string? OnWarningContainer { get; set; } 
    public string? Info { get; set; } 
    public string? OnInfo { get; set; } 
    public string? InfoContainer { get; set; } 
    public string? OnInfoContainer { get; set; } 

}