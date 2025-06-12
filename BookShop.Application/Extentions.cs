using BookShop.Application.Abstractions;
using BookShop.Application.Models;
using BookShop.Application.Validators;
using BookShop.Domain;
using BookShop.Domain.Entities;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Application;

public static class Extentions
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
    
    
    public static BookModel ToModel(this BookEntity bookEntity)
    {
        return new BookModel()
        {
            Id = bookEntity.Id,
            Title = bookEntity.Title,
            Description = bookEntity.Description,
            ReleaseDate = bookEntity.ReleaseDate,
            AuthorId = bookEntity.AuthorId,
            Author = bookEntity.Author.ToModel(),
            CoverImgUrl = bookEntity.CoverImgUrl,
            IsDeleted = bookEntity.IsDeleted,
            AverageRating = (int)Math.Round(bookEntity.Ratings.Count > 0 ? bookEntity.Ratings.Average(x => x.Score) : 0, 0),
        };
    }
    
    
    public static AuthorModel ToModel(this AuthorEntity authorEntity)
    {
        return new AuthorModel()
        {
            Id = authorEntity.Id,
            Name = authorEntity.Name,
            Surname = authorEntity.Surname,
            BookCount = authorEntity.BookCount,
            IsDeleted = authorEntity.IsDeleted,
            AverageRating = (int)Math.Round(authorEntity.Ratings.Count > 0 ? authorEntity.Ratings.Average(x => x.Score) : 0, 0),
        };
    }
    
}