using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;

namespace TnTComponents;
public partial class TnTDocumentPreview :ComponentBase{
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    [Parameter]
    public string Url { get; set; } = default!;

    [Parameter, EditorRequired]
    public string ContentType { get; set; } = default!;

    [Parameter]
    public Stream Stream { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "embed");
        if(AdditionalAttributes is not null) {
            builder.AddMultipleAttributes(10, AdditionalAttributes!);
        }
        builder.AddAttribute(20, "src", GetSource());
        builder.AddAttribute(30, "type", ContentType);
        builder.CloseComponent();
    }

    protected override void OnInitialized() {
        base.OnInitialized();
        if (string.IsNullOrWhiteSpace(Url) && Stream is null) {
            throw new InvalidOperationException("Either Url or Stream must be provided.");
        }

        if (!string.IsNullOrWhiteSpace(Url) && Stream?.Length > 0) {
            throw new InvalidOperationException("Only one of Url or Stream can be provided.");
        }
    }

    private string GetSource() {
        if (Url is not null) {
            return Url;
        }
        else {
            using var ms = new MemoryStream();
            Stream.CopyTo(ms);
            return $"data:{ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
        }
    }
}
