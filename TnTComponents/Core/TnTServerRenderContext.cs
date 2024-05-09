using Microsoft.AspNetCore.Http;

namespace TnTComponents.Core;

/// <inheritdoc />
public sealed class TnTServerRenderContext(IHttpContextAccessor contextAccessor) : ITnTRenderContext {

    /// <inheritdoc />
    public bool IsClient => false;

    /// <inheritdoc />
    public bool IsPrerendering => !contextAccessor.HttpContext?.Response.HasStarted ?? false;

    /// <inheritdoc />
    public bool IsServer => true;
}