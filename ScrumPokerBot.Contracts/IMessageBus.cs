namespace ScrumPokerBot.Contracts
{
    public interface IMessageBus
    {
        void Publish<TMessage>(TMessage message)
            where TMessage : IMessage;

        void Subscribe<TMessage>(IHandle<TMessage> handler)
            where TMessage : IMessage;

    }
}