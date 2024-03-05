using TnTComponents.Core;

namespace TnTComponents;

public partial class TnTThemeToggle {
    public override string? CssClass => string.Empty;

    public override string? CssStyle => CssStyleBuilder.Create()
   .AddFromAdditionalAttributes(AdditionalAttributes)
   .Build();

    protected override string? JsModulePath => "./_content/TnTComponents/Theming/TnTThemeToggle.razor.js";
    protected override bool RunIsolatedJsScript => true;
}