using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Infrastructure;
internal class TabViewContext(TnTTabView parentView) {

    public IReadOnlyDictionary<string, TnTTabChild> TabChildren => _tabChildren;

    public readonly TnTTabView ParentView = parentView;

    private Dictionary<string, TnTTabChild> _tabChildren = [];

    public void AddChild(TnTTabChild child) {
        if (_tabChildren.TryGetValue(child.Title, out var _)) {
            throw new InvalidOperationException($"Attempted to add a {nameof(TnTTabChild)} with {child.Title} when the {nameof(TnTTabView)} already contains a {nameof(TnTTabChild)} with that {nameof(TnTTabChild.Title)}");
        }

        _tabChildren.Add(child.Title, child);
    }

    public void RemoveChild(TnTTabChild child) {
        _tabChildren.Remove(child.Title);
    }

}

