using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TripleTriadRefresh.Server.Models.System
{
    public class Board
    {
        public static IEnumerable<BoardTile> GetAdjacentTiles(BoardTile tile)
        {
            switch (tile)
            {
                case BoardTile.TopLeft: return new[] { BoardTile.TopCenter, BoardTile.CenterLeft };
                case BoardTile.TopCenter: return new[] { BoardTile.TopLeft, BoardTile.TopRight, BoardTile.Center };
                case BoardTile.TopRight: return new[] { BoardTile.TopCenter, BoardTile.CenterRight };
                case BoardTile.CenterLeft: return new[] { BoardTile.TopLeft, BoardTile.Center, BoardTile.BottomLeft };
                case BoardTile.Center: return new[] { BoardTile.TopCenter, BoardTile.CenterLeft, BoardTile.CenterRight, BoardTile.BottomCenter };
                case BoardTile.CenterRight: return new[] { BoardTile.TopRight, BoardTile.Center, BoardTile.BottomRight };
                case BoardTile.BottomLeft: return new[] { BoardTile.BottomCenter, BoardTile.CenterLeft };
                case BoardTile.BottomCenter: return new[] { BoardTile.BottomLeft, BoardTile.BottomRight, BoardTile.Center };
                case BoardTile.BottomRight: return new[] { BoardTile.BottomCenter, BoardTile.CenterRight };
                default: return new BoardTile[] {};
            }
        }

        public static Tuple<CardBorder, CardBorder> GetCardBorders(BoardTile tile1, BoardTile tile2)
        {
            var pos1 = (int)tile1;
            var pos2 = (int)tile2;

            if (pos1 - pos2 == 1)
            {
                return Tuple.Create(CardBorder.Left, CardBorder.Right);
            }
            else if (pos1 - pos2  == -1)
            {
                return Tuple.Create(CardBorder.Right, CardBorder.Left);
            }
            else if (pos1 - pos2 == 3)
            {
                return Tuple.Create(CardBorder.Top, CardBorder.Bottom);
            }
            else if (pos1 - pos2 == -3)
            {
                return Tuple.Create(CardBorder.Bottom, CardBorder.Top);
            }

            throw new InvalidOperationException("Not adjacent tiles");
        }
    }
}
