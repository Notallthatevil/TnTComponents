using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnTComponents.Dialogs;

namespace Microsoft.Extensions.DependencyInjection;
public static class TnTServicesExt {
    public static IServiceCollection AddTnTServices(this IServiceCollection services) {
       return services.AddScoped<TnTDialogService>();
    }
}

