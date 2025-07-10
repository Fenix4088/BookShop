using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BookShop.Domain.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BookShop.Infrastructure.Services.DomainEventDispatcher;

public class DomainEventDispatcher(IServiceScopeFactory scopeFactory) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        using var scope = scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;

        foreach (var ev in events)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(ev.GetType());
            var handlers = sp.GetServices(handlerType);
            foreach (dynamic handler in handlers)
                await handler.HandleAsync((dynamic)ev, ct);
        }
    }
}