using TnTComponents.Dialogs;

namespace Microsoft.Extensions.DependencyInjection;

public static class TnTComponentsServiceExt {

    public static IServiceCollection AddTnTComponentsServices(this IServiceCollection serviceCollection) {
        return serviceCollection.AddScoped<TnTDialogService>();
    }
}