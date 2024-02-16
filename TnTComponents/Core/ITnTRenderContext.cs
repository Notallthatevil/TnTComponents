using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;
/// <summary>
/// Provide the render mode information in which the component is rendering.
/// </summary>
public interface ITnTRenderContext {
    /// <summary>
    /// Rendering from the Client project. Using HTTP request for connectivity.
    /// </summary>
    public bool IsClient { get; }

    /// <summary>
    /// Rendering from the Server project. Using WebSockets for connectivity.
    /// </summary>
    public bool IsServer { get; }

    /// <summary>
    /// Rendering from the Server project. Indicates if the response has started rendering.
    /// </summary>
    public bool IsPrerendering { get; }
}
