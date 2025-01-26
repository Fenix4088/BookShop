using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using BookShop.Domain.Repositories;

namespace BookShop.Infrastructure.Repositories.Abstractions;

public abstract class GenericRepository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class
    where TContext: DbContext
{
    protected readonly TContext context;

    public GenericRepository(TContext shopDbContext)
    {
        context = shopDbContext;
    }

    public async Task AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public Task UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }
}