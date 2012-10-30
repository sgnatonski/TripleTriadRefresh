using System;
using SubSonic.SqlGeneration.Schema;
using System.Collections.Generic;

namespace TripleTriadRefresh.Data.Domain
{
    public class DbSeason : DbTrackableEntity
    {
        public string Name { get; set; }
    }
}