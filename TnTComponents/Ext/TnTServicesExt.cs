using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using TnTComponents;
using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.RenderContext;
using TnTComponents.Toast;
using TnTComponents.Storage;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class TnTServicesExt {

    public static WebAssemblyHostBuilder AddTnTServices(this WebAssemblyHostBuilder builder) {
        builder.Services.AddTnTServices()
            .AddTnTClientServices();
        return builder;
    }

    public static IHostApplicationBuilder AddTnTServices(this IHostApplicationBuilder builder) {
        builder.Services.AddTnTServices()
            .AddTnTServerServices();
        return builder;
    }

    private static IServiceCollection AddTnTClientServices(this IServiceCollection services) {
        return services.AddSingleton<ITnTRenderContext, TnTClientRenderContext>();
    }

    private static IServiceCollection AddTnTServerServices(this IServiceCollection services) {
        return services.AddSingleton<ITnTRenderContext, TnTServerRenderContext>();
    }

    private static IServiceCollection AddTnTServices(this IServiceCollection services) {
        return services.AddScoped<ITnTDialogService, TnTDialogService>()
             .AddScoped<ITnTToastService, TnTToastService>()
             .AddScoped<ISessionStorageService, SessionStorageService>()
             .AddScoped<ILocalStorageService, LocalStorageService>();
    }
}