// Copyright 2011 ReflexSystems
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TripleTriadRefresh.Data.Linq;
using TripleTriadRefresh.Server.Models.System;
using MoreLinq;

namespace TripleTriadRefresh.Server.Models
{
    public class Hand
    {
        private double handStrength;
        private readonly IList<Card> cards;

        public Hand(IList<Card> cards)
        {
            this.cards = cards;
        }

        [JsonIgnore]
        public IEnumerable<Card> Cards
        {
            get
            {
                return this.cards.ToArray();
            }
        }

        [JsonProperty("playCards")]
        public IList<Card> PlayCards 
        { 
            get 
            {
                return this.cards.Where(c => c.Position == 0).ToList(); 
            }
        }

        [JsonProperty("handStrength")]
        public double HandStrength
        {
            get
            {
                if (handStrength == 0 && IsValid)
                {
                    var strengthList = this.cards.SelectMany(c => c.Strength.ToArray());
                    // the more spread values in cards the less relative strength
                    var spread = strengthList.StdDev() * 10;

                    handStrength = strengthList.Average() + strengthList.Sum() / spread;
                }

                return handStrength;
            }
        }

        [JsonProperty("isValid")]
        public bool IsValid
        {
            get
            {
                return this.cards.Count == 5;
            }
        }

        public Card GetStrongestCard(CardBorder border)
        {
            var card = PlayCards.MaxBy(x => x.Strength.GetBorderStrength(border));
            return card;
        }

        public Card GetWeakestCard(CardBorder border)
        {
            var card = PlayCards.MinBy(x => x.Strength.GetBorderStrength(border));
            return card;
        }
    }
}