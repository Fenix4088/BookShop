using System;
using BookShop.Application.Commands.Handlers;
using BookShop.Application.Queries.Handlers;
using BookShop.Application.Validators;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Identity;
using BookShop.Infrastructure.Repositories;
using BookShop.Infrastructure.Services.Domain;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
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
        services.AddLogging();
        services.AddIdentity<BookShopUser, BookShopRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ShopDbContext>()
            .AddDefaultTokenProviders();
        
        
        // Repositories
        services
            .AddTransient<IDataSeeder, DataSeeder>()
            .AddTransient<IAuthorRepository, AuthorRepository>()
            .AddTransient<IBookRepository, BookRepository>()
            .AddTransient<IRatingRepository<AuthorRatingEntity>, AuthorRatingRepository>()
            .AddTransient<IRatingRepository<BookRatingEntity>, BookRatingRepository>()
            .AddTransient<IAuthorDomainService, AuthorDomainService>()
            .AddTransient<IBookDomainService, BookDomainService>();

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
            .AddTransient<GetBookListQueryHandler>()
            .AddTransient<RateAuthorCommandHandler>()
            .AddTransient<RateBookCommandHandler>();

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