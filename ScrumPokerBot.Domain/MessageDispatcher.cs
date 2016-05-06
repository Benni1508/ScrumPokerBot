using ScrumPokerBot.Contracts;
using ScrumPokerBot.Contracts.Messages;

namespace ScrumPokerBot.Domain
{
    public class MessageDispatcher
    {
        private readonly IScrumPokerService pokerService;

        public MessageDispatcher(IScrumPokerService pokerService)
        {
            this.pokerService = pokerService;
        }

    }
}