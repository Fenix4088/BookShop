using BookShop.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace BookShop.Application.Decorators;

public class LoggingQueryHandlerDecorator<TCommand>: ICommandHandler<TCommand> where TCommand : class, ICommand
{

    private readonly ICommandHandler<TCommand> commandHandler;
    private readonly ILogger<LoggingQueryHandlerDecorator<TCommand>> logger;
    
    public LoggingQueryHandlerDecorator(
        ICommandHandler<TCommand> commandHandler,
        ILogger<LoggingQueryHandlerDecorator<TCommand>> logger)
    {
        this.commandHandler = commandHandler;
        this.logger = logger;
    }
    
    public async Task Handler(TCommand command)
    {
        try
        {
            await commandHandler.Handler(command);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in handler {Handler} for query {@Query}",
                commandHandler.GetType().Name, command);

            throw;
        }
    }
}