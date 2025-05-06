using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using TnTComponents;
using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Toast;
using TnTComponents.Storage;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Extensions for adding TnT services to the service collection.
/// </summary>
public static class TnTServicesExt {

    /// <summary>
    /// Adds TnT services to the service collection for a Blazor WebAssembly application.
    /// </summary>
    public static WebAssemblyHostBuilder AddTnTServices(this WebAssemblyHostBuilder builder) {
        builder.Services.AddTnTServices()
            .AddTnTClientServices();
        return builder;
    }

    /// <summary>
    /// Adds TnT services to the service collection for a server application.
    /// </summary>
    public static IHostApplicationBuilder AddTnTServices(this IHostApplicationBuilder builder) {
        builder.Services.AddTnTServices()
            .AddTnTServerServices();
        return builder;
    }

    private static IServiceCollection AddTnTClientServices(this IServiceCollection services) {
        return services;
    }

    private static IServiceCollection AddTnTServerServices(this IServiceCollection services) {
        return services;
    }

    /// <summary>
    /// Adds TnT services to the service collection.
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