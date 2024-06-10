using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using TnTComponents.Core;

namespace TnTComponents;
public partial class TnTMarkdownEditor {

    public override string? JsModulePath => "./_content/TnTComponents/Markdown/TnTMarkdownEditor.razor.js";

    public override string? CssClass => string.Empty;

    public override string? CssStyle => string.Empty;

    [Parameter]
    public string? Value { get; set; }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public MarkupString? RenderedHtml { get; set; }

    [Parameter]
    public EventCallback<MarkupString?> RenderedHtmlChanged { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        Id = TnTComponentIdentifier.NewId();
    }


    [JSInvokable]
    public async Task UpdateValue(string value, string renderedText) {
        Value = value;
        await ValueChanged.InvokeAsync(Value);
        var body = Regex.Match(renderedText, @"<body>((.|\r|\n)*)<\/body>").Groups[1].Value;
        if (body is not null) {
            RenderedHtml = new MarkupString(body);
            await RenderedHtmlChanged.InvokeAsync(RenderedHtml);
        }
        StateHasChanged();
    }
}
