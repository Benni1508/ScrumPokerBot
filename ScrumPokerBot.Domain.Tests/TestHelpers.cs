using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain.Tests
{
    public static class TestHelpers
    {
        public static PokerUser GetTestUser(long chatId)
        {
            return new PokerUser {ChatId = chatId, Firstname = "Test", Lastname = "User", Username = $"Name{chatId}"};
        }
    }
}