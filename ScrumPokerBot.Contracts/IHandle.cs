namespace ScrumPokerBot.Contracts
{
    public interface IHandle<T>
    {
        void Handle(T message);
    }
}