using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Bets.Domain
{
    [DataContract]
    public class TeamViewModel
    {
        [DataMember(Name = "names")]
        public List<string> Names { get; }

        public TeamViewModel(List<string> names)
        {
            Names = names;
        }

        public override string ToString()
        {
            return Names.First();
        }

        protected bool Equals(TeamViewModel other)
        {
            return Names.Any(n => other.Names.Any(n1 => n1.Equals(n, StringComparison.CurrentCultureIgnoreCase)));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TeamViewModel) obj);
        }

        public override int GetHashCode()
        {
            return Names?.GetHashCode() ?? 0;
        }
    }
}
