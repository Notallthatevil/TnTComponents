namespace TnTComponents.RenderContext;

/// <inheritdoc />
public sealed class TnTClientRenderContext : ITnTRenderContext {

    /// <inheritdoc />
    public bool IsClient => true;

    /// <inheritdoc />
    public bool IsPrerendering => false;

    /// <inheritdoc />
    public bool IsServer => false;
}