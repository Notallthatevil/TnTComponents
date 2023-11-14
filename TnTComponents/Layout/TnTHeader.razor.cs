using Microsoft.AspNetCore.Components;
using TnTComponents.Infrastructure;

namespace TnTComponents.Layout;
public  partial class TnTHeader  {
    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-header";

    protected override void AddSelfToContext() {
        Context.Header = this;
    }

    protected override void OnInitialized() {
        base.OnInitialized();

    }
}
