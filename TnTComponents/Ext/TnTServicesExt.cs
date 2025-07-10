using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using TnTComponents;
using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Toast;
using TnTComponents.Storage;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for adding TnT services to the service collection.
/// </summary>
public static class TnTServicesExt {

    /// <summary>
    ///     Adds TnT services to the service collection.
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>The IServiceCollection instance</returns>
    public static IServiceCollection AddTnTServices(this IServiceCollection services) {
        return services.AddScoped<ITnTDialogService, TnTDialogService>()
             .AddScoped<ITnTToastService, TnTToastService>()
             .AddScoped<ISessionStorageService, SessionStorageService>()
             .AddScoped<ILocalStorageService, LocalStorageService>();
    }
}