using BookShop.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace BookShop.Application.Decorators;

public class LoggingQueryHandlerDecorator<TQuery, TResult>: IQueryHandler<TQuery, TResult>
{

    private readonly IQueryHandler<TQuery, TResult> queryHandler;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger;
    
    public LoggingQueryHandlerDecorator(
        IQueryHandler<TQuery, TResult> queryHandler,
        ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    {
        this.queryHandler = queryHandler;
        this.logger = logger;
    }
    
    public async Task<TResult> Handler(TQuery query)
    {
        try
        {
            return await queryHandler.Handler(query);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in handler {Handler} for query {@Query}",
                queryHandler.GetType().Name, query);

            throw;
        }
    }
}