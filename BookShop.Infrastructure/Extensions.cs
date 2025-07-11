using Azure.Monitor.OpenTelemetry.AspNetCore;
using BookShop.Application;
using BookShop.Application.Abstractions;
using BookShop.Application.EventHandlers;
using BookShop.Application.Services;
using BookShop.Application.Users;
using BookShop.Domain.Abstractions;
using BookShop.Domain.Entities.Rating;
using BookShop.Domain.Events;
using BookShop.Domain.Repositories;
using BookShop.Infrastructure.Abstractions;
using BookShop.Infrastructure.Context;
using BookShop.Infrastructure.Decorators;
using BookShop.Infrastructure.Identity;
using BookShop.Infrastructure.Middlewares;
using BookShop.Infrastructure.Repositories;
using BookShop.Infrastructure.Repositories.Abstractions;
using BookShop.Infrastructure.Services.Background;
using BookShop.Infrastructure.Services.Cart;
using BookShop.Infrastructure.Services.Domain;
using BookShop.Infrastructure.Services.DomainEventDispatcher;
using BookShop.Infrastructure.Services.Email;
using BookShop.Infrastructure.Services.PolicyRole;
using BookShop.Infrastructure.Services.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BookShop.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var isProduction = environment.IsProduction();

        if (isProduction)
        {
            services.AddTelemetry(configuration);
        }
        
        services.Configure<EmailSettings>(configuration.GetSection("Smtp"));
        
        services.AddDbContext<ShopDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("BookShop")));

        services
            .AddScoped<IDataSeeder, DataSeeder>()
            .AddHostedService<DatabaseInitializer>();

        services
            .AddDatabaseDeveloperPageExceptionFilter()
            .AddTransient<ExceptionsMiddleware>()
            .AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddRepositories()
            .AddApplicationServices()
            .AddDomainServices()
            .AddDomainEvents();
        

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
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Authors}/{action=AuthorList}/{id?}"
        );
        return app;
    }

    private static IServiceCollection AddTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        const string serviceName = "BookShop.Api";
        
        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName))
            .WithTracing(t =>
                t.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter()
            )
            .WithMetrics(m =>
                m.AddAspNetCoreInstrumentation()
                    .AddConsoleExporter()
            )
            .WithLogging(l =>
                l.AddConsoleExporter());

        services.AddOpenTelemetry()
            .UseAzureMonitor(opt =>
            {
                opt.ConnectionString = configuration.GetConnectionString("AzureMonitor");
                opt.SamplingRatio = 0.5F;
            });

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IAuthorRepository, AuthorRepository>()
            .AddScoped<IBookRepository, BookRepository>()
            .AddScoped<IRatingRepository<BookRatingEntity>, BookRatingRepository>()
            .AddScoped<IRatingRepository<AuthorRatingEntity>, AuthorRatingRepository>()
            .AddScoped<ICartRepository, CartRepository>();
        
        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services
            .AddScoped<IEmailSender, EmailSender>()
            .AddScoped<IPolicyRoleService, PolicyRoleService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IPolicyRoleService, PolicyRoleService>()
            .AddScoped<ICartService, CartService>();
        
        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services
            .AddScoped<IAuthorDomainService, AuthorDomainService>()
            .AddScoped<IBookDomainService, BookDomainService>();
        
        return services;
    }
    
    private static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        services
            .AddScoped<IDomainEventHandler<BookDeleteEvent>, BookDeleteEventHandler>();
        
        return services;
    }
    
}