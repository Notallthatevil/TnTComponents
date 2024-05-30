using Microsoft.Extensions.DependencyInjection;

namespace TnTComponents.Core;

/// <inheritdoc />
public sealed class TnTServerRenderContext : ITnTRenderContext {

    /// <inheritdoc />
    public bool IsClient => false;

    /// <inheritdoc />
    public bool IsPrerendering => !(_contextAccessor.HttpContext?.Response.HasStarted) ?? false;

    /// <inheritdoc />
    public bool IsServer => true;

    private dynamic _contextAccessor;

    public TnTServerRenderContext(IServiceProvider serviceProvider) {
        const string HttpAccessor = "Microsoft.AspNetCore.Http.IHttpContextAccessor";
        var type = Type.GetType("Microsoft.AspNetCore.Http.IHttpContextAccessor") ?? 
            throw new InvalidOperationException($"Failed to locate type '{HttpAccessor}' in services. Did you forget to call builder.Services.AddHttpContextAccessor()?");
        _contextAccessor = serviceProvider.GetRequiredService(type);
    }
}