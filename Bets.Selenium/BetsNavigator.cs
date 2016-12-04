using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bets.Domain;
using Bets.Domain.PageElements;
using Bets.Selenium.Pages;

namespace Bets.Selenium
{
    public class BetsNavigator : IDisposable
    {
        private static object _sync = new object();
        private static bool _inited;
        private static BetsNavigator _instance;

        public static BetsNavigator Instance = LazyInitializer.EnsureInitialized(ref _instance, ref _inited, ref _sync,
            () => new BetsNavigator());


        private readonly FonbetOnlineBasketPage _fonbetOnlineBasketPage;
        private readonly WinlineOnlineBasketPage _winlineOnlineBasketPage;

        public BetsNavigator()
        {
            _fonbetOnlineBasketPage = new FonbetOnlineBasketPage();
            _winlineOnlineBasketPage = new WinlineOnlineBasketPage();
        }

        public FonbetOnlineBasketPage FonbetPage => _fonbetOnlineBasketPage;
        public WinlineOnlineBasketPage WinlinePage => _winlineOnlineBasketPage;

        public List<ResultViewModel> GetResults(StringBuilder errBuilder)
        {
            IRow[] fbRows = null, winlineRows = null;
            var run = Task.Run(() =>
            {
                winlineRows = _winlineOnlineBasketPage.GetRows(errBuilder);
            });
            var task = Task.Run(() =>
            {
                fbRows = _fonbetOnlineBasketPage.GetRows(errBuilder);
            });

            Task.WaitAll(task, run);

            var results = new List<ResultViewModel>();
            foreach (var fonbetGame in fbRows.OrderBy(r => r.Team1.ToString()))
            {
                var gamesWl = winlineRows
                    .Where(r => r.Team1.Equals(fonbetGame.Team1) || r.Team2.Equals(fonbetGame.Team2))
                    .ToArray();

                if (gamesWl.Length > 1)
                {
                    errBuilder.Append("Дубли: ");
                    FillTeamsNames(fonbetGame, errBuilder);
                    errBuilder.AppendLine();

                    continue;
                }
                if (!gamesWl.Any())
                {
                    errBuilder.Append("Не найдено: ");
                    FillTeamsNames(fonbetGame, errBuilder);
                    errBuilder.AppendLine();

                    continue;
                }

                var gameWl = gamesWl.Single();

                results.Add(new ResultViewModel
                {
                    Team1 = gameWl.Team1,
                    Team2 = gameWl.Team2,
                    Fonbet = new StatViewModel(fonbetGame),
                    Winline = new StatViewModel(gameWl)
                });
            }

            return results;
        }

        private static void FillTeamsNames(IRow row, StringBuilder errorsBuilder)
        {
            foreach (var name in row.Team1?.Names.Union(row.Team2?.Names))
            {
                errorsBuilder.Append($"{name};");
            }
        }

        public void Dispose()
        {
            _fonbetOnlineBasketPage?.Dispose();
            _winlineOnlineBasketPage?.Dispose();
        }
    }
}