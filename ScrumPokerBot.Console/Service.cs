using ScrumPokerBot.Domain;
using ScrumPokerBot.Telgram;

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
        scrumPokerService.StartService();
    }

    public void Stop()
    {
        botService.Stop();
    }
}