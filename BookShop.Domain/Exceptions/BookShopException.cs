using System;

public abstract class BookShopException: Exception
{
    protected BookShopException(string message) : base(message)
    {
    }

}