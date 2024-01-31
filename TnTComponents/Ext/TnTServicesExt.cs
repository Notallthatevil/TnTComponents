using TnTComponents.Dialog;
using TnTComponents.Snackbar;

namespace Microsoft.Extensions.DependencyInjection;

public static class TnTServicesExt {

    public static IServiceCollection AddTnTServices(this IServiceCollection services) {
        return services.AddScoped<TnTDialogService>()
             .AddScoped<TnTSnackbarService>();
    }
}