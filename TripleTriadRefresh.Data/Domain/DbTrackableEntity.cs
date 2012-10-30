using System;
using SubSonic.SqlGeneration.Schema;

namespace TripleTriadRefresh.Data.Domain
{
    [IgnoreCreateTable]
    public class DbTrackableEntity : DbEntity
    {
        public string CreatedBy { get; set; }
        [SubSonicNullString]
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}