using BookShop.Application.Abstractions;
using BookShop.Application.Decorators;
using BookShop.Application.Models;
using BookShop.Application.Validators;
using BookShop.Domain;
using BookShop.Domain.Entities;
using BookShop.Domain.Entities.Cart;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        
        services.RegisterFluentValidators();
        
        var applicationAssembly = typeof(ICommandHandler<>).Assembly;

        //? This logic automatically scan current assembly and register all DI into DI Container
        //? It works because of Scrutor nugget package added to Application assembly
        services.Scan(s => s.FromAssemblies(applicationAssembly).AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        
        services.Scan(s => s.FromAssemblies(applicationAssembly).AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        
        services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingQueryHandlerDecorator<>));

        return services;
    }


    public static IServiceCollection RegisterFluentValidators(this IServiceCollection services)
    {
        services.AddFluentValidation(fv => fv
            .RegisterValidatorsFromAssemblyContaining<CreateAuthorValidation>().RegisterValidatorsFromAssemblyContaining<UpdateAuthorValidation>()
            .RegisterValidatorsFromAssemblyContaining<CreateBookValidation>()
            .RegisterValidatorsFromAssemblyContaining<UpdateBookValidation>()
        );

        return services;
    }
}


