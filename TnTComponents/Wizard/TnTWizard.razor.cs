using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TnTComponents.Core;
using TnTComponents.Wizard;

namespace TnTComponents;

/// <summary>
///     Represents a wizard component that manages multiple steps and provides navigation between them.
/// </summary>
public partial class TnTWizard : TnTComponentBase {

    /// <summary>
    ///     The visual layout style of the wizard component.
    /// </summary>
    [Parameter]
    public LayoutDirection LayoutDirection { get; set; } = LayoutDirection.Vertical;

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
        if (_currentStep is TnTWizardFormStep formStep && formStep is not null && !await formStep.FormValidAsync()) {
            return;
        }
        await OnNextButtonClicked.InvokeAsync(_stepIndex + 1);
        _stepIndex++;
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
    ///     Invokes the submit callback when the submit button is clicked.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private Task SubmitClickedAsync() => OnSubmitCallback.InvokeAsync();
}