using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Infrastructure;
internal class TabViewContext(TnTTabView parentView) {

    public IReadOnlyDictionary<string, TnTTabChild> TabChildren => _tabChildren;

    public readonly TnTTabView ParentView = parentView;

    private readonly Dictionary<string, TnTTabChild> _tabChildren = [];

    public void AddChild(TnTTabChild child) {
        _tabChildren[child.Title] = child;
    }

    public void RemoveChild(TnTTabChild child) {
        _tabChildren.Remove(child.Title);
    }

}

