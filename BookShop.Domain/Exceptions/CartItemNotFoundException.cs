using System;

namespace BookShop.Domain.Exceptions;

public class CartItemNotFoundException(Guid cartItemId) : BookShopException($"Cart item with ID {cartItemId} not found.");