using System;
using System.Linq;
using System.Text;
using Bets.Domain;
using Bets.Services;

namespace Bats.Desktop
{
    internal static class Helpers
    {
        //public static ResultViewModel[] ParseGames(TeamGameViewModel[] whGames, TeamGameViewModel[] fonbetGames,
        //   StringBuilder errBuilder)
        //{
        //    var results = whGames
        //        .Where(l => !IgnoreLeaguesHolder.Instance.IgnorLeaguees.Any(il => il.Equals(l.League, StringComparison.CurrentCultureIgnoreCase)))
        //        .Select(game => new ResultViewModel
        //        {
        //            Team1 = game.Team1,
        //            Team2 = game.Team2,
        //            HandicapWl = game.Handicap,
        //            TotalWl = game.Total
        //        })
        //    .ToList();

        //    fonbetGames = fonbetGames
        //        .Where(l => !IgnoreLeaguesHolder.Instance.IgnorLeaguees.Any(il => il.Equals(l.League, StringComparison.CurrentCultureIgnoreCase)))
        //        .ToArray();

        //    foreach (var game in fonbetGames)
        //    {
        //        var games = results
        //            .Where(r => r.Team1.Equals(game.Team1) || r.Team2.Equals(game.Team2))
        //            .ToArray();

        //        if (games.Length > 1)
        //        {
        //            errBuilder.Append("Найдено слишком много матчей по названиям: ");
        //            FillTeamsNames(game, errBuilder);
        //            errBuilder.AppendLine();

        //            continue;
        //        }
        //        if (!games.Any())
        //        {
        //            errBuilder.Append("Не найдены совпадения: ");
        //            FillTeamsNames(game, errBuilder);
        //            errBuilder.AppendLine();

        //            //results.Add(new ResultViewModel
        //            //{
        //            //    Team1 = game.Team1,
        //            //    Team2 = game.Team2,
        //            //    HandicapFb = game.Handicap,
        //            //    TotalFb = game.Total,
        //            //    League = game.League
        //            //});

        //            continue;
        //        }

        //        var gameWh = games.Single();
        //        gameWh.HandicapFb = game.Handicap;
        //        gameWh.TotalFb = game.Total;
        //    }

        //    //foreach (var game in whGames)
        //    //{
        //    //    if (!foundGames.Any(g => g.Team1.Equals(game.Team1) || g.Team2.Equals(game.Team2)))
        //    //    {
        //    //        results.Add(new ResultViewModel
        //    //        {
        //    //            Team1 = game.Team1,
        //    //            Team2 = game.Team2,
        //    //            HandicapWh = game.Handicap,
        //    //            TotalWh = game.Total
        //    //        });
        //    //    }
        //    //}

        //    return results
        //        .Where(r => !string.IsNullOrEmpty(r.HandicapFb) && !string.IsNullOrEmpty(r.HandicapWl))
        //        .OrderBy(r => r.Team1.ToString()).ToArray();
        //}

        private static void FillTeamsNames(TeamGameViewModel game, StringBuilder errorsBuilder)
        {
            foreach (var name in game.Team1?.Names.Union(game.Team2?.Names))
            {
                errorsBuilder.Append($"{name};");
            }
        }

    }
}
