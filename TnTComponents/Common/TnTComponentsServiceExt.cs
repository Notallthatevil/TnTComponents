using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Services;

namespace Microsoft.Extensions.DependencyInjection;
public static class TnTComponentsServiceExt {

    public static IServiceCollection AddTnTComponentsServices(this IServiceCollection serviceCollection) {
        return serviceCollection.AddScoped<TnTDialogService>();
    }

}
