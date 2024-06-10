using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TnTComponents.Core;

namespace TnTComponents;
public partial class TnTMarkdownEditor {

    public override string? JsModulePath => "./_content/TnTComponents/Markdown/TnTMarkdownEditor.razor.js";

    public override string? CssClass => throw new NotImplementedException();

    public override string? CssStyle => throw new NotImplementedException();


    protected override void OnInitialized() {
        base.OnInitialized();
        Id = TnTComponentIdentifier.NewId();
    }
}
