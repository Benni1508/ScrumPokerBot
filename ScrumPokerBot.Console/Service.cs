using ScrumPokerBot.Telgram;

namespace ScrumPokerBot.Console
{
    internal class Service : IService
    {
        private readonly IBotService botService;

        public Service(IBotService botService)
        {
            this.botService = botService;
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