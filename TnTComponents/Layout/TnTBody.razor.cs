using Microsoft.AspNetCore.Components;
using TnTComponents.Infrastructure;

namespace TnTComponents.Layout;
public partial class TnTBody
{
    [Parameter]
    public override string BaseCssClass { get; set; } = "tnt-body";

    protected override void AddSelfToContext() {
        Context.Body = this;
    }
}
