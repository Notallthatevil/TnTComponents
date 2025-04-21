using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public sealed partial class MaterialIcon : TnTIcon {

    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-icon")
        .AddClass("material-symbols-outlined", Appearance == IconAppearance.Default || Appearance == IconAppearance.Outlined)
        .AddClass("material-symbols-sharp", Appearance == IconAppearance.Sharp)
        .AddClass("material-symbols-rounded", Appearance == IconAppearance.Round)
        .AddClass("mi-small", Size == IconSize.Small)
        .AddClass("mi-medium", Size == IconSize.Medium)
        .AddClass("mi-large", Size == IconSize.Large)
        .AddClass("mi-extra-large", Size == IconSize.ExtraLarge)
        .AddClass(AdditionalClass, !string.IsNullOrWhiteSpace(AdditionalClass))
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    public MaterialIcon() { }
    internal MaterialIcon(string icon) : base(icon) { }

    public static implicit operator string(MaterialIcon icon) => icon.Icon;
}