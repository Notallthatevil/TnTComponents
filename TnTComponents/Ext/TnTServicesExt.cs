using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using TnTComponents;
using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Storage;
using TnTComponents.Toast;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for adding TnT services to the service collection.
/// </summary>
[ExcludeFromCodeCoverage]
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