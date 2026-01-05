using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using NTComponents.Core;
using NTComponents.Wizard;

namespace NTComponents;

/// <summary>
///     Represents a wizard component that manages multiple steps and provides navigation between them.
/// </summary>
public partial class TnTWizard : TnTComponentBase {

    /// <summary>
    ///     The child content to be rendered inside the wizard.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment ChildContent { get; set; } = default!;

    /// <inheritdoc />
    public override string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .AddClass("tnt-wizard")
        .AddClass("tnt-layout-horizontal", LayoutDirection == LayoutDirection.Horizontal)
        .Build();

    /// <inheritdoc />
    public override string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes)
        .Build();

    /// <summary>
    ///     The visual layout style of the wizard component.
    /// </summary>
    [Parameter]
    public LayoutDirection LayoutDirection { get; set; } = LayoutDirection.Vertical;

    /// <summary>
    ///     A value indicating whether the "Next" button is disabled.
    /// </summary>
    [Parameter]
    public bool NextButtonDisabled { get; set; }

    /// <summary>
    ///     Callback invoked when the "Next" button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<int> OnNextButtonClicked { get; set; }

    /// <summary>
    ///     Callback invoked when the "Previous" button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<int> OnPreviousButtonClicked { get; set; }

    /// <summary>
    ///     The callback to be invoked when the wizard is submitted.
    /// </summary>
    [Parameter]
    public EventCallback OnSubmitCallback { get; set; }

    /// <summary>
    ///     A value indicating whether the "Previous" button is disabled.
    /// </summary>
    [Parameter]
    public bool PreviousButtonDisabled { get; set; }

    /// <summary>
    ///     A value indicating whether the "Submit" button is disabled.
    /// </summary>
    [Parameter]
    public bool SubmitButtonDisabled { get; set; }

    /// <summary>
    ///     The title of the wizard, displayed at the top.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    ///     Gets the current step in the wizard.
    /// </summary>
    private TnTWizardStepBase? _currentStep => _steps.ElementAtOrDefault(_stepIndex);

    /// <summary>
    ///     Static field to generate unique IDs for child steps.
    /// </summary>
    private static int _childId;

    /// <summary>
    ///     List of all steps added to the wizard.
    /// </summary>
    private readonly List<TnTWizardStepBase> _steps = [];

    /// <summary>
    ///     Index of the currently active step.
    /// </summary>
    private int _stepIndex;

    /// <summary>
    ///     Adds a child step to the wizard.
    /// </summary>
    /// <param name="step">The step to add.</param>
    /// <exception cref="ArgumentNullException">Thrown if the step is null.</exception>
    internal void AddChildStep(TnTWizardStepBase step) {
        ArgumentNullException.ThrowIfNull(step);
        if (step._internalId == -1) {
            step._internalId = Interlocked.Increment(ref _childId);
        }

        if (!_steps.Any(s => s._internalId == step._internalId)) {
            _steps.Add(step);
        }
        StateHasChanged();
    }

    /// <summary>
    ///     Advances to the next step in the wizard.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task NextStepAsync() {
        if (await ValidateCurrentStepAsync()) {
            await OnNextButtonClicked.InvokeAsync(_stepIndex + 1);
            _stepIndex++;
        }
        return;
    }

    /// <summary>
    ///     Navigates to the previous step in the wizard.
    /// </summary>
    internal async Task PreviousStepAsync() {
        if (_stepIndex > 0) {
            await OnPreviousButtonClicked.InvokeAsync(_stepIndex);
            _stepIndex--;
        }
    }

    /// <summary>
    ///     Removes a child step from the wizard.
    /// </summary>
    /// <param name="step">The step to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown if the step is null.</exception>
    internal void RemoveChildStep(TnTWizardStepBase step) {
        ArgumentNullException.ThrowIfNull(step);
        _steps.RemoveAll(s => s._internalId == step._internalId);
        StateHasChanged();
    }

    /// <summary>
    ///     Handles the key press event for keyboard input, advancing the workflow or submitting the form when the Enter key is pressed.
    /// </summary>
    /// <remarks>If the Enter key is pressed on the final step, the form is submitted; otherwise, the workflow advances to the next step.</remarks>
    /// <param name="args">The keyboard event arguments containing information about the key that was pressed.</param>
    /// <returns>A task that represents the asynchronous operation of handling the key press event.</returns>
    private async Task HandleKeyPressAsync(KeyboardEventArgs args) {
        if (args.Key == "Enter") {
            if (_stepIndex + 1 == _steps.Count) {
                if (!SubmitButtonDisabled) {
                    await SubmitClickedAsync();
                }
            }
            else {
                if (!NextButtonDisabled) {
                    await NextStepAsync();
                }
            }
        }
    }

    /// <summary>
    ///     Invokes the submit callback when the submit button is clicked.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SubmitClickedAsync() {
        if (await ValidateCurrentStepAsync()) {
            await OnSubmitCallback.InvokeAsync();
        }
    }

    private async Task<bool> ValidateCurrentStepAsync() => _currentStep is not TnTWizardFormStep formStep || formStep is null || await formStep.FormValidAsync();
}