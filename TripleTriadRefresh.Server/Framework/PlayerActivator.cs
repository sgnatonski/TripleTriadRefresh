using System;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Data.Domain;
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
                DbRepository.Current.Add(newPlayer);

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