namespace BookShop.Domain.Exceptions;

public class RateException : BookShopException
{
    public RateException(string message) : base(message)
    {
    }
}