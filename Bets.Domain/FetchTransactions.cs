using System.Collections.Generic;
using System.Text;
using Bets.Domain.PageElements;

namespace Bets.Domain
{
    public class FetchTransactions
    {
        public StringBuilder ErrorBuilder { get; set; }
        public IEnumerable<WinlineRow> WinlineRows { get; set; }
        public IEnumerable<WinlineRow> WinlineRows1 { get; set; }
        public IEnumerable<FonbetRow> FonbetRows { get; set; }
    }
}
