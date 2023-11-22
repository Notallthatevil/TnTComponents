using Microsoft.AspNetCore.Components;

namespace TnTComponents.Forms;

public class TnTSwitch : TnTCheckbox {

    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-form-field-switch";

    protected override void OnInitialized() {
        base.OnInitialized();
    }
}