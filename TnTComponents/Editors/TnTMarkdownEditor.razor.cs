using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.RegularExpressions;
using TnTComponents.Core;

namespace TnTComponents;

/// <summary>
///     A Blazor component for editing Markdown content.
/// </summary>
public partial class TnTMarkdownEditor {

    /// <inheritdoc />
    public override string? ElementClass => string.Empty;

    /// <inheritdoc />
    public override string? ElementStyle => string.Empty;

    /// <summary>
    ///     The initial value of the Markdown content.
    /// </summary>
    [Parameter]
    public string? InitialValue { get; set; }

    /// <inheritdoc />
    public override string? JsModulePath => "./_content/TnTComponents/Editors/TnTMarkdownEditor.razor.js";

    /// <summary>
    ///     Gets or sets the rendered HTML content.
    /// </summary>
    [Parameter]
    public MarkupString? RenderedHtml { get; set; }

    /// <summary>
    ///     Event callback for when the rendered HTML content changes.
    /// </summary>
    [Parameter]
    public EventCallback<MarkupString?> RenderedHtmlChanged { get; set; }

    /// <summary>
    ///     Gets or sets the Markdown content.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    ///     Event callback for when the Markdown content changes.
    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    /// <summary>
    ///     Updates the Markdown content and the rendered HTML content.
    /// </summary>
    /// <param name="value">       The new Markdown content.</param>
    /// <param name="renderedText">The rendered HTML content.</param>
    [JSInvokable]
    public async Task UpdateValue(string value, string renderedText) {
        Value = value;
        await ValueChanged.InvokeAsync(Value);
        var body = BodyRegex().Match(renderedText).Groups[1].Value;
        if (body is not null) {
            RenderedHtml = new MarkupString(body);
            await RenderedHtmlChanged.InvokeAsync(RenderedHtml);
        }
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void OnInitialized() {
        base.OnInitialized();
        ElementId = TnTComponentIdentifier.NewId();
    }

    [GeneratedRegex(@"<body>((.|\r|\n)*)<\/body>")]
    private static partial Regex BodyRegex();
}