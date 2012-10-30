using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Data.Domain;
using System.Diagnostics;
using TripleTriadRefresh.Server.Models.System;
using TripleTriadRefresh.Server.Models.Ai;
using CSScriptLibrary;
using System.Reflection;
using System.Globalization;

namespace TripleTriadRefresh.Server.Models
{
    public class AiPlayer : IPlayer
    {
        private double strength;
        private IMind mind;
        public AiPlayer(int id, double strength)
        {
            //CSScript.GlobalSettings.AddSearchDir(@"C:\Users\Slawek\Documents\GitHub\TripleTriadRefresh\TripleTriadRefresh.Server\bin\Debug\Scripts\Ai\");
            //var ass = CSScript.Load(@"Mind.cs");
            //this.mind = ass.CreateInstance("Mind", false, BindingFlags.CreateInstance, null, null, CultureInfo.InvariantCulture, null).AlignToInterface<IMind>();
            this.mind = new Mind().AlignToInterface<IMind>();

            this.strength = strength;
            DbEntity = DbRepository.Current.Single<DbPlayer>(id);
            this.ConnectionId = Guid.NewGuid().ToString();
        }

        [JsonIgnore]
        public DbPlayer DbEntity { get; private set; }

        public string ConnectionId { get; set; }
        public string IpAddress { get; set; }
        public bool IsReady { get; set; }
        public string UserName
        {
            get
            {
                return "AI_" + DbEntity.Id.ToString();
            }
        }
        public Hand Hand { get; set; }

        public void CreatePlayHand()
        {
            var cards = new List<Card>();
            var rnd = new Random();

            for (int i = 0; i < 5; i++)
            {
                var cardStr = this.strength * (0.75 + rnd.NextDouble());
                var cardIds = CardImage.GetUpToStrength((int)cardStr);

                var cardId = cardIds.Any() ? cardIds[rnd.Next(0, cardIds.Length - 1)] : 1;

                cards.Add(new Card(new DbCard() { CardId = cardId }) { OwnedOriginallyBy = this.ConnectionId });              
            }

            Hand = new Hand(cards);
        }

        public void Play(Game game, CardCommand command)
        {
            game.PlaceCard(this.ConnectionId, command.CardId, command.Position);
        }

        public CardCommand GetCommand(Game game)
        {
            mind.FocusOn(game);
            return mind.GetDecision();
        }
    }
}