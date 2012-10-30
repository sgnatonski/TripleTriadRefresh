using System;
using System.Linq;
using TripleTriadRefresh.Data.Domain;
using Newtonsoft.Json;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Server.Framework;

namespace TripleTriadRefresh.Server.Models
{
    public class Card
    {
        private string ownedOriginallyBy;

        public Card(int id) : this(DbRepository.Current.Single<DbCard>(id))
        {
        }

        public Card(DbCard card)
        {
            DbEntity = card;
            Id = IdGenerator.GenerateId();
            InternalId = card.Id;
            CardId = card.CardId;
            Strength = CardImage.GetStrength(CardId);
            Elemental = CardImage.GetElemental(CardId);
        }

        [JsonIgnore]
        public DbCard DbEntity { get; private set; }

        [JsonIgnore]
        public int InternalId { get; private set; }
        [JsonProperty("id")]
        public string Id { get; private set; }
        [JsonProperty("cardId")]
        public int CardId { get; private set; }
        [JsonProperty("ownedOriginallyBy")]
        public string OwnedOriginallyBy 
        {
            get
            {
                return ownedOriginallyBy;
            }
            set 
            { 
                ownedOriginallyBy = value;
                OwnedBy = value;
            }
        }
        [JsonProperty("ownedBy")]
        public string OwnedBy { get; set; }
        [JsonProperty("position")]
        public int Position { get; set; }
        [JsonProperty("strength")]
        public CardStrength Strength { get; private set; }
        [JsonProperty("elemental")]
        public Elemental Elemental { get; private set; }
        [JsonProperty("image")]
        public string Image
        {
            get
            {
                return CardImage.GetImagePath(CardId);
            }
        }
    }
}