using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Wizard;

namespace TnTComponents;

/// <summary>
///     Represents a wizard step that contains a form in the TnTComponents library.
/// </summary>
public class TnTWizardFormStep : TnTWizardStepBase {

    /// <summary>
    ///     The child content of the form step, which is a render fragment that takes an <see
    ///     cref="EditContext" />.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<EditContext> ChildContent { get; set; } = default!;

/// <summary>
/// The appearance of the form. <see cref="FormAppearance"/>
/// </summary>
    [Parameter]
    public FormAppearance FormAppearance { get; set; } = FormAppearance.Outlined;

    /// <summary>
    ///     The name of the form. This is optional.
    /// </summary>
    [Parameter]
    public string? FormName { get; set; }

    /// <summary>
    ///     A value indicating whether to include a <see cref="DataAnnotationsValidator" /> in the
    ///     form. Defaults to <c>true</c>.
    /// </summary>
    [Parameter]
    public bool IncludeDataAnnotationsValidator { get; set; } = true;

    /// <summary>
    ///     The model object used for data binding in the form.
    /// </summary>
    [Parameter]
    public object Model { get; set; } = default!;

    /// <summary>
    ///     The callback to invoke when the form submission is invalid.
    /// </summary>
    [Parameter]
    public EventCallback<object> OnInvalidSubmitCallback { get; set; }

    /// <summary>
    ///     The callback to invoke when the form submission is valid.
    /// </summary>
    [Parameter]
    public EventCallback<object> OnValidSubmitCallback { get; set; }

    /// <summary>
    ///     Holds a reference to the <see cref="TnTForm" /> component used in this step.
    /// </summary>
    private TnTForm _form = default!;

    /// <summary>
    ///     Validates the form and invokes the appropriate callback based on the validation result.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that resolves to <c>true</c> if the form is valid;
    ///     otherwise, <c>false</c>.
    /// </returns>
    internal async Task<bool> FormValidAsync() {
        if (_form?.EditContext?.Validate() == true) {
            await OnValidSubmitCallback.InvokeAsync(Model);
            return true;
        }
        else {
            await OnInvalidSubmitCallback.InvokeAsync(Model);
            return false;
        }
    }

    /// <inheritdoc />
    internal override RenderFragment Render() => new(builder => {
        builder.OpenComponent<TnTForm>(0);
        builder.AddComponentParameter(10, nameof(TnTForm.Model), Model);
        builder.AddComponentParameter(15, nameof(TnTForm.Appearance), FormAppearance);
        builder.AddAttribute(20, "class", "tnt-wizard-form");
        if (!string.IsNullOrWhiteSpace(FormName)) {
            builder.AddComponentParameter(30, nameof(TnTForm.FormName), FormName);
        }

        builder.SetKey(_internalId);

        builder.AddComponentParameter(60, nameof(TnTForm.ChildContent), new RenderFragment<EditContext>(editContext => new RenderFragment(b => {
            if (IncludeDataAnnotationsValidator) {
                b.OpenComponent<DataAnnotationsValidator>(0);
                b.CloseComponent();
            }

            b.AddContent(10, ChildContent(editContext));
        })));

        builder.AddComponentReferenceCapture(70, component => {
            if (component is TnTForm form) {
                _form = form;
            }
        });
        builder.CloseComponent();
    });

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();
        ArgumentNullException.ThrowIfNull(Model, nameof(Model));
    }
}