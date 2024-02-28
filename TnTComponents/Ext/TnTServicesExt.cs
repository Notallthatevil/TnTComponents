using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Snackbar;
using TnTComponents.Storage;

namespace Microsoft.Extensions.DependencyInjection;

public static class TnTServicesExt {

    public static IServiceCollection AddTnTServices(this IServiceCollection services) {
        return services.AddScoped<TnTDialogService>()
             .AddScoped<TnTSnackbarService>()
             .AddScoped<ISessionStorageService, SessionStorageService>()
             .AddScoped<ILocalStorageService, LocalStorageService>();
    }

    public static IServiceCollection AddTnTServerServices(this IServiceCollection services) {
        return services.AddSingleton<ITnTRenderContext, TnTServerRenderContext>();
    }

    public static IServiceCollection AddTnTClientServices(this IServiceCollection services) {
        return services.AddSingleton<ITnTRenderContext, TnTClientRenderContext>();
    }
}