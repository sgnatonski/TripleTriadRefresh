using SubSonic.Query;
using System;
using System.Linq;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Data.Domain;
using TripleTriadRefresh.Data.Models;
using TripleTriadRefresh.Server.Models;

namespace TripleTriadRefresh.Server.Framework
{
    public class PlayerActivator : IPlayerActivator
    {
        public IPlayer Activate(string id)
        {
            DbRepository.Transacted(() =>
            {
                var newPlayer = new DbPlayer() { ExternalId = id, ExternalType = "FB" };
                newPlayer.Id = (int)DbRepository.Current.Add(newPlayer);

                var standing = new DbStanding();
                standing.Player = newPlayer;
                standing.Season = DbRepository.Current.Single<DbSeason>(s => s.DateCreated == DbRepository.Current.All<DbSeason>().Max(max => max.DateCreated).Date);
                standing.UnlockedRules = Rules.Open;
                DbRepository.Current.Add(standing);

                CreateInitialDeck(newPlayer);
            });

            return new Player(id);
        }

        private void CreateInitialDeck(DbPlayer newPlayer)
        {
            var rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                DbRepository.Current.Add(new DbCard() { Owner = newPlayer, CardId = rand.Next(1, 10) });
            }
        }
    }
}