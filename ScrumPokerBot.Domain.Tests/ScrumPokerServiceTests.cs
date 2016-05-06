using System.Linq;
using FluentAssertions;
using NSubstitute;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Telgram;
using Telegram.Bot.Types;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class ScrumPokerServiceTests
    {
        private readonly IMessageSender messageSender;
        private readonly ScrumPokerService service;

        public ScrumPokerServiceTests()
        {
            messageSender = Substitute.For<IMessageSender>();
            var idGenerator = Substitute.For<IIdGenerator>();
            idGenerator.GetId().Returns(12);
            service = new ScrumPokerService(messageSender, idGenerator);
        }

        [Fact]
        public void StartNewSession_ShouldReturnsSessionid()
        {
            service.StartNewSession(TestHelpers.GetTestUser(123));
            service.ScrumPokerSessions.Count.Should().Be(1);
            service.ScrumPokerSessions.First().MasterUser.ChatId.Should().Be(123);
        }

        [Fact]
        public void ConnectWithWrongId_InformsUser()
        {
            service.ConnectToSession(TestHelpers.GetTestUser(1), 1);
            messageSender.Received().NoSessionFound(Arg.Any<PokerUser>(), 1);
        }

        [Fact]
        public void Test1()
        {
            service.StartNewSession(TestHelpers.GetTestUser(1));
            service.ConnectToSession(TestHelpers.GetTestUser(2), 12);
            messageSender.Received().InformaAddedUserAndMaster(Arg.Is<PokerUser>(p => p.ChatId == 2), Arg.Is<PokerUser>(p => p.ChatId == 1));
        }

        [Fact]
        public void EndSession_ShouldInformUsers()
        {
            service.StartNewSession(TestHelpers.GetTestUser(123));
            service.ConnectToSession(TestHelpers.GetTestUser(2), 12);

            service.LeaveSession(TestHelpers.GetTestUser(2));

            service.ScrumPokerSessions.Count.Should().Be(1);
            messageSender.Received().SendUserLeaveSession(Arg.Is<PokerUser>(u => u.ChatId == 123), Arg.Is<PokerUser>(u => u.ChatId== 2));
        }

        [Fact]
        public void AddUserToRunningSession()
        {
            service.StartNewSession(TestHelpers.GetTestUser(123));
            service.ConnectToSession(TestHelpers.GetTestUser(2), 12);
            service.ScrumPokerSessions.First().AllUsers.Count().Should().Be(2);

            messageSender.Received().InformaAddedUserAndMaster(Arg.Any<PokerUser>(), Arg.Any<PokerUser>());
        }
        [Fact]
        public void StartTwoSessions()
        {
            service.StartNewSession(TestHelpers.GetTestUser(123));
            service.StartNewSession(TestHelpers.GetTestUser(123));

            service.ScrumPokerSessions.First().AllUsers.Count().Should().Be(1);

            messageSender.Received().UserAlreadyInSession(Arg.Any<PokerUser>());
        }
    }

}