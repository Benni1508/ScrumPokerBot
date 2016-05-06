using System;
using ScrumPokerBot.Contracts;

namespace ScrumPokerBot.Domain
{
    public interface IMessageReceiver
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived; 

    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(ITelegramMessage telegramMessage)
        {
            TelegramMessage = telegramMessage;
        }

        public ITelegramMessage TelegramMessage { get; }
    }

}