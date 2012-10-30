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
            var result = new List<int>();

            var val = (int)tile;
            result.AddRange(new[]
            {
                (val - 1),
                (val + 1),
                (val - 3),
                (val + 3),
            });

            return result.Where(x => x > 0 && x < 9).Cast<BoardTile>();
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
