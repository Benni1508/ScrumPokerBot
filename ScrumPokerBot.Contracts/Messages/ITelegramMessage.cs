namespace ScrumPokerBot.Contracts.Messages
{
    public interface ITelegramMessage : IMessage
    {
        PokerUser User { get; }
        string Message { get; }

    }
}