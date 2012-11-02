using System;
using System.Linq;
using TripleTriadRefresh.Data.Domain;
using Newtonsoft.Json;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Server.Framework;
using TripleTriadRefresh.Data.Models;

namespace TripleTriadRefresh.Server.Models
{
    public class PlayerStanding
    {

        public PlayerStanding(int id) : this(DbRepository.Current.Single<DbStanding>(id))
        {
        }

        public PlayerStanding(DbStanding standing)
        {
            DbEntity = standing;
        }

        [JsonIgnore]
        public DbStanding DbEntity { get; private set; }

        [JsonProperty("won")]
        public int Won { get { return DbEntity.Won; } }
        [JsonProperty("lost")]
        public int Lost { get { return DbEntity.Lost; } }
        [JsonProperty("draw")]
        public int Draw { get { return DbEntity.Draw; } }
        [JsonProperty("experience")]
        public int Experience { get { return DbEntity.Experience; } }
        [JsonProperty("cardPoints")]
        public int CardPoints { get { return DbEntity.CardPoints; } }
        [JsonProperty("unlockedRules")]
        public Rules UnlockedRules { get { return DbEntity.UnlockedRules; } }
        [JsonProperty("unlockedTradeRules")]
        public TradeRules UnlockedTradeRules { get { return DbEntity.UnlockedTradeRules; } }
    }
}