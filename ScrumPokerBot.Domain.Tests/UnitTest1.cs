using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using ScrumPokerBot.Contracts;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class UnitTest1
    {
        private ScrumPokerService service;
        private IMessageSender messageSender;

        public UnitTest1()
        {
            messageSender = Substitute.For<IMessageSender>();
            var idGenerator = Substitute.For<IIdGenerator>();
            var messageReceiver = Substitute.For<IMessageReceiver>();
            idGenerator.GetId().Returns(12);
            this.service = new ScrumPokerService(messageSender, idGenerator,messageReceiver );
        }

        [Fact]
        public void StartNewSession_ShouldReturnsSessionid()
        {
            var result = this.service.StartNewSession(GetTestUser(123));
            this.service.ScrumPokerSessions.Count.Should().Be(1);
            this.service.ScrumPokerSessions.First().MasterUser.ChatId.Should().Be(123);
            result.Should().Be(12);
        }

        [Fact]
        public void EndSession_ShouldInformUsers()
        {
            var result = this.service.StartNewSession(GetTestUser(123));
            this.service.EndSession(result);

            this.service.ScrumPokerSessions.Count.Should().Be(0);
            this.messageSender.Received().SendEndSession(Arg.Any<PokerUser[]>());
        }

        [Fact]
        public void AddUserToRunningSession()
        {
            var result = this.service.StartNewSession(GetTestUser(123));
            this.service.AddUserToSession(GetTestUser(2),result);
            this.service.ScrumPokerSessions.First().AllUsers.Count().Should().Be(2);
              
            this.messageSender.Received().InformaAddedUserAndMaster(Arg.Any<PokerUser>(), Arg.Any<PokerUser>());
        }
 
        
        private PokerUser GetTestUser(long chatId)
        {
            return new PokerUser {ChatId = chatId, Firstname = "Test", Lastname = "User", Username = $"Name{chatId}"};
        }
    }
}
