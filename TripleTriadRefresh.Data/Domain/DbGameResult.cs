using TripleTriadRefresh.Data.Models;

namespace TripleTriadRefresh.Data.Domain
{
    public class DbGameResult : DbTrackableEntity
    {
        public string GameId { get; set; }
        public int WinnerId { get; set; }
        public int DefeatedId { get; set; }
        public int SeasonId { get; set; }
        public int WinnerScore { get; set; }
        public double WinnerHandStrength { get; set; }
        public double DefeatedHandStrength { get; set; }
        public double WinnerCardsFlip { get; set; }
        public double DefeatedCardsFlip { get; set; }
        public int WinnerExpGain { get; set; }
        public int DefeatedExpGain { get; set; }
        public int WinnerCardPtsGain { get; set; }
        public int DefeatedCardPtsGain { get; set; }
        public bool Resolved { get; set; }
        public Rules Rules { get; set; }
        public TradeRules TradeRules { get; set; }

        public DbPlayer Winner
        {
            get
            {
                return GetForeign<DbPlayer>(WinnerId);
            }
            set
            {
                WinnerId = SetForeign(value);
            }
        }

        public DbPlayer Defeated
        {
            get
            {
                return GetForeign<DbPlayer>(DefeatedId);
            }
            set
            {
                DefeatedId = SetForeign(value);
            }
        }

        public DbSeason Season
        {
            get
            {
                return GetForeign<DbSeason>(SeasonId);
            }
            set
            {
                SeasonId = SetForeign(value);
            }
        }
    }
}