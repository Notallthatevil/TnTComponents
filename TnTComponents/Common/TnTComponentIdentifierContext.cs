namespace TnTComponents.Common;

public class TnTComponentIdentifierContext : IDisposable {

    public static TnTComponentIdentifierContext? Current {
        get {
            if (_threadScopeStack.Value == null || _threadScopeStack.Value.Count == 0) {
                return null;
            }
            else {
                return _threadScopeStack.Value?.Peek();
            }
        }
    }

    private uint CurrentIndex { get; set; }
    private Func<uint, string> NewId { get; init; }
    private static readonly ThreadLocal<Stack<TnTComponentIdentifierContext>> _threadScopeStack = new(() => new Stack<TnTComponentIdentifierContext>());

    public TnTComponentIdentifierContext(Func<uint, string> newId) {
        _threadScopeStack.Value?.Push(this);
        NewId = newId;
        CurrentIndex = 0;
    }

    public void Dispose() {
        _ = _threadScopeStack.Value?.TryPop(out _);
    }

    internal string GenerateId() {
        var id = NewId.Invoke(CurrentIndex);

        CurrentIndex++;

        if (CurrentIndex >= 99999999) {
            CurrentIndex = 0;
        }

        return id;
    }
}