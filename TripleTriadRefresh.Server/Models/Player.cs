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
    }
}