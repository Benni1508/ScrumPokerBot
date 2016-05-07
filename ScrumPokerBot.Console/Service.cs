using ScrumPokerBot.Domain;
using ScrumPokerBot.Domain.Interfaces;
using ScrumPokerBot.Telgram;

namespace ScrumPokerBot.Console
{
    internal class Service : IService
    {
        private readonly IBotService botService;
        private readonly IScrumPokerService scrumPokerService;

        public Service(IBotService botService, IScrumPokerService scrumPokerService)
        {
            this.botService = botService;
            this.scrumPokerService = scrumPokerService;
        }

        public void Start()
        {
            botService.Start();
        }

        public void Stop()
        {
            botService.Stop();
        }
    }
}