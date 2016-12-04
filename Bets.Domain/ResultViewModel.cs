namespace Bets.Domain
{
    public class ResultViewModel
    {
        public TeamViewModel Team1 { get; set; }
        public TeamViewModel Team2 { get; set; }

        public StatViewModel Winline { get; set; }
        public StatViewModel Fonbet { get; set; }
    }
}