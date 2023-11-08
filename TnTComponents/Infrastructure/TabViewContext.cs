﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Infrastructure;
internal class TabViewContext(TnTTabView tabView) {

    internal TnTTabView ParentView { get; set; } = tabView;

    internal List<TnTTabChild> TabChildren { get; set; } = [];

    internal TnTTabChild? ActiveTab {
        get {
            if(ParentView.Disabled) {
                return null;
            }
            else if(_activeTab?.Disabled == true) {
                _activeTab = TabChildren.FirstOrDefault(t => !t.Disabled);
            }
            return _activeTab;
        }    
        set {
            if (_activeTab != value) {
                _activeTab = value;
                ParentView.Refresh();
            }
        }
    }

    internal ElementReference ActiveTabElementRef { 
        get {
            if(_activeTab is not null) {
                return _activeTab.TabReference;
            }
            return default;
        } 
    }

    private TnTTabChild? _activeTab;

    internal void NotifyStateChanged() {
        ParentView.Refresh();
    }

    internal void AddChild(TnTTabChild tabChild) {
        TabChildren.Add(tabChild);
        if (TabChildren.Count == 1) {
            ActiveTab = TabChildren[0];
        }
        ParentView.Refresh();
    }

    internal void RemoveChild(TnTTabChild tabChild) {
        TabChildren.Remove(tabChild);
        if (TabChildren.Count == 0) {
            ActiveTab = null;
        }
        ParentView.Refresh();
    }

    internal void SetActiveTab(TnTTabChild tabChild) {
        if (!tabChild.Disabled && !ParentView.Disabled && _activeTab != tabChild) {
            ActiveTab = tabChild;
        }
    }
}
