namespace ScrumPokerBot.Contracts
{
    public interface ITelegramMessage
    {
        PokerUser User { get; }
        string Message { get; }

    }
}