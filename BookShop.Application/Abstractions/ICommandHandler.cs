namespace BookShop.Application.Abstractions
{
    public interface ICommandHandler<TCommand>
    {
        Task Handler(TCommand command);
    }
}