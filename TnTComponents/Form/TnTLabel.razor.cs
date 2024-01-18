using Microsoft.AspNetCore.Components;
using TnTComponents.Form;

namespace TnTComponents;
public partial class TnTLabel : IFormItem {
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
    [Parameter]
    public TnTForm? ParentForm { get; set; }
}
