using System;

namespace BookShop.Domain.Exceptions;

public class CartNotFoundException(Guid cartId) : BookShopException($"Cart with ID '{cartId}' not found.");