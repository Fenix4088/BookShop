using BookShop.Application.Models;
using BookShop.Shared.Abstractions;

namespace BookShop.Application.Queries;

public record GetCartQuery(Guid UserId) : IQuery<CartModel>;