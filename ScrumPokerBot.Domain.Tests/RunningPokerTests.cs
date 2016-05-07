using System.Collections.Generic;
using Xunit;

namespace ScrumPokerBot.Domain.Tests
{
    public class RunningPokerTests
    {
        [Fact]
        public void Test()
        {
            var results = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                var session = new ScrumPokerSession(TestHelpers.GetTestUser(1), 1);
                session.AddUser(TestHelpers.GetTestUser(2));
                session.AddUser(TestHelpers.GetTestUser(3));
                session.AddUser(TestHelpers.GetTestUser(4));
                session.AddUser(TestHelpers.GetTestUser(5));
                session.AddUser(TestHelpers.GetTestUser(6));
                session.AddUser(TestHelpers.GetTestUser(7));
                session.AddUser(TestHelpers.GetTestUser(8));

                session.StartPoker();
                session.Estimate(TestHelpers.GetTestUser(1), 1);
                session.Estimate(TestHelpers.GetTestUser(2), 1);
                session.Estimate(TestHelpers.GetTestUser(3), 1);
                session.Estimate(TestHelpers.GetTestUser(4), 13);
                session.Estimate(TestHelpers.GetTestUser(5), 13);
                session.Estimate(TestHelpers.GetTestUser(6), 13);
                session.Estimate(TestHelpers.GetTestUser(7), 1);
                session.Estimate(TestHelpers.GetTestUser(8), 1);
                
                results.Add(session.Poker.ToString());
            }
        } 
    }
}