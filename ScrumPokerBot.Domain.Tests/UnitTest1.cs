using System.Linq;
using FluentAssertions;
using NSubstitute;
using ScrumPokerBot.Contracts;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class UnitTest1
    {
        private readonly IMessageSender messageSender;
        private readonly ScrumPokerService service;

        public UnitTest1()
        {
            messageSender = Substitute.For<IMessageSender>();
            var idGenerator = Substitute.For<IIdGenerator>();
            var messageReceiver = Substitute.For<IMessageReceiver>();
            idGenerator.GetId().Returns(12);
            service = new ScrumPokerService(messageSender, idGenerator, messageReceiver);
        }

        [Fact]
        public void StartNewSession_ShouldReturnsSessionid()
        {
            var result = service.StartNewSession(GetTestUser(123));
            service.ScrumPokerSessions.Count.Should().Be(1);
            service.ScrumPokerSessions.First().MasterUser.ChatId.Should().Be(123);
            result.Should().Be(12);
        }

        [Fact]
        public void EndSession_ShouldInformUsers()
        {
            var result = service.StartNewSession(GetTestUser(123));
            service.EndSession(result);

            service.ScrumPokerSessions.Count.Should().Be(0);
            messageSender.Received().SendEndSession(Arg.Any<PokerUser[]>());
        }

        [Fact]
        public void AddUserToRunningSession()
        {
            var result = service.StartNewSession(GetTestUser(123));
            service.AddUserToSession(GetTestUser(2), result);
            service.ScrumPokerSessions.First().AllUsers.Count().Should().Be(2);

            messageSender.Received().InformaAddedUserAndMaster(Arg.Any<PokerUser>(), Arg.Any<PokerUser>());
        }

        private PokerUser GetTestUser(long chatId)
        {
            return new PokerUser {ChatId = chatId, Firstname = "Test", Lastname = "User", Username = $"Name{chatId}"};
        }
    }
}