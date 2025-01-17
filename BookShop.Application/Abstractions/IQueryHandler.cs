namespace BookShop.Application.Abstractions
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult> Handler(TQuery query);
    }
}