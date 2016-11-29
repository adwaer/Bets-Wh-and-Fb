using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bets.Domain;
using Newtonsoft.Json.Linq;

namespace Bets.Services
{
    public class IgnoreLeaguesHolder
    {
        private static object _sync = new object();
        private static bool _inited;
        private static IgnoreLeaguesHolder _instance;

        public static IgnoreLeaguesHolder Instance = LazyInitializer.EnsureInitialized(ref _instance, ref _inited, ref _sync,
            () => new IgnoreLeaguesHolder());

        public string[] IgnorLeaguees;

        public IgnoreLeaguesHolder()
        {
            FillFromStorage();
        }

        private void FillFromStorage()
        {
            lock (this)
            {
                using (var stream = File.OpenRead("ignore_leagues.json"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var leagues = JArray.Parse(reader.ReadToEnd());
                        IgnorLeaguees = new string[leagues.Count];

                        for (int index = 0; index < leagues.Count; index++)
                        {
                            var league = leagues[index];
                            IgnorLeaguees[index] = league.Value<string>();
                        }
                    }
                }
            }
        }

    }
}
