using System;
using BookShop.Application.Commands.Handlers;
using BookShop.Application.Validators;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Handlers;
using BookShop.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.UnitTests.Extensions;

public static class DIExtensions
{
    public static IServiceCollection AddBookShopTestDeps(this IServiceCollection services)
    {
        
        services.AddDbContext<ShopDbContext>((options) =>
        {
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            options.EnableSensitiveDataLogging();
        });
        
        // Repositories
        services.AddTransient<IAuthorRepository, AuthorRepository>()
            .AddTransient<IBookRepository, BookRepository>();

        //Handlers
        services.AddTransient<CreateAuthorCommandHandler>()
            .AddTransient<GetAuthorListQueryHandler>()
            .AddTransient<UpdateAuthorCommandHandler>()
            .AddTransient<GetAuthorQueryHandler>()
            .AddTransient<SoftDeleteAuthorCommandHandler>()
            .AddTransient<GetBookQueryHandler>()
            .AddTransient<CreateBookCommandHandler>()
            .AddTransient<UpdateBookCommandHandler>()
            .AddTransient<SoftDeleteBookCommandHandler>()
            .AddTransient<GetBookListQueryHandler>();

        //Validators
        services.AddFluentValidation(fv => fv
            .RegisterValidatorsFromAssemblyContaining<CreateAuthorValidation>()
            .RegisterValidatorsFromAssemblyContaining<UpdateAuthorValidation>()
            .RegisterValidatorsFromAssemblyContaining<CreateBookValidation>()
            .RegisterValidatorsFromAssemblyContaining<UpdateBookValidation>()
        );

        return services;
    }
}