using System;
using TripleTriadRefresh.Data.Models;

namespace TripleTriadRefresh.Data.Domain
{
    public class DbGameResult : DbTrackableEntity
    {
        public int WinnerId { get; set; }
        public int DefeatedId { get; set; }
        public int SeasonId { get; set; }
        public int WinnerScore { get; set; }
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