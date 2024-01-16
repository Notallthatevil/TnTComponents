using Microsoft.AspNetCore.Components;
using TnTComponents.Forms;

namespace TnTComponents;
public partial class TnTLabel : IFormItem {
    [Parameter]
    public IFormField? For { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [CascadingParameter(Name = nameof(ParentFormDisabled))]
    public bool ParentFormDisabled { get; }
    [CascadingParameter(Name = nameof(ParentFormReadOnly))]
    public bool ParentFormReadOnly { get; }
    [CascadingParameter(Name = nameof(ParentFormAppearance))]
    public FormAppearance ParentFormAppearance { get; }
    [Parameter]
    public bool Disabled { get; set; }
    [Parameter]
    public bool ReadOnly { get; set; }
    [Parameter]
    public FormAppearance Appearance { get; set; }
    [CascadingParameter]
    public TnTForm? ParentForm { get; set; }



}
