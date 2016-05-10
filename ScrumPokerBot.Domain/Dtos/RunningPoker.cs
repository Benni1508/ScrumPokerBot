using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrumPokerBot.Domain.Dtos
{
    public class RunningPoker
    {
        private readonly ScrumPokerSession session;

        public RunningPoker(ScrumPokerSession session)
        {
            this.session = session;
            Users = session.AllUsers.Select(u => new UserEstimation(u)).ToList();
       
        }

        
        public List<UserEstimation> Users { get;}

        public override string ToString()
        {
            var poker = session.Poker;
            var x = poker.Users.GroupBy(u => u.Estimation).OrderBy(e => e.Key);
            var results = x.ToDictionary(k => k.Key, v => v.Count());
            var sb = new StringBuilder();
            foreach (var result in results)
            {
                sb.AppendLine($"{result.Value} x {result.Key} Story points");
            }

            if (results.Count > 1)
            {
                var lowestEstimation = results.First().Key;
                var highestEstimation= results.Last().Key;

                var highest = poker.Users.Where(u => u.Estimation == highestEstimation).OrderBy(e => Guid.NewGuid()).First();
                var lowest = poker.Users.Where(u => u.Estimation == lowestEstimation).OrderBy(e => Guid.NewGuid()).First();

                sb.AppendLine();
                sb.AppendLine($"Niedrigste Schätzung {lowest.Estimation} von {lowest.User}");
                sb.AppendLine($"Höchste Schätzung {highest.Estimation} von {highest.User}");
            }

            return sb.ToString();
        }

    }
}