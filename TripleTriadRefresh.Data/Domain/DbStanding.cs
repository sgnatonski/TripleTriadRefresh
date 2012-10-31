using TripleTriadRefresh.Data.Models;

namespace TripleTriadRefresh.Data.Domain
{
    public class DbStanding : DbEntity
    {
        public int PlayerId { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }
        public int Draw { get; set; }
        public int SeasonId { get; set; }
        public int Experience { get; set; }
        public int CardPoints { get; set; }
        public Rules UnlockedRules { get; set; }
        public TradeRules UnlockedTradeRules { get; set; }

        public DbPlayer Player
        {
            get
            {
                return GetForeign<DbPlayer>(PlayerId);
            }
            set
            {
                PlayerId = SetForeign(value);
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