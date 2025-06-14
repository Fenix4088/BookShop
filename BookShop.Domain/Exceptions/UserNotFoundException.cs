using System;

namespace BookShop.Domain.Exceptions;

public class UserNotFoundException : BookShopException
{
    public UserNotFoundException(Guid userId) : base($"User with ID '{userId}' not found.")
    {
    }
}