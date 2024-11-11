using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TnTComponents.AspNetCore.ModelBinder;
public class TnTComponentsBinderProvider : IModelBinderProvider {
    public IModelBinder? GetBinder(ModelBinderProviderContext context) {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        return context.Metadata.ModelType == typeof(ITnTItemsProviderRequest)
            ? new BinderTypeModelBinder(typeof(TnTItemsProviderRequestBinder))
            : (IModelBinder?)null;
    }
}
