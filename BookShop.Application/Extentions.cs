using BookShop.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Application;

public static class Extentions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ICommandHandler<>).Assembly;

        //? This logic automatically scan current assembly and register all DI into DI Container
        //? It works because of Scrutor nugget package added to Application assembly
        services.Scan(s => s.FromAssemblies(applicationAssembly).AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );
        return services;
    }
}