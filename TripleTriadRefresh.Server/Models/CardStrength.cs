using Newtonsoft.Json;
using System.Collections.Generic;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Models
{
    public class CardStrength
    {
        public CardStrength(int top, int right, int bottom, int left)
        {
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.Left = left;
        }

        [JsonProperty("top")]
        public int Top { get; private set; }
        [JsonProperty("right")]
        public int Right { get; private set; }
        [JsonProperty("bottom")]
        public int Bottom { get; private set; }
        [JsonProperty("left")]
        public int Left { get; private set; }

        public IEnumerable<int> ToArray()
        {
            return new[]
            {
                Top,
                Right,
                Bottom,
                Left
            };
        }

        public int GetBorderStrength(CardBorder border)
        {
            var str = 0;

            if (border.HasFlag(CardBorder.Top)) str += Top;
            if (border.HasFlag(CardBorder.Right)) str += Right;
            if (border.HasFlag(CardBorder.Bottom)) str += Bottom;
            if (border.HasFlag(CardBorder.Left)) str += Left;

            return str;
        }
    }
}