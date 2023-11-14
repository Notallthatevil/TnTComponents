using Microsoft.AspNetCore.Components;
using TnTComponents.Infrastructure;

namespace TnTComponents.Layout;
public partial class TnTLayout {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;


    //[Parameter]
    //public RenderFragment? TnTHeader { get; set; }
    //
    //[Parameter]
    //public RenderFragment? TnTSideNav { get; set; }

    private readonly TnTLayoutContext _context;

    private string _tntHeaderId = Guid.NewGuid().ToString();
    private string _tntSideNavId = Guid.NewGuid().ToString();

    public TnTLayout() {
        _context = new TnTLayoutContext(this);
    }
}
