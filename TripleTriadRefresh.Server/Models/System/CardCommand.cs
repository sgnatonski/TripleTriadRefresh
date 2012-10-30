using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TripleTriadRefresh.Server.Models.System
{
    public class CardCommand
    {
        public string CardId { get; private set; }
        public int Position { get; private set; }

        public CardCommand(string cardId, int position)
        {
            CardId = cardId;
            Position = position;
        }
    }
}
