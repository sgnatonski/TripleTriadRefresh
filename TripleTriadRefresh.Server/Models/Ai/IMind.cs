using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Models.Ai
{
    public interface IMind
    {
        void FocusOn(Game game);
        IEnumerable<CardCommand> GetOptions();
        CardCommand GetDecision();
    }
}
