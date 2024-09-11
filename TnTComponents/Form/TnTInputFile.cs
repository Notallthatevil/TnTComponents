using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Core;
using TnTComponents.Form;
using TnTComponents.Interfaces;

namespace TnTComponents;

public class TnTInputFile : InputFile, ITnTInteractable {

    [Parameter]
    public FormAppearance Appearance { get; set; }

    [Parameter]
    public bool? AutoFocus { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string? ElementName { get; set; }
    [Parameter]
    public bool EnableRipple { get; set; }
    [Parameter]
    public TnTColor? TintColor { get; set; }
    [Parameter]
    public TnTColor? OnTintColor { get; set; }

    public string? ElementClass => CssClassBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes?.ToDictionary())
        .AddClass("tnt-input-file")
        .AddClass("tnt-input")
        .AddTnTInteractable(this)
        .AddFilled(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Filled : Appearance == FormAppearance.Filled)
        .AddOutlined(_tntForm?.Appearance is not null ? _tntForm.Appearance == FormAppearance.Outlined : Appearance == FormAppearance.Outlined)
        .Build();

    [Parameter]
    public string? ElementId { get; set; }

    [Parameter]
    public string? ElementLang { get; set; }

    public string? ElementStyle => CssStyleBuilder.Create()
        .AddFromAdditionalAttributes(AdditionalAttributes?.ToDictionary())
        .Build();

    [Parameter]
    public string? ElementTitle { get; set; }

    public bool FieldDisabled => _tntForm?.Disabled is not null ? _tntForm.Disabled : Disabled;

    public bool FieldReadonly => _tntForm?.ReadOnly is not null ? _tntForm.ReadOnly : ReadOnly;

    [Parameter]
    public int MaximumFileCount { get; set; } = 1;

    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnInputFileChange { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [CascadingParameter]
    private ITnTForm? _tntForm { get; set; }

    protected override void OnInitialized() {
        base.OnInitialized();
        OnChange = EventCallback.Factory.Create(this, async (InputFileChangeEventArgs args) => await OnUploadFilesHandlerAsync(args));
    }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        var dict = AdditionalAttributes?.ToDictionary() ?? new Dictionary<string, object>();
        dict.TryAdd("disabled", FieldDisabled);
        dict.TryAdd("readonly", FieldReadonly);
        AdditionalAttributes = dict;
    }

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