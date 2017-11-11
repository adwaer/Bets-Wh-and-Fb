using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bets.Domain;

namespace Bets.Services
{
    public class BetsService : IDisposable
    {
        private Dictionary<BetModel, ResultViewModel> _dictionary = new Dictionary<BetModel, ResultViewModel>();
        private readonly StreamWriter _writer;
        public BetsService()
        {
            _writer =
                new StreamWriter($"bets_log.{DateTime.Now.ToShortDateString()}.csv", true, Encoding.UTF8)
                {
                    AutoFlush = true
                };
        }

        public void Place(ResultViewModel model)
        {
            if (model.IsGoodTotal.Value != 0)
            {
                var moreSide = model.IsGoodTotal.Value > 0 ? model.Team1 : model.Team2;
                var lessSide = model.IsGoodTotal.Value > 0 ? model.Team2 : model.Team1;
                var betModel = new BetModel(moreSide.ToString()
                    , lessSide.ToString()
                    , model.AmountTotal
                    , "TOTAL"
                    , $"{DateTime.Now:dd.mm.yyyy HH:mm}"
                    , model.Fonbet.Total.Value
                    , model.Winline.Total.Value
                    , model.IsGoodTotal.Value);

                AddBet(betModel, model);
            }
            if (model.IsGoodHc.Value != 0)
            {
                var moreSide = model.IsGoodTotal.Value > 0 ? model.Team1 : model.Team2;
                var lessSide = model.IsGoodTotal.Value > 0 ? model.Team2 : model.Team1;

                var betModel = new BetModel(moreSide.ToString()
                    , lessSide.ToString()
                    , model.AmountHandicap
                    , "Hc"
                    , $"{DateTime.Now:dd.mm.yyyy HH:mm}"
                    , model.Fonbet.Handicap.Value
                    , model.Winline.Handicap.Value
                    , model.IsGoodHc.Value);

                AddBet(betModel, model);
            }

        }
        private void AddBet(BetModel betModel, ResultViewModel model)
        {
            if (!_dictionary.ContainsKey(betModel))
            {
                WriteFile($"{DateTime.Now.ToShortTimeString()};{betModel.Category};{betModel.MoreSide};{betModel.LessSide};{betModel.Amount};{betModel.Val1};{betModel.Val2}");
                _dictionary.Add(betModel, model);
            }
        }


        public void Place(TeamViewModel team1, TeamViewModel team2, int side, decimal amount, string category, string val1, string val2)
        {
            var moreSide = side > 0 ? team1 : team2;
            var lessSide = side > 0 ? team2 : team1;
            WriteFile($"{DateTime.Now.ToShortTimeString()};{category};{moreSide};{lessSide};{amount};{val1};{val2}");
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
