using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SignalR.Hubs;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Framework
{
    public class GameContainer : IGameContainer
    {
        private readonly ConcurrentDictionary<string, Game> games = new ConcurrentDictionary<string, Game>();

        public void AddGame(Game game)
        {
            games[game.GameId] = game;
        }

        public Game GetGame(string gameId)
        {
            return games[gameId];
        }

        public Game GetGame(HubCallerContext context)
        {
            return games.Select(x => x.Value).FirstOrDefault(
                x => (x.FirstPlayer.ConnectionId == context.ConnectionId ||
                      x.SecondPlayer == null ||
                      x.SecondPlayer.ConnectionId == context.ConnectionId));
        }

        public IEnumerable<object> GetGameList()
        {
            return games.Select(x => new
            {
                id = x.Key,
                firstName = x.Value.FirstPlayer.UserName,
                secondName = x.Value.SecondPlayer == null ? null : x.Value.SecondPlayer.UserName,
                winnerName = x.Value.Winner == null ? null : x.Value.Winner.UserName,
                firstHandStr = x.Value.FirstPlayer.Hand.HandStrength.ToString("0.00"),
                secondHandStr = x.Value.SecondPlayer == null ? null : x.Value.SecondPlayer.Hand.HandStrength.ToString("0.00"),
                canJoin = x.Value.CanJoin
            });
        }

        public void RemoveGame(Game game)
        {
            Game removedGame;
            games.TryRemove(game.GameId, out removedGame);
        }
    }
}