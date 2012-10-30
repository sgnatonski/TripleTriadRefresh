using System;

namespace TripleTriadRefresh.Data.Models
{
    [Flags]
    public enum Rules
    {
        Open,
        Same,
        SameWall,
        SuddenDeath,
        Random,
        Plus,
        Combo,
        Elemental
    }
}