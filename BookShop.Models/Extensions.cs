using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Models;

public static class Extensions
{
    public static IServiceCollection AddModels(this IServiceCollection services)
    {
        // var modelsAssembly = typeof(ICommandHa)
        // services.Scan();
        return services;
    }
}