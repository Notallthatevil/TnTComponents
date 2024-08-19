using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTThemeToggle {
    public override string? ElementClass => string.Empty;

    public override string? ElementStyle => CssStyleBuilder.Create()
   .AddFromAdditionalAttributes(AdditionalAttributes)
   .Build();

    public override string? JsModulePath => "./_content/TnTComponents/Theming/TnTThemeToggle.razor.js";
}