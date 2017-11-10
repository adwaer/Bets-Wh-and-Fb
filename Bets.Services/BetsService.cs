using System;
using System.IO;
using System.Text;
using Bets.Domain;

namespace Bets.Services
{
    public class BetsService : IDisposable
    {
        private readonly StreamWriter _writer;
        public BetsService()
        {
            _writer =
                new StreamWriter($"bets_log.{DateTime.Now.ToShortDateString()}.csv", true, Encoding.UTF8)
                {
                    AutoFlush = true
                };
        }

        public void Place(TeamViewModel team1, TeamViewModel team2, int side, decimal amount, string category)
        {
            var moreSide = side > 0 ? team1 : team2;
            var lessSide = side > 0 ? team2 : team1;
            WriteFile($"{DateTime.Now.ToShortTimeString()};{category};{moreSide};{lessSide};{amount}");
        }

        private void WriteFile(string line)
        {
            _writer.WriteLine(line);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
