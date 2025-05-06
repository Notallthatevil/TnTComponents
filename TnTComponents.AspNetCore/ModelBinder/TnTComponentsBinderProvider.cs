using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Virtualization;

namespace TnTComponents.AspNetCore.ModelBinder;

/// <summary>
///     Binder for <see cref="TnTItemsProviderRequest" />. This binder is used to bind the query parameters of the request to a <see cref="TnTItemsProviderRequest" /> instance.
/// </summary>
public class TnTComponentsBinderProvider : IModelBinderProvider {

    /// <inheritdoc />
    public IModelBinder? GetBinder(ModelBinderProviderContext context) {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        return context.Metadata.ModelType == typeof(TnTItemsProviderRequest)
            ? new BinderTypeModelBinder(typeof(TnTItemsProviderRequestBinder))
            : (IModelBinder?)null;
    }
}