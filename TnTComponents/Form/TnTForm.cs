using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using TnTComponents.Form;

namespace TnTComponents;


public interface ITnTForm {
    FormAppearance Appearance { get; }

    bool Disabled { get; }

    bool ReadOnly { get; }
}

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