using Microsoft.AspNetCore.Components;

namespace TnTComponents.Infrastructure;
public interface ITnTGridColumn
{
    string Class { get; }
    RenderFragment HeaderContent { get; }

}

