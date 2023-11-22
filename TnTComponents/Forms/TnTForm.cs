using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Enum;

namespace TnTComponents.Forms;

public class TnTForm : EditForm {
    internal const string ParentFormTypeName = "ParentFormType";

    [Parameter]
    public FormType FormType { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<FormType>>(0);
        builder.AddComponentParameter(1, nameof(CascadingValue<FormType>.IsFixed), true);
        builder.AddComponentParameter(2, nameof(CascadingValue<FormType>.Value), FormType);
        builder.AddComponentParameter(3, nameof(CascadingValue<FormType>.Name), ParentFormTypeName);

        builder.AddComponentParameter(4, nameof(CascadingValue<FormType>.ChildContent), new RenderFragment(base.BuildRenderTree));

        builder.CloseComponent();
    }
}

