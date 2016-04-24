using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using ScrumPokerBot.Domain;
using Telegram.Bot;

namespace ScrumPokerBot.Telgram
{
    public class TelegramNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<Api>().ToConstant(new Api("API Key")).InSingletonScope();
            this.Bind<IBotService, IMessageReceiver>().To<BotService>().InSingletonScope();
            this.Bind<IMessageFactory>().To<MessageFactory>();
        }
    }
}
