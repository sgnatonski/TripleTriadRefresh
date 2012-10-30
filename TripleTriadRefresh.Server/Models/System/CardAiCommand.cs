using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TripleTriadRefresh.Server.Models.System
{
    public class CardAiCommand : CardCommand
    {
        public double Value { get; private set; }
        public CardAiCommand(string cardId, int position, double value) : base(cardId, position)
        {
            Value = value;
        }
    }
}
