using System;

namespace BookShop.Domain.Exceptions;

public abstract class BookShopException(string message, bool isGlobal = false) : Exception(message)
{
    public bool IsGlobal { get; init; } = isGlobal;
};