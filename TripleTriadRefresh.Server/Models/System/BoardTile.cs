using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TripleTriadRefresh.Server.Models.System
{
    public enum BoardTile
    {
        TopLeft = 1,
        TopCenter,
        TopRight,
        CenterLeft,
        Center,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public static class BoardTileExtensions
    {
        public static int Row(this BoardTile tile)
        {
            return ((int)tile - 1) / 3;
        }
        public static int Col(this BoardTile tile)
        {
            return ((int)tile - 1) % 3;
        }
    }
}
