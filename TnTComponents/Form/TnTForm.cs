using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Form;

namespace TnTComponents;

/// <summary>
///     Interface representing the properties and behavior of a TnTForm.
/// </summary>
public interface ITnTForm {

    /// <summary>
    ///     Gets the appearance of the form.
    /// </summary>
    FormAppearance Appearance { get; }

    /// <summary>
    ///     Gets a value indicating whether the form is disabled.
    /// </summary>
    bool Disabled { get; }

    /// <summary>
    ///     Gets a value indicating whether the form is read-only.
    /// </summary>
    bool ReadOnly { get; }
}

/// <summary>
///     Represents a form component that extends <see cref="EditForm" /> and implements <see
///     cref="ITnTForm" />.
/// </summary>
public class TnTForm : EditForm, ITnTForm {

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<ITnTForm>>(0);
        builder.AddComponentParameter(10, nameof(CascadingValue<ITnTForm>.Value), this);
        builder.AddComponentParameter(20, nameof(CascadingValue<ITnTForm>.IsFixed), true);
        builder.AddComponentParameter(30, nameof(CascadingValue<ITnTForm>.ChildContent), new RenderFragment(base.BuildRenderTree));
        builder.CloseComponent();
    }
}