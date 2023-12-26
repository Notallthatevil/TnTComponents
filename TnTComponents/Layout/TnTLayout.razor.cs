using Microsoft.AspNetCore.Components;

namespace TnTComponents.Layout;

public partial class TnTLayout {

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    private TnTHeader? _header;
    private TnTSideNav? _sideNav;
    private TnTBody? _body;
    private TnTFooter? _footer;

    public void SetHeader(TnTHeader header) {
        if (_header is not null && _header != header) {
            throw new InvalidOperationException("Only one header can be set in a layout!");
        }
        _header = header;
    }

    public void SetSideNav(TnTSideNav sideNav) {
        if (_sideNav is not null && _sideNav != sideNav) {
            throw new InvalidOperationException("Only one side nav can be set in a layout!");
        }
        _sideNav = sideNav;
    }

    public void SetBody(TnTBody body) {
        if (_body is not null && _body != body) {
            throw new InvalidOperationException("Only one body can be set in a layout!");
        }
        _body = body;
    }

    public void SetHeader(TnTFooter footer) {
        if (_footer is not null && _footer != footer) {
            throw new InvalidOperationException("Only one footer can be set in a layout!");
        }
        _footer = footer;
    }
}