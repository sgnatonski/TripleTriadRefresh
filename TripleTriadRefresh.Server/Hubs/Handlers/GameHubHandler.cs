using System;
using System.Linq;
using System.Threading.Tasks;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Data.Domain;
using TripleTriadRefresh.Data.Models;
using TripleTriadRefresh.Server.Framework;
using TripleTriadRefresh.Server.Framework.Aspects.Attributes;
using TripleTriadRefresh.Server.Models;
using TripleTriadRefresh.Server.Models.System;

namespace TripleTriadRefresh.Server.Hubs.Handlers
{
    [HandleError]
    public class GameHubHandler : IGameHubHandler
    {
        private GameHub Hub { get; set; }
        private IGameContainer Games { get; set; }

        public GameHubHandler(GameHub hub, IGameContainer games)
        {
            Hub = hub;
            Games = games;
        }

        [RequireLogin]
        public void CreateGame()
        {
            var player = new Player() { User = Hub.Context.User, ConnectionId = Hub.Context.ConnectionId };
            player.CreatePlayHand();

            var game = new Game(player, Rules.Open, TradeRules.One, GameEnded);
            Games.AddGame(game);

            Hub.Groups.Add(Hub.Context.ConnectionId, game.GameId);

            BroadcastGameList();
            Hub.Caller.updateBoard(game);
            Hub.Caller.gameJoined(game.GameId);
        }

        [RequireLogin]
        public void CreateGameWithAi()
        {
            var player = new Player() { User = Hub.Context.User, ConnectionId = Hub.Context.ConnectionId };
            player.CreatePlayHand();

            var ai = new AiPlayer(2, 4) { IsReady = true, ConnectionId = "ai_" + IdGenerator.GenerateId() };
            ai.CreatePlayHand();

            var game = new Game(player, Rules.Open, TradeRules.One, GameEnded);
            game.SecondPlayer = ai;
            game.AiMove = AiMove;

            Games.AddGame(game);

            Hub.Groups.Add(Hub.Context.ConnectionId, game.GameId);

            BroadcastGameList();
            Hub.Caller.updateBoard(game);
            Hub.Caller.gameJoined(game.GameId);
        }

        [RequireLogin]
        public void JoinGame(string gameId)
        {
            var game = gameId == null ? Games.GetGame(Hub.Context) : Games.GetGame(gameId);

            if (game != null)
            {
                Hub.Groups.Add(Hub.Context.ConnectionId, game.GameId);

                if (game.CanJoin)
                {
                    //if (game.FirstPlayer.ConnectionId != Hub.Context.ConnectionId)
                    {
                        game.SecondPlayer = new Player()
                        {
                            User = Hub.Context.User,
                            ConnectionId = Hub.Context.ConnectionId
                        };
                        game.SecondPlayer.CreatePlayHand();

                        BroadcastGameList();
                        Hub.Clients[game.GameId].updateBoard(game);
                        Hub.Caller.gameJoined(game.GameId);
                    }
                }
                else
                {
                    Hub.Caller.updateBoard(game);
                    Hub.Caller.gameJoined(game.GameId);
                }
            }
            else
            {
                Hub.Caller.receiveError("Game doesn't exist");
            }
        }

        [RequireLogin]
        public void LeaveGame()
        {
            var game = Games.GetGame(Hub.Context);

            if (game != null)
            {
                Hub.Groups.Remove(Hub.Context.ConnectionId, game.GameId);

                var gameChanged = false;
                if (game.FirstPlayer.ConnectionId == Hub.Context.ConnectionId)
                {
                    game.MakeOwner(game.SecondPlayer);
                    gameChanged = true;
                }
                else if (game.SecondPlayer != null && game.SecondPlayer.ConnectionId == Hub.Context.ConnectionId)
                {
                    game.SecondPlayer = null;
                    gameChanged = true;
                }

                if (gameChanged)
                {
                    if (game.FirstPlayer == null && game.SecondPlayer == null)
                    {
                        Games.RemoveGame(game);
                    }

                    BroadcastGameList();
                    Hub.Clients[game.GameId].updateBoard(game);
                }
            }

            Hub.Caller.gameLeft();
        }

        [RequireLogin]
        public void DeclareReady()
        {
            var game = Games.GetGame(Hub.Context);

            if (game != null)
            {
                game.GetCurrentPlayer(Hub.Context.ConnectionId).IsReady = true;
                Hub.Clients[game.GameId].updateBoard(game);
            }
            else
            {
                Hub.Caller.receiveError("Game doesn't exist");
            }
        }

        [RequireLogin]
        public void PlaceCard(string id, int position)
        {
            var game = Games.GetGame(Hub.Context);

            if (game != null)
            {
                if (game.CurrentPlayer != game.GetCurrentPlayer(Hub.Context.ConnectionId))
                {
                    Hub.Caller.receiveError("Wait for your turn");
                    Hub.Caller.updateBoard(game);
                    return;
                }

                game.CurrentPlayer.Play(game, new CardCommand(id, position));

                Hub.Clients[game.GameId].updateBoard(game);
            }
            else
            {
                Hub.Caller.receiveError("Game doesn't exist");
            }
        }

        [RequireLogin]
        public void GetOwnedCards()
        {
            var game = Games.GetGame(Hub.Context);

            if (game != null)
            {
                Hub.Caller.receiveWonCards(game.GetWonCards(game.Winner).ToList());
            }
            else
            {
                Hub.Caller.receiveError("Game doesn't exist");
            }
        }

        [RequireLogin]
        public void ResolveGame(string gameId)
        {
            var game = Games.GetGame(Hub.Context);

            if (game != null)
            {
                Hub.Caller.gameResolve(new { });
            }
            else
            {
                Hub.Caller.receiveError("Game doesn't exist");
            }
        }

        public void Disconnect()
        {
            var game = Games.GetGame(Hub.Context);

            if (game != null)
            {
                HoldGame(game, 60);
            }
        }

        public void Connect()
        {
            var game = Games.GetGame(Hub.Context);

            if (game != null)
            {
                ResumeGame(game);
            }
        }

        private void ResumeGame(Game game)
        {
            game.ReconnectTimer.Stop();
            var player = game.GetCurrentPlayer(Hub.Context.ConnectionId);
            if (player != null)
                player.IsReady = true;

            Hub.Clients[game.GameId].updateBoard(game);
            Hub.Clients[game.GameId].playerConnected();
        }

        private void HoldGame(Game game, int reconnectTimeInSec)
        {
            var player = game.GetCurrentPlayer(Hub.Context.ConnectionId);
            if (player != null)
                player.IsReady = false;
            game.ReconnectTimer.Interval = reconnectTimeInSec * 1000;
            game.ReconnectTimer.Start();

            Hub.Clients[game.GameId].updateBoard(game);
            Hub.Clients[game.GameId].playerDisconnected();
        }

        private void GameEnded(Game game)
        {
            DbRepository.Transacted(() =>
            {
                var season = DbRepository.Current.Single<DbSeason>(s => true);

                if (season == null)
                {
                    DbRepository.Current.Add(new DbSeason() { Name = "Season 1", CreatedBy = "SYSTEM", ModifiedBy = "SYSTEM" });
                    season = DbRepository.Current.Single<DbSeason>(s => true);
                }

                var newResult = new DbGameResult()
                {
                    GameId = game.GameId,
                    Rules = game.Rules,
                    TradeRules = game.TradeRule,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    CreatedBy = game.GetCurrentPlayer(Hub.Context.ConnectionId).UserName,
                    ModifiedBy = game.GetCurrentPlayer(Hub.Context.ConnectionId).UserName,
                    Resolved = false,
                    Season = season
                };

                if (game.Winner != null)
                {
                    newResult.Winner = game.Winner.DbEntity;
                    newResult.WinnerHandStrength = game.Winner.Hand.HandStrength;
                    newResult.Defeated = game.GetOpponent(game.Winner).DbEntity;
                    newResult.DefeatedHandStrength = game.GetOpponent(game.Winner).Hand.HandStrength;
                    newResult.WinnerScore = game.GetWinnerScore();
                }
                else
                {
                    newResult.WinnerHandStrength = game.GetCurrentPlayer(Hub.Context.ConnectionId).Hand.HandStrength;
                    newResult.DefeatedHandStrength = game.GetOpponent(Hub.Context.ConnectionId).Hand.HandStrength;
                }

                DbRepository.Current.Add(newResult);

                game.FirstPlayer.UpdateStanding(newResult);
                game.SecondPlayer.UpdateStanding(newResult);
            });

            Games.RemoveGame(game);
            BroadcastGameList();
            Hub.Clients[game.GameId].updateBoard(game);
        }

        private Task AiMove(Game game)
        {
            return Task.Factory.StartNew(() =>
            {
                game.CurrentPlayer.Play(game, ((AiPlayer)game.CurrentPlayer).GetCommand(game));
                Hub.Clients[game.GameId].updateBoard(game);
            });
        }

        private void BroadcastGameList()
        {
            Hub.Clients.receiveGames(Games.GetGameList().ToList());
        }
    }
}