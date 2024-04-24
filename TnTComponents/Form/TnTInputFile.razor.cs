using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Form;

namespace TnTComponents;
public partial class TnTInputFile {

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }

    public string FormCssClass => CssClassBuilder.Create()
        .AddClass("tnt-input")
        .AddOutlined((ParentFormAppearance ?? Appearance) == FormAppearance.Outlined)
        .AddFilled((ParentFormAppearance ?? Appearance) == FormAppearance.Filled)
        .Build();

    [Parameter]
    public TnTColor ButtonBackgroundColor { get; set; } = TnTColor.Primary;

    [Parameter]
    public TnTColor ButtonTextColor { get; set; } = TnTColor.OnPrimary;

    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }
    [CascadingParameter(Name = nameof(IFormItem.ParentFormAppearance))]
    public FormAppearance? ParentFormAppearance { get; set; }
    [CascadingParameter(Name = nameof(IFormItem.ParentFormDisabled))]
    public bool? ParentFormDisabled { get; set; }
    [CascadingParameter(Name = nameof(IFormItem.ParentFormReadOnly))]
    public bool? ParentFormReadOnly { get; set; }

    [Parameter]
    public FormAppearance Appearance { get; set; }
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public bool Readonly { get; set; }

    [Parameter]
    public int MaximumFileCount { get; set; } = 1;
    [Parameter]
    public EventCallback<InputFileChangeEventArgs> OnInputFileChange { get; set; }

    protected override void OnParametersSet() {
        base.OnParametersSet();

        if (ParentFormDisabled == true || Disabled) {
            AdditionalAttributes?.TryAdd("disabled", true);
        }

        if (ParentFormReadOnly == true || Readonly) {
            AdditionalAttributes?.TryAdd("readonly", true);
        }
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

