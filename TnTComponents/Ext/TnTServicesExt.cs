using TnTComponents.Core;
using TnTComponents.Dialog;
using TnTComponents.Snackbar;

namespace Microsoft.Extensions.DependencyInjection;

public static class TnTServicesExt {

    public static IServiceCollection AddTnTServices(this IServiceCollection services) {
        return services.AddScoped<TnTDialogService>()
             .AddScoped<TnTSnackbarService>();
    }

    public static IServiceCollection AddTnTServerServices(this IServiceCollection services) {
        return services.AddSingleton<ITnTRenderContext, TnTServerRenderContext>();
    }

    public static IServiceCollection AddTnTClientServices(this IServiceCollection services) {
        return services.AddSingleton<ITnTRenderContext, TnTClientRenderContext>();
    }
}