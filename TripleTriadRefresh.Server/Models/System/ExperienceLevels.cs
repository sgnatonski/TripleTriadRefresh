using System.Collections.Generic;

namespace TripleTriadRefresh.Server.Models.System
{
    public class ExperienceLevels
    {
        private static Dictionary<int, int> levels = new Dictionary<int, int>();

        public int this[int level]
        {
            get
            {
                return levels[level];
            }
        }
    }
}
