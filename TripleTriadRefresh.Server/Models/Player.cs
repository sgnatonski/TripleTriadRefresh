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

        public void UpdateStanding(GameResult gameResult)
        {
            var standing = DbRepository.Current.Single<DbStanding>(s => s.PlayerId == DbEntity.Id && s.SeasonId == gameResult.DbEntity.SeasonId);
            if (standing == null)
            {
                standing = new DbStanding();
                standing.Id = (int)DbRepository.Current.Add(standing);
            }

            standing.Player = DbEntity;
            standing.Season = gameResult.DbEntity.Season;

            standing.Experience += gameResult.ExpGain;

            if (gameResult.Score - 5 > 0)
            {
                standing.Won += 1;
                standing.CardPoints += gameResult.CardPtsGain;
            }
            else if (gameResult.Score - 5 < 0)
            {
                standing.Lost += 1;
            }
            else
            {
                standing.Draw += 1;
            }

            if (standing.Experience < 0)
            {
                standing.Experience = 0;
            }

            standing.UnlockedRules = Data.Models.Rules.Open;
            standing.UnlockedTradeRules = Data.Models.TradeRules.Direct;

            DbRepository.Current.Update(standing);
        }

        public void Logout()
        {
            DbEntity.LastSeen = DateTime.Now;
            DbEntity.IsOnline = false;
            DbRepository.Current.Update(DbEntity);
        }

        public void Login()
        {
            if (DbEntity != null)
            {
                DbEntity.LastSeen = DateTime.Now;
                DbEntity.IsOnline = true;
                DbRepository.Current.Update(DbEntity);
            }
        }
    }
}