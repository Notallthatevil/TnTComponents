using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

/// <summary>
///     Represents a custom input file component with additional features and styling options.
/// </summary>
public class TnTInputFile : InputFile, ITnTInteractable {

    /// <inheritdoc />
    IReadOnlyDictionary<string, object>? ITnTComponentBase.AdditionalAttributes { get => base.AdditionalAttributes?.ToDictionary(); set => base.AdditionalAttributes = value is null ? null : new Dictionary<string, object>(value!); }

    /// <summary>
    ///     Gets or sets the appearance of the input file component.
    /// </summary>
    [Parameter]
    public FormAppearance Appearance { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool? AutoFocus { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool Disabled { get; set; }

    /// <inheritdoc />
    ElementReference ITnTComponentBase.Element => base.Element ?? default;

    /// <inheritdoc />
    public string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes?.ToDictionary())
        .AddClass("tnt-input-file")
        .AddClass("tnt-input")
        .AddTnTInteractable(this)
        .AddFilled(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Filled : Appearance == FormAppearance.Filled)
        .AddOutlined(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Outlined : Appearance == FormAppearance.Outlined)
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementId { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementLang { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? ElementName { get; set; }

    /// <inheritdoc />
    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes?.ToDictionary())
        .Build();

    /// <inheritdoc />
    [Parameter]
    public string? ElementTitle { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool EnableRipple { get; set; }

    /// <summary>
    ///     Gets a value indicating whether the input file component is disabled, considering the
    ///     form's state.
    /// </summary>
    public bool FieldDisabled => _tntForm?.Disabled is not null ? _tntForm.Disabled : Disabled;

    /// <summary>
    ///     Gets a value indicating whether the input file component is read-only, considering the
    ///     form's state.
    /// </summary>
    public bool FieldReadonly => _tntForm?.ReadOnly is not null ? _tntForm.ReadOnly : ReadOnly;

    /// <summary>
    ///     Gets or sets the maximum number of files that can be selected.
    /// </summary>
    [Parameter]
    public int MaximumFileCount { get; set; } = 1;

    /// <summary>
    ///     Gets or sets the event callback for when the input file changes.
    /// </summary>
    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnInputFileChange { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether the input file component is read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <inheritdoc />
    [Parameter]
    public TnTColor? TintColor { get; set; }

    /// <summary>
    ///     Gets or sets the cascading parameter for the form.
    /// </summary>
    [CascadingParameter]
    private ITnTForm? _tntForm { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddAttribute(10, "class", ElementClass);
        builder.AddAttribute(20, "style", ElementStyle);
        builder.AddAttribute(30, "id", ElementId);
        builder.AddAttribute(40, "lang", ElementLang);
        builder.AddAttribute(50, "title", ElementTitle);
        builder.AddContent(60, new RenderFragment(base.BuildRenderTree));
        builder.CloseElement();
    }

    /// <inheritdoc />
    protected override void OnInitialized() {
        base.OnInitialized();
        OnChange = EventCallback.Factory.Create(this, async (InputFileChangeEventArgs args) => await OnUploadFilesHandlerAsync(args));
    }

    /// <inheritdoc />
    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes?.ToDictionary() ?? new Dictionary<string, object>();
        dict.TryAdd("disabled", FieldDisabled);
        dict.TryAdd("readonly", FieldReadonly);
        AdditionalAttributes = dict;
    }

    /// <summary>
    ///     Handles the file upload event.
    /// </summary>
    /// <param name="e">The input file change event arguments.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ApplicationException">
    ///     Thrown when the number of files exceeds the maximum allowed.
    /// </exception>
    /// <exception cref="NotImplementedException">
    ///     Thrown when the OnInputFileChange event callback is not implemented.
    /// </exception>
    protected async Task OnUploadFilesHandlerAsync(InputFileChangeEventArgs e) {
        if (e.FileCount > MaximumFileCount) {
            throw new ApplicationException($"The maximum number of files accepted is {MaximumFileCount}, but {e.FileCount} were supplied.");
        }

        // Use the native Blazor event
        if (OnInputFileChange.HasDelegate) {
            await OnInputFileChange.InvokeAsync(e);
            return;
        }

        throw new NotImplementedException($"Additional upload support is not yet implemented. You must implement {nameof(OnInputFileChange)} parameter.");
    }
}