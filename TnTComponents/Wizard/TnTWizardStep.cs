﻿using Microsoft.AspNetCore.Components;
using TnTComponents.Wizard;

namespace TnTComponents;

/// <summary>
///     Represents a single step in the TnT wizard component.
/// </summary>
public class TnTWizardStep : TnTWizardStepBase {

    /// <summary>
    ///     The content to be rendered inside the wizard step.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    internal override RenderFragment Render() => new(builder => builder.AddContent(0, ChildContent));
}