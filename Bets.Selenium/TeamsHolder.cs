using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Bets.Domain;
using Newtonsoft.Json.Linq;

namespace Bets.Selenium
{
    public class TeamsHolder
    {
        private static object _sync = new object();
        private static bool _inited;
        private static TeamsHolder _instance;

        public static TeamsHolder Instance = LazyInitializer.EnsureInitialized(ref _instance, ref _inited, ref _sync,
            () => new TeamsHolder());

        private readonly List<TeamViewModel> _teams;
        private readonly ReaderWriterLockSlim _lockSlim;

        public TeamsHolder()
        {
            _teams = new List<TeamViewModel>();
            _lockSlim = new ReaderWriterLockSlim();
            FillFromStorage();
        }

        private void FillFromStorage()
        {
            lock (this)
            {
                using (var stream = File.OpenRead("teams.json"))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var teamsArray = JArray.Parse(reader.ReadToEnd());
                        foreach (var team in teamsArray)
                        {
                            var names = team["names"].Select(name => name.Value<string>()).ToList();
                            AddTeam(new TeamViewModel(names));
                        }
                    }
                }
            }
        }

        private void SaveToStorage()
        {
            var array = new JArray();
            foreach (var team in GetTeams())
            {
                var obj = JObject.FromObject(team);
                array.Add(obj);
            }

            lock (this)
            {
                using (var stream = File.OpenWrite("teams.json"))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(array.ToString());
                    }
                }
            }
        }

        public void ConcatTeams(string name1, string name2)
        {
            name1 = name1.Trim();
            name2 = name2.Trim();

            TeamViewModel team;
            _lockSlim.EnterReadLock();
            try
            {
                team = _teams.SingleOrDefault(t => t.Names.Contains(name1) || t.Names.Contains(name2));
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
            if (team == null)
            {
                throw new ArgumentException("Невозможно найти команды");
            }

            if (!team.Names.Contains(name1))
            {
                team.Names.Add(name1);
            }
            else if (!team.Names.Contains(name2))
            {
                team.Names.Add(name2);
            }

            SaveToStorage();
        }

        public TeamViewModel GetTeam(string name)
        {
            name = name.Trim();

            TeamViewModel team;

            _lockSlim.EnterReadLock();
            try
            {
                team = _teams.SingleOrDefault(t => t.Names.Contains(name));
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
            if (team != null)
            {
                return team;
            }

            team = new TeamViewModel(new List<string>(new[] { name }));
            AddTeam(team);

            return team;
        }

        public TeamViewModel[] GetTeams()
        {
            _lockSlim.EnterReadLock();
            try
            {
                return _teams.ToArray();
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        private void AddTeam(TeamViewModel team)
        {
            _lockSlim.EnterWriteLock();
            try
            {
                _teams.Add(team);
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }
        }
    }
}
