using System;
using SubSonic.SqlGeneration.Schema;
using System.Collections.Generic;

namespace TripleTriadRefresh.Data.Domain
{
    public class DbPlayer : DbEntity
    {
        [SubSonicNullString]
        public string ExternalId { get; set; }
        [SubSonicNullString]
        public string ExternalType { get; set; }
        public DateTime LastSeen { get; set; }

        public IList<DbCard> Deck
        {
            get
            {
                return GetForeignList<DbCard>(c => c.OwnerId == this.Id);
            }
        }
    }
}