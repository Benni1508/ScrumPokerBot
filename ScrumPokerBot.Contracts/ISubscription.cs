namespace ScrumPokerBot.Contracts
{
    public interface ISubscription<TMessage>
        where TMessage : IMessage
    {
        IHandle<TMessage> Action { get; }
    }
}