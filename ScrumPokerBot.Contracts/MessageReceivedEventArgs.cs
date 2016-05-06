using System;
using ScrumPokerBot.Contracts.Messages;

namespace ScrumPokerBot.Contracts
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(ITelegramMessage telegramMessage)
        {
            TelegramMessage = telegramMessage;
        }

        public ITelegramMessage TelegramMessage { get; }
    }
}