using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;

namespace TnTComponents;

public sealed partial class MaterialIcon : TnTIcon {
    public override string? CssClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-icons")
        .AddClass("material-symbols-outlined", Appearance == IconAppearance.Default || Appearance == IconAppearance.Outlined)
        .AddClass("material-symbols-sharp", Appearance == IconAppearance.Sharp)
        .AddClass("material-symbols-rounded", Appearance == IconAppearance.Round)
        .AddClass("mi-small", Size == IconSize.Small)
        .AddClass("mi-medium", Size == IconSize.Medium)
        .AddClass("mi-large", Size == IconSize.Large)
        .AddClass("mi-extra-large", Size == IconSize.ExtraLarge)
        .AddClass(AdditionalClass, AdditionalClass is not null)
        .Build();

    public override string? CssStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();


    public MaterialIcon() : base() { }

    public MaterialIcon(string icon) : base(icon) { }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        base.BuildRenderTree(builder);
    }
}