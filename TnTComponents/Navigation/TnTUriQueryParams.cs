using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Core;
using TnTComponents.Ext;

namespace TnTComponents;
public class TnTUriQueryParams : IComponent {
    private RenderHandle _renderHandle;

    [Parameter, EditorRequired]
    public IEnumerable<KeyValuePair<string, object?>> Properties { get; set; } = default!;

    [Inject]
    private NavigationManager _navManager { get; set; } = default!;

    [Parameter]
    public string Uri { get; set; } = default!;

    private TnTDebouncer _debouncer = new();

    public void Attach(RenderHandle renderHandle) => _renderHandle = renderHandle;

    public async Task SetParametersAsync(ParameterView parameters) {
        parameters.SetParameterProperties(this);
        await _debouncer.DebounceAsync(_ => {
            Uri ??= _navManager.Uri;

            _navManager.UpdateUriWithQueryParameters(Uri, new Dictionary<string, object?>(Properties));

            return Task.CompletedTask;
        });
    }
}
