using System;
using System.Collections.Generic;
using System.Linq;
using TripleTriadRefresh.Data.Domain;
using TripleTriadRefresh.Data;

namespace TripleTriadRefresh.Server.Framework
{
    public class PlayerActivator : IPlayerActivator
    {
        public void Activate(string id)
        {
            var newPlayer = new DbPlayer() { ExternalId = id, ExternalType = "FB", LastSeen = DateTime.Now };
            DbRepository.Current.Add(newPlayer);

            CreateInitialDeck(newPlayer);
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