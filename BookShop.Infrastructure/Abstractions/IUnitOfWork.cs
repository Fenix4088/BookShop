using System;
using System.Threading.Tasks;

namespace BookShop.Infrastructure.Abstractions;

internal interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
}