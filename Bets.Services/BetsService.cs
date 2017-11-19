using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bets.Domain;
using Bets.Selenium;

namespace Bets.Services
{
    public class BetsService : IDisposable
    {
        private readonly ConcurrentDictionary<BetModel, ResultViewModel> _dictionary = new ConcurrentDictionary<BetModel, ResultViewModel>();
        private readonly StreamWriter _writer;
        private readonly string _autobetting;
        public BetsService()
        {
            _autobetting = ConfigurationManager.AppSettings["autobetting"];

            _writer =
                new StreamWriter($"bets_log.{DateTime.Now.ToShortDateString()}.csv", true, Encoding.UTF8)
                {
                    AutoFlush = true
                };
        }

        public async Task Place(ResultViewModel model)
        {
            if (model.AutoBettingTotal && model.IsGoodTotal.Value != 0)
            {
                var betModel = new BetModel(model.Team1.ToString(), model.Team2.ToString(), "TOTAL");
                //var moreSide = model.IsGoodTotal.Value > 0 ? model.Team1 : model.Team2;
                //var lessSide = model.IsGoodTotal.Value > 0 ? model.Team2 : model.Team1;
                //var betModel = new BetModel(moreSide.ToString()
                //    , lessSide.ToString()
                //    , model.AmountTotal
                //    , "TOTAL"
                //    , $"{DateTime.Now:dd.mm.yyyy HH:mm}"
                //    , model.Fonbet.Total.Value
                //    , model.Winline.Total.Value
                //    , model.IsGoodTotal.Value);

                await AddBet(betModel, model, true);
            }
            if (model.AutoBettingHandicap && model.IsGoodHc.Value != 0)
            {
                var betModel = new BetModel(model.Team1.ToString(), model.Team2.ToString(), "HC");

                //var moreSide = model.IsGoodHc.Value > 0 ? model.Team1 : model.Team2;
                //var lessSide = model.IsGoodHc.Value > 0 ? model.Team2 : model.Team1;

                //var betModel = new BetModel(moreSide.ToString()
                //    , lessSide.ToString()
                //    , model.AmountHandicap
                //    , "Hc"
                //    , $"{DateTime.Now:dd.mm.yyyy HH:mm}"
                //    , model.Fonbet.Handicap.Value
                //    , model.Winline.Handicap.Value
                //    , model.IsGoodHc.Value);

                await AddBet(betModel, model, false);
            }

        }
        private async Task AddBet(BetModel betModel, ResultViewModel model, bool total)
        {
            if (!_dictionary.ContainsKey(betModel))
            {
                if (!_dictionary.TryAdd(betModel, model))
                {
                    return;
                }

                if (_autobetting.Equals("live"))
                {
                    bool placed = total
                        ? await BetsNavigator.Instance.PlaceTotal(model)
                        : await BetsNavigator.Instance.PlaceHc(model);
                }
                if (_autobetting.Equals("file"))
                {
                    WriteFile($"{DateTime.Now.ToShortTimeString()};{betModel.Category};{betModel.MoreSide};{betModel.LessSide};{betModel.Amount};{betModel.Val1};{betModel.Val2}");
                }
            }
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
