using System.Collections.Generic;
using BookShop.Application.Models;
using BookShop.Shared.Pagination.Abstractions;

namespace BookShop.Web.Models;

public sealed class CartDetailsViewModel
{
    public CartModel Cart { get; set;  }
    public IPagedResult<CartItemModel> Items { get; set;  }
}