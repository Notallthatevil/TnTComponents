using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Snackbar;
using TnTComponents.Storage;

namespace Microsoft.Extensions.DependencyInjection;

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
        return services.AddScoped<TnTDialogService>()
             .AddScoped<TnTSnackbarService>()
             .AddScoped<ISessionStorageService, SessionStorageService>()
             .AddScoped<ILocalStorageService, LocalStorageService>();
    }
}