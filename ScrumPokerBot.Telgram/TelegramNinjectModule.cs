using System.Configuration;
using System.IO;
using Ninject.Modules;
using Telegram.Bot;

namespace ScrumPokerBot.Telgram
{
    public class TelegramNinjectModule : NinjectModule
    {
        public override void Load()
        {
            var keyString = ConfigurationManager.AppSettings["ApiKeyFile"];
            var apikey = File.ReadAllText(keyString);
            Bind<Api>().ToConstant(new Api(apikey)).InSingletonScope();
            Bind<IBotService>().To<BotService>().InSingletonScope();
            Bind<IMessageFactory>().To<MessageFactory>();
        }
    }
}