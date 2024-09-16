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
        .AddClass(AdditionalClass, AdditionalClass is not null)
        .Build();

    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        base.BuildRenderTree(builder);
    }
}