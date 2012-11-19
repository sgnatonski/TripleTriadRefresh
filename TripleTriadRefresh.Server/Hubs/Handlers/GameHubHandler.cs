using System;
using System.Linq;
using System.Threading.Tasks;
using TripleTriadRefresh.Data;
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

        public const string GameNotFoundError = "Game doesn't exist";

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
            Hub.Clients.Caller.updateBoard(game);
            Hub.Clients.Caller.gameJoined(game.GameId);
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
            Hub.Clients.Caller.updateBoard(game);
            Hub.Clients.Caller.gameJoined(game.GameId);
        }

        [GameInject(1, 0, GameHubHandler.GameNotFoundError)]
        [RequireLogin]
        public void JoinGame(string gameId, Game game)
        {
            Hub.Groups.Add(Hub.Context.ConnectionId, game.GameId);

            if (game.CanJoin)
            {
                if (game.FirstPlayer.ConnectionId != Hub.Context.ConnectionId)
                {
                    game.SecondPlayer = new Player()
                    {
                        User = Hub.Context.User,
                        ConnectionId = Hub.Context.ConnectionId
                    };
                    game.SecondPlayer.CreatePlayHand();

                    BroadcastGameList();
                    Hub.Clients.Group(game.GameId).updateBoard(game);
                    Hub.Clients.Caller.gameJoined(game.GameId);
                }
            }
            else
            {
                Hub.Clients.Caller.updateBoard(game);
                Hub.Clients.Caller.gameJoined(game.GameId);
            }
        }

        [GameInject(0, GameHubHandler.GameNotFoundError)]
        [RequireLogin]
        public void LeaveGame(Game game)
        {
            Hub.Groups.Remove(Hub.Context.ConnectionId, game.GameId);

            var gameChanged = false;
            if (game.FirstPlayer.ConnectionId == Hub.Context.ConnectionId)
            {
                game.MakeOwner(game.SecondPlayer);

                if (game.FirstPlayer is AiPlayer)
                {
                    game.MakeOwner(null);
                }
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
                Hub.Clients.Group(game.GameId).updateBoard(game);
            }

            Hub.Clients.Caller.gameLeft();
        }

        [GameInject(0, GameHubHandler.GameNotFoundError)]
        [RequireLogin]
        public void DeclareReady(Game game)
        {
            var player = game.GetCurrentPlayer(Hub.Context.ConnectionId);
            if (!player.IsReady)
            {
                player.IsReady = true;

                if (new Random().Next(0, 10) > 5)
                {
                    game.NextPlayer();
                }
            }

            Hub.Clients.Group(game.GameId).updateBoard(game);
        }

        [GameInject(2, GameHubHandler.GameNotFoundError)]
        [RequireLogin]
        public void PlaceCard(string id, int position, Game game)
        {
            if (game.CurrentPlayer != game.GetCurrentPlayer(Hub.Context.ConnectionId))
            {
                Hub.Clients.Caller.receiveError("Wait for your turn");
                Hub.Clients.Caller.updateBoard(game);
                return;
            }

            game.CurrentPlayer.Play(game, new CardCommand(id, position));

            Hub.Clients.Group(game.GameId).updateBoard(game);
        }

        [GameInject(0, GameHubHandler.GameNotFoundError)]
        [RequireLogin]
        public void GetOwnedCards(Game game)
        {
            Hub.Clients.Caller.receiveWonCards(game.GetWonCards(game.Winner).ToList());
        }

        [GameInject(1, GameHubHandler.GameNotFoundError)]
        [RequireLogin]
        public void ResolveGame(string gameId, Game game)
        {
            Hub.Clients.Caller.gameResolve(new { });
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

            Hub.Clients.Group(game.GameId).updateBoard(game);
            Hub.Clients.Group(game.GameId).playerConnected();
        }

        private void HoldGame(Game game, int reconnectTimeInSec)
        {
            var player = game.GetCurrentPlayer(Hub.Context.ConnectionId);
            if (player != null)
                player.IsReady = false;
            game.ReconnectTimer.Interval = reconnectTimeInSec * 1000;
            game.ReconnectTimer.Start();

            Hub.Clients.Group(game.GameId).updateBoard(game);
            Hub.Clients.Group(game.GameId).playerDisconnected();
        }

        private void GameEnded(Game game)
        {
            // TODO: if called by ai player error wont be handled by interceptor
            GameResult result = null;
            DbRepository.Transacted(() =>
            {
                result = new GameResult(game);
                DbRepository.Current.Add(result.DbEntity);
                game.FirstPlayer.UpdateStanding(result.GetFor(game.FirstPlayer));
                game.SecondPlayer.UpdateStanding(result.GetFor(game.SecondPlayer));
            });

            Games.RemoveGame(game);
            BroadcastGameList();
            Hub.Clients.Group(game.GameId).updateBoard(game);
            Hub.Clients.Caller.receiveResult(result.GetFor(game.GetCurrentPlayer(Hub.Context.ConnectionId)));
        }

        private Task AiMove(Game game)
        {
            return Task.Factory.StartNew(() =>
            {
                game.CurrentPlayer.Play(game, ((AiPlayer)game.CurrentPlayer).GetCommand(game));
                Hub.Clients.Group(game.GameId).updateBoard(game);
            });
        }

        private void BroadcastGameList()
        {
            Hub.Clients.All.receiveGames(Games.GetGameList().ToList());
        }
    }
}