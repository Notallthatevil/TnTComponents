using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Core;

public static class TnTComponentIdentifier {
    private const string _identifierPrefix = "tnt_";
    private static readonly Random _rnd = new();

    /// <summary>
    /// Returns a new small Id. HTML id must start with a letter.
    /// Example: f127d9edf14385adb
    /// </summary>
    /// <remarks>
    /// You can use a <see cref="IdentifierContext" /> instance to customize the Generation process,
    /// for example in Unit Tests.
    /// </remarks>
    /// <returns>A new id</returns>
    public static string NewId(int length = 8) {
        if (TnTComponentIdentifierContext.Current == null) {
            if (length > 16) {
                throw new ArgumentOutOfRangeException(nameof(length), "length must be less than 16");
            }
            if (length <= 8) {
                return $"{_identifierPrefix}{_rnd.Next():x}";
            }

            return $"{_identifierPrefix}{_rnd.Next():x}{_rnd.Next():x}"[..length];
        }

        return TnTComponentIdentifierContext.Current.GenerateId();
    }

    /// <summary>
    /// Returns a new <see cref="TnTComponentIdentifierContext" /> where ID are sequential:
    /// "tnt_00000000", "tnt_00000001", "tnt_00000002", ...
    /// </summary>
    /// <returns></returns>
    public static TnTComponentIdentifierContext SequentialContext() => new((n) => $"{_identifierPrefix}{n:00000000}");
}

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