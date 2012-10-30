using System;

namespace TripleTriadRefresh.Data.Domain
{
    public class DbCard : DbEntity
    {
        public int OwnerId { get; set; }
        public int CardId { get; set; }

        public DbPlayer Owner
        {
            get
            {
                return GetForeign<DbPlayer>(OwnerId);
            }
            set
            {
                OwnerId = SetForeign(value);
            }
        }
    }
}