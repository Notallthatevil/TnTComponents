using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace TnTComponents;

/// <summary>
///     Defines the contract for a TnT form, exposing appearance, disabled, and read-only state. Implementations provide metadata for form rendering and state management in TnTComponents.
/// </summary>
public interface ITnTForm {

    /// <summary>
    ///     Gets the visual appearance of the form, which controls its style and layout.
    /// </summary>
    FormAppearance Appearance { get; }

    /// <summary>
    ///     Gets a value indicating whether the form is disabled. When true, user input is blocked.
    /// </summary>
    bool Disabled { get; }

    /// <summary>
    ///     Gets a value indicating whether the form is read-only. When true, input fields cannot be edited but may be copied.
    /// </summary>
    bool ReadOnly { get; }
}

/// <summary>
///     A Blazor form component that extends <see cref="EditForm" /> and implements <see cref="ITnTForm" />. Provides additional parameters for appearance, disabled, and read-only state, and supplies
///     itself as a cascading value for child components to access form metadata and state. This class is sealed for performance as it is not intended to be inherited.
/// </summary>
public sealed class TnTForm : EditForm, ITnTForm {

    /// <inheritdoc />
    [Parameter]
    public FormAppearance Appearance { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<ITnTForm>>(0);
        builder.AddComponentParameter(10, nameof(CascadingValue<ITnTForm>.Value), this);
        builder.AddComponentParameter(20, nameof(CascadingValue<ITnTForm>.IsFixed), true);
        builder.AddComponentParameter(30, nameof(CascadingValue<ITnTForm>.ChildContent), new RenderFragment(base.BuildRenderTree));
        builder.CloseComponent();
    }
}