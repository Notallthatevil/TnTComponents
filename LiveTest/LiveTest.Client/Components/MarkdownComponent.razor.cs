using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace LiveTest.Client.Components;

/// <summary>
///     Demonstrates the Markdown editor with form validation.
/// </summary>
public partial class MarkdownComponent : ComponentBase {
    private MarkdownDemoModel Model { get; } = new();

    private string? BlurMessage { get; set; }

    private string? SubmitMessage { get; set; }

    private string? RenderedHtmlValue => Model.RenderedHtml?.Value;

    private Task HandleBlurAsync(FocusEventArgs args) {
        BlurMessage = "Editor blurred.";
        return Task.CompletedTask;
    }

    private Task HandleInvalidSubmit(EditContext editContext) {
        SubmitMessage = "Form is invalid.";
        return Task.CompletedTask;
    }

    private Task HandleValidSubmit(EditContext editContext) {
        SubmitMessage = "Form is valid.";
        return Task.CompletedTask;
    }

    private sealed class MarkdownDemoModel {
        [Required]
        public string? Value { get; set; }
        [Required]
        public MarkupString? RenderedHtml { get; set; }

    }
}
