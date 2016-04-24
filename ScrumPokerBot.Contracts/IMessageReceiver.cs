using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IMessageReceiver
    {
        event EventHandler<ITelegramMessage> MessageReceived;
    }

}