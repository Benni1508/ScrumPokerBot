﻿using System.IO;
using Ninject.Modules;
using Telegram.Bot;

namespace ScrumPokerBot.Telgram
{
    public class TelegramNinjectModule : NinjectModule
    {
        public override void Load()
        {
            var apikey = File.ReadAllText("C:\\Temp\\ApiKey.txt");
            Bind<Api>().ToConstant(new Api(apikey)).InSingletonScope();
            Bind<IBotService>().To<BotService>().InSingletonScope();
            Bind<IMessageFactory>().To<MessageFactory>();
        }
    }
}