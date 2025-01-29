using BookShop.Application.Models;
using BookShop.Domain;

namespace BookShop.Infrastructure.Handlers;

internal static class Extensions
{
    public static AuthorModel AsModel(this AuthorEntity entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Surname = entity.Surname
    };
}