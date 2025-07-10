using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
/// MaterialIcon component for displaying Material Design icons.
/// </summary>
public sealed partial class MaterialIcon : TnTIcon {

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-icon")
        .AddClass("material-symbols-outlined", Appearance is IconAppearance.Default or IconAppearance.Outlined)
        .AddClass("material-symbols-sharp", Appearance == IconAppearance.Sharp)
        .AddClass("material-symbols-rounded", Appearance == IconAppearance.Round)
        .AddClass("mi-small", Size == IconSize.Small)
        .AddClass("mi-medium", Size == IconSize.Medium)
        .AddClass("mi-large", Size == IconSize.Large)
        .AddClass("mi-extra-large", Size == IconSize.ExtraLarge)
        .AddClass(AdditionalClass, !string.IsNullOrWhiteSpace(AdditionalClass))
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    /// Base constructor for MaterialIcon.
    /// </summary>
    public MaterialIcon() { }
    internal MaterialIcon(string icon) : base(icon) { }

    /// <summary>
    /// Implicit conversion from string to MaterialIcon.
    /// </summary>
    /// <param name="icon"></param>
    public static implicit operator string(MaterialIcon icon) => icon.Icon;
}