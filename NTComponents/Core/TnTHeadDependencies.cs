using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace NTComponents;

/// <summary>
///     Meant to be placed in the head section of App.razor to include necessary dependencies for NTComponents.
/// </summary>
[ExcludeFromCodeCoverage]
public class TnTHeadDependencies : IComponent {
    private RenderHandle _renderHandle;

    /// <inheritdoc />
    public void Attach(RenderHandle renderHandle) => _renderHandle = renderHandle;

    /// <inheritdoc />
    public Task SetParametersAsync(ParameterView parameters) {
        _renderHandle.Render(Render);
        return Task.CompletedTask;
    }

    private void Render(RenderTreeBuilder builder) {
        // <link rel="preconnect" href="https://fonts.googleapis.com">
        builder.OpenElement(0, "link");
        builder.AddAttribute(1, "rel", "preconnect");
        builder.AddAttribute(2, "href", "https://fonts.googleapis.com");
        builder.CloseElement();

        // <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
        builder.OpenElement(3, "link");
        builder.AddAttribute(4, "rel", "preconnect");
        builder.AddAttribute(5, "href", "https://fonts.gstatic.com");
        builder.AddAttribute(6, "crossorigin", string.Empty);
        builder.CloseElement();

        // <link href="https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap" rel="stylesheet">
        builder.OpenElement(7, "link");
        builder.AddAttribute(8, "href", "https://fonts.googleapis.com/css2?family=Roboto:ital,wght@0,100..900;1,100..900&display=swap");
        builder.AddAttribute(9, "rel", "stylesheet");
        builder.CloseElement();

        // <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Material+Symbols+Sharp" />
        builder.OpenElement(10, "link");
        builder.AddAttribute(11, "rel", "stylesheet");
        builder.AddAttribute(12, "href", "https://fonts.googleapis.com/css2?family=Material+Symbols+Sharp");
        builder.CloseElement();

        // <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded" />
        builder.OpenElement(13, "link");
        builder.AddAttribute(14, "rel", "stylesheet");
        builder.AddAttribute(15, "href", "https://fonts.googleapis.com/css2?family=Material+Symbols+Rounded");
        builder.CloseElement();

        // <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" />
        builder.OpenElement(16, "link");
        builder.AddAttribute(17, "rel", "stylesheet");
        builder.AddAttribute(18, "href", "https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined");
        builder.CloseElement();
    }
}