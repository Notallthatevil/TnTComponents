using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Common;
public class TnTComponentIdentifierContext : IDisposable {

    private static readonly ThreadLocal<Stack<TnTComponentIdentifierContext>> _threadScopeStack = new(() => new Stack<TnTComponentIdentifierContext>());
    private uint CurrentIndex { get; set; }

    private Func<uint, string> NewId { get; init; }

    public TnTComponentIdentifierContext(Func<uint, string> newId) {
        _threadScopeStack.Value?.Push(this);
        NewId = newId;
        CurrentIndex = 0;
    }

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

    internal string GenerateId() {
        var id = NewId.Invoke(CurrentIndex);

        CurrentIndex++;

        if (CurrentIndex >= 99999999) {
            CurrentIndex = 0;
        }

        return id;
    }

    public void Dispose() {
        _ = _threadScopeStack.Value?.TryPop(out _);
    }
}

