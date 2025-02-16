using BookShop.Application.Abstractions;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Decorators;
using BookShop.Infrastructure.Handlers;
using BookShop.Infrastructure.Middlewares;
using BookShop.Infrastructure.Repositories;
using BookShop.Infrastructure.Services.Background;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookShop.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ShopDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("BookShop")))
            .AddDatabaseDeveloperPageExceptionFilter()
            .AddTransient<ExceptionsMiddleware>()
            .AddScoped<IAuthorRepository, AuthorRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
        
        var infrastructureAssembly = typeof(GetAuthorListQueryHandler).Assembly;
        services.Scan(s => s.FromAssemblies(infrastructureAssembly)
            .AddClasses(c => c.AssignableTo(typeof (IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        

        services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));

        services.AddHostedService<ArchiveEntitiesCleanupService>();
        
        return services;
    }

    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseMiddleware<ExceptionsMiddleware>();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Authors}/{action=AuthorList}/{id?}"
            );
        return app;
    }
}