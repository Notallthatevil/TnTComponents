using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.Tests.Ext;
public static class BunitJSInteropExt {

    public static void SetupDefaultModule(this BunitJSInterop jsInterop, string path) {
        var module = jsInterop.SetupModule(path);
        module.SetupVoid("onLoad", _ => true);
        module.SetupVoid("onUpdate", _ => true);
        module.SetupVoid("onDispose", _ => true);
    }
}
