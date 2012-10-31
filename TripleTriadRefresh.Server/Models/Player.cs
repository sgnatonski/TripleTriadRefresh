using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Data.Domain;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Models
{
    public class Player : IPlayer
    {
        public Player()
        {
            var externalId = (string)HttpContext.Current.Items["UserId"];
            DbEntity = DbRepository.Current.Single<DbPlayer>(x => x.ExternalId == externalId);
        }

        public Player(string externalId)
        {
            DbEntity = DbRepository.Current.Single<DbPlayer>(p => p.ExternalId == externalId);
        }

        public Player(int id)
        {
            DbEntity = DbRepository.Current.Single<DbPlayer>(id);
        }

        [JsonIgnore]
        public DbPlayer DbEntity { get; private set; }

        [JsonIgnore]
        public IPrincipal User { get; set; }

        [JsonIgnore]
        public int CardsFlip { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonIgnore]
        public string IpAddress { get; set; }

        [JsonProperty("isReady")]
        public bool IsReady { get; set; }

        [JsonProperty("userName")]
        public string UserName
        {
            get
            {
                return User.Identity.Name;
            }
        }

        [JsonProperty("hand")]
        public Hand Hand { get; set; }

        public void CreatePlayHand()
        {
            var cards = new List<Card>();
            DbEntity.Deck.Take(5).ToList().ForEach(c => cards.Add(new Card(c) { OwnedOriginallyBy = this.ConnectionId }));
            Hand = new Hand(cards);
        }

        public void Play(Game game, CardCommand command)
        {
            game.PlaceCard(this.ConnectionId, command.CardId, command.Position);
        }

        public void UpdateStanding(DbGameResult gameResult)
        {
            var standing = DbRepository.Current.Single<DbStanding>(s => s.PlayerId == DbEntity.Id && s.SeasonId == gameResult.Season.Id);
            if (standing == null)
            {
                standing = new DbStanding();
                standing.Id = (int)DbRepository.Current.Add(standing);
            }

            standing.Player = DbEntity;
            standing.Season = gameResult.Season;

            double expMod = gameResult.WinnerHandStrength - gameResult.DefeatedHandStrength;
            if (expMod < 0)
            {
                expMod = 1 / Math.Abs(expMod);
            }

            if (gameResult.WinnerId == DbEntity.Id)
            {
                standing.Won += 1;
                standing.CardPoints += gameResult.WinnerScore - 5;
                standing.Experience += (int)(CardsFlip * 10 / expMod);
            }
            else if (gameResult.DefeatedId == DbEntity.Id)
            {
                standing.Lost += 1;
                standing.Experience += (int)(CardsFlip / expMod);
            }
            else
            {
                standing.Draw += 1;
                standing.Experience += (int)(CardsFlip * 5 / expMod);
            }

            if (standing.Experience < 0)
            {
                standing.Experience = 0;
            }

            standing.UnlockedRules = Data.Models.Rules.Open;
            standing.UnlockedTradeRules = Data.Models.TradeRules.Direct;

            DbRepository.Current.Update(standing);
        }
    }
}