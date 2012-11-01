using System;
using System.Collections.Generic;
using SubSonic.SqlGeneration.Schema;

namespace TripleTriadRefresh.Data.Domain
{
    public class DbPlayer : DbEntity
    {
        [SubSonicNullString]
        public string ExternalId { get; set; }
        [SubSonicNullString]
        public string ExternalType { get; set; }
        public DateTime LastSeen { get; set; }
        public bool IsOnline { get; set; }

        public IList<DbCard> Deck
        {
            get
            {
                return GetForeignList<DbCard>(c => c.OwnerId == this.Id);
            }
        }
    }
}