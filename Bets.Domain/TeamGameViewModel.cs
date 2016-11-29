namespace Bets.Domain
{
    public class TeamGameViewModel  
    {
        public TeamViewModel Team1 { get; set; }
        public TeamViewModel Team2 { get; set; }
        public string Total { get; set; }
        public string Handicap { get; set; }
        public string League { get; set; }
    }
}
