namespace ScrumPokerBot.Contracts
{
    public interface ITelegramMessage
    {
        long ChatId { get; }
        PokerUser User { get; }
        string Message { get; }
    }
}