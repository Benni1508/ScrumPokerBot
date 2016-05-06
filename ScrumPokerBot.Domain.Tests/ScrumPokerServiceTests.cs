using System.Linq;
using FluentAssertions;
using NSubstitute;
using ScrumPokerBot.Contracts;
using ScrumPokerBot.Telgram;
using Telegram.Bot.Types;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class ConnectionMessageTests
    {
        [Fact]
        public void CreateMessage()
        {
            var message = new ConnectSessionMessage(TestHelpers.GetTestUser(1), "/connect 12");
            message.Sessionid.Should().Be(12);
            message.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateMessage2()
        {
            var message = new ConnectSessionMessage(TestHelpers.GetTestUser(1), "/connect qw");
            message.Sessionid.Should().Be(0);
            message.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateMessage3()
        {
            var message = new ConnectSessionMessage(TestHelpers.GetTestUser(1), "/connect qw");
            message.Sessionid.Should().Be(0);
            message.IsValid.Should().BeFalse();
        }
    }

    public static class TestHelpers
    {
        public static PokerUser GetTestUser(long chatId)
        {
            return new PokerUser {ChatId = chatId, Firstname = "Test", Lastname = "User", Username = $"Name{chatId}"};
        }
    }

    public class StartPokerMessageTests
    {
        [Fact]
        public void MessageTest1()
        {
            var message = new StartPokerMessage(TestHelpers.GetTestUser(1), "/poker qw");
            message.Description.Should().Be("qw");
        }

        [Fact]
        public void MessageTest2()
        {
            var message = new StartPokerMessage(TestHelpers.GetTestUser(1), "/poker");
            message.Description.Should().Be("");
        }

        [Fact]
        public void MessageTest3()
        {
            var message = new StartPokerMessage(TestHelpers.GetTestUser(1), "/poker Enter BLI here");
            message.Description.Should().Be("Enter BLI here");
        }
    }

    public class EstimationMessageTests
    {
        [Fact]
        public void MessageTest1()
        {
            var message = new EstimationMessage(TestHelpers.GetTestUser(1), "1 Story Points");
            message.IsValid.Should().BeTrue();
            message.Estimation.Should().Be(1);
        }

        [Fact]
        public void MessageTest2()
        {
            var message = new EstimationMessage(TestHelpers.GetTestUser(1), "Story Points");
            message.IsValid.Should().BeFalse();
            message.Estimation.Should().Be(0);
        }

        [Fact]
        public void MessageTest3()
        {
            var message = new EstimationMessage(TestHelpers.GetTestUser(1), "asd Story Points");
            message.IsValid.Should().BeFalse();
            message.Estimation.Should().Be(0);
        }
    }

    public class ScrumPokerServiceTests
    {
        private readonly IMessageReceiver messageReceiver;
        private readonly IMessageSender messageSender;
        private readonly ScrumPokerService service;

        public ScrumPokerServiceTests()
        {
            messageSender = Substitute.For<IMessageSender>();
            var idGenerator = Substitute.For<IIdGenerator>();
            messageReceiver = Substitute.For<IMessageReceiver>();
            idGenerator.GetId().Returns(12);
            service = new ScrumPokerService(messageSender, idGenerator);
        }

        [Fact]
        public void StartNewSession_ShouldReturnsSessionid()
        {
            service.OnStartSessionMessageReceived(GetStartSession(123));
            service.ScrumPokerSessions.Count.Should().Be(1);
            service.ScrumPokerSessions.First().MasterUser.ChatId.Should().Be(123);
        }

        private StartSessionMessage GetStartSession(int userChat)
        {
            return new StartSessionMessage(TestHelpers.GetTestUser(userChat), "/Startsession");
        }

        private ConnectSessionMessage GetConnectSession(int userChat, int id)
        {
            return new ConnectSessionMessage(TestHelpers.GetTestUser(userChat), $"/Connect {id}");
        }

        [Fact]
        public void ConnectWithWrongId_InformsUser()
        {
            var message = GetConnectSession(1, 1);
            service.OnConnectedMessageReceived(message);
            messageSender.Received().NoSessionFound(Arg.Any<PokerUser>(), 1);
        }

        [Fact]
        public void Test1()
        {
            service.OnStartSessionMessageReceived(GetStartSession(1));
            service.OnConnectedMessageReceived(GetConnectSession(2,12));
            messageSender.Received().InformaAddedUserAndMaster(Arg.Is<PokerUser>(p => p.ChatId == 2), Arg.Is<PokerUser>(p => p.ChatId == 1));
        }

        [Fact]
        public void EndSession_ShouldInformUsers()
        {
            service.OnStartSessionMessageReceived(GetStartSession(123));
            service.OnConnectedMessageReceived(GetConnectSession(2,12));

            service.OnLeaveSessionMessageReceived(new LeaveSessionMessage(TestHelpers.GetTestUser(2),"/leaveSession"));

            service.ScrumPokerSessions.Count.Should().Be(1);
            messageSender.Received().SendUserLeaveSession(Arg.Is<PokerUser>(u => u.ChatId == 123), Arg.Is<PokerUser>(u => u.ChatId== 2));
        }

        [Fact]
        public void AddUserToRunningSession()
        {
            service.OnStartSessionMessageReceived(GetStartSession(123));
            service.OnConnectedMessageReceived(GetConnectSession(2,12));;
            service.ScrumPokerSessions.First().AllUsers.Count().Should().Be(2);

            messageSender.Received().InformaAddedUserAndMaster(Arg.Any<PokerUser>(), Arg.Any<PokerUser>());
        }
        [Fact]
        public void StartTwoSessions()
        {
            service.OnStartSessionMessageReceived(GetStartSession(123));
            service.OnStartSessionMessageReceived(GetStartSession(123));

            service.ScrumPokerSessions.First().AllUsers.Count().Should().Be(1);

            messageSender.Received().UserAlreadyInSession(Arg.Any<PokerUser>());
        }
    }

}