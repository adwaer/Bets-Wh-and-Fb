namespace Bets.Domain
{
    public class ResultViewModel
    {
        public TeamViewModel Team1 { get; set; }
        public TeamViewModel Team2 { get; set; }

        public string TotalWn { get; set; }
        public string HandicapWn { get; set; }
        public string TotalFb { get; set; }
        public string HandicapFb { get; set; }
        public string League { get; set; }
    }
}