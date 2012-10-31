using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using TripleTriadRefresh.Data.Models;
using TripleTriadRefresh.Server.Framework;

namespace TripleTriadRefresh.Server.Models.System
{
    public class Game
    {
        public Game(IPlayer creator, Rules rules, TradeRules tradeRules, Action<Game> endCallback)
        {
            GameId = IdGenerator.GenerateId();
            InProgress = true;
            CurrentPlayer = FirstPlayer = creator;
            Rules = rules;
            TradeRule = tradeRules;
            GameEnded = endCallback;

            ReconnectTimer = new Timer();
            ReconnectTimer.Enabled = false;
            ReconnectTimer.Elapsed += (s, ea) =>
            {
                if (FirstPlayer == null)
                {
                    SecondPlayerScore = 0;
                    Winner = SecondPlayer;
                }
                else if (SecondPlayer == null)
                {
                    FirstPlayerScore = 0;
                    Winner = FirstPlayer;
                }

                InProgress = false;
                GameEnded(this);
            };
        }

        [JsonProperty("gameId")]
        public string GameId { get; private set; }
        [JsonProperty("firstPlayer")]
        public IPlayer FirstPlayer { get; private set; }
        [JsonProperty("secondPlayer")]
        public IPlayer SecondPlayer { get; set; }
        [JsonProperty("currentPlayer")]
        public IPlayer CurrentPlayer { get; private set; }
        [JsonProperty("winner")]
        public IPlayer Winner { get; private set; }
        [JsonProperty("tradeRule")]
        public TradeRules TradeRule { get; private set; }
        [JsonProperty("rules")]
        public Rules Rules { get; private set; }
        [JsonProperty("cardChain")]
        public List<Card> CardChain { get; private set; }
        [JsonProperty("firstPlayerScore")]
        public int FirstPlayerScore { get; private set; }
        [JsonProperty("secondPlayerScore")]
        public int SecondPlayerScore { get; private set; }
        [JsonProperty("inProgress")]
        public bool InProgress { get; private set; }

        [JsonIgnore]
        public Timer ReconnectTimer { get; private set; }
        [JsonIgnore]
        public Action<Game> GameEnded { get; set; }
        [JsonIgnore]
        public Func<Game, Task> AiMove { get; set; }

        [JsonProperty("board")]
        public readonly Card[,] Board = new Card[3, 3];

        [JsonProperty("elementsOnBoard")]
        public readonly Elemental[,] ElementsOnBoard = new Elemental[3, 3];

        [JsonProperty("isFull")]
        public bool IsFull
        {
            get
            {
                return FirstPlayer != null && SecondPlayer != null;
            }
        }

        [JsonProperty("canJoin")]
        public bool CanJoin
        {
            get
            {
                return !IsFull && Winner == null;
            }
        }

        public IPlayer GetCurrentPlayer(string connectionId)
        {
            return FirstPlayer.ConnectionId == connectionId
                   ? FirstPlayer : SecondPlayer;
        }

        public IPlayer GetOpponent(string connectionId)
        {
            return FirstPlayer.ConnectionId != connectionId
                   ? FirstPlayer : SecondPlayer;
        }

        public IPlayer GetOpponent(IPlayer player)
        {
            return FirstPlayer != player
                   ? FirstPlayer : SecondPlayer;
        }

        public IPlayer GetOpponent()
        {
            return FirstPlayer != CurrentPlayer
                   ? FirstPlayer : SecondPlayer;
        }

        public int GetWinnerScore()
        {
            return Winner == FirstPlayer
                   ? FirstPlayerScore : SecondPlayerScore;
        }

        public int GetDefeatedScore()
        {
            return Winner != FirstPlayer
                   ? SecondPlayerScore : FirstPlayerScore;
        }

        public void NextPlayer()
        {
            CurrentPlayer = CurrentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
        }

        public void MakeOwner(IPlayer player)
        {
            FirstPlayer = player;
            if (player == SecondPlayer)
            {
                SecondPlayer = null;
            }
        }

        public IEnumerable<Card> GetWonCards(IPlayer player)
        {
            var result = from Card val in Board where val != null select val;

            if (TradeRule == TradeRules.Direct)
            {
                return result.Where(x => x.OwnedBy == player.ConnectionId && x.OwnedOriginallyBy != player.ConnectionId);
            }
            return result.Where(x => x.OwnedOriginallyBy != player.ConnectionId);
        }

        public void PlaceCard(string connectionId, string id, int position)
        {
            var cards = GetCurrentPlayer(connectionId).Hand.Cards;
            var card = cards.First(x => x.Id == id);
            var initPos = card.Position;
            card.Position = position;

            if (!Process(card))
            {
                card.Position = initPos;

                if (this.CurrentPlayer is AiPlayer)
                {
                    throw new ArgumentException("AI processing error");
                }
            }
            else
            {
                NextPlayer();

                if (this.CurrentPlayer is AiPlayer && this.InProgress)
                {
                    if (AiMove == null)
                    {
                        throw new ArgumentException("AiMove callback must be set when playing against AI");
                    }

                    Task.Factory.StartNew(() => global::System.Threading.Thread.Sleep(new Random().Next(1000, 2500)))
                        .ContinueWith(t => AiMove(this));
                }
            }
        }

        private bool Process(Card currentCard)
        {
            CardChain = new List<Card>();
            var row = ((BoardTile)currentCard.Position).Row();
            var col = ((BoardTile)currentCard.Position).Col();

            if (Board[row, col] != null)
            {
                return false;
            }

            if (SecondPlayer == null || !FirstPlayer.IsReady || !SecondPlayer.IsReady || CurrentPlayer == null)
            {
                return false;
            }

            ProcessInternal(currentCard, row, col, -1);
            ComputeScore();

            return true;
        }

        private void ProcessInternal(Card currentCard, int row, int col, int chainStep)
        {
            var i = chainStep + 1;
            var currentPosition = (row * 3 + col + 1);

            Board[row, col] = currentCard;

            var adjacentTiles = System.Board.GetAdjacentTiles((BoardTile)currentPosition);
            foreach (var tile in adjacentTiles)
            {
                TestBorder(currentCard, (BoardTile)currentPosition, tile, i);
            }
        }

        private bool TestBorder(Card currentCard, BoardTile currentTile, BoardTile targetTile, int chainStep)
        {
            var currentRow = currentTile.Row();
            var currentCol = currentTile.Col();

            var targetRow = targetTile.Row();
            var targetCol = targetTile.Col();

            var targetCard = Board[targetRow, targetCol];
            if (targetCard == null)
            {
                return true;
            }

            var borders = System.Board.GetCardBorders(currentTile, targetTile);
            var currentStr = GetElemStrength(currentCard.Strength.GetBorderStrength(borders.Item1), currentCard.Elemental, currentRow, currentCol);
            var targetStr = GetElemStrength(targetCard.Strength.GetBorderStrength(borders.Item2), targetCard.Elemental, targetRow, targetCol);

            if (currentStr > targetStr)
            {
                if (TakeCard(targetCard, currentCard.OwnedBy, chainStep) && Rules.HasFlag(Rules.Combo))
                {
                    ProcessInternal(targetCard, targetRow, targetCol, chainStep);
                    return true;
                }
            }
            return false;
        }

        private int GetElemStrength(int strength, Elemental elem, int row, int col)
        {
            if (Rules.HasFlag(Rules.Elemental))
            {
                return strength + ElementsOnBoard[row, col] == elem ? 1 : -1;
            }
            return strength;
        }

        private void ComputeScore()
        {
            var result = from Card val in Board where val != null select val;
            FirstPlayerScore = result.Count(x => x.OwnedBy == FirstPlayer.ConnectionId) + FirstPlayer.Hand.PlayCards.Count;
            SecondPlayerScore = result.Count(x => x.OwnedBy == SecondPlayer.ConnectionId) + SecondPlayer.Hand.PlayCards.Count;

            if ((from Card val in Board where val == null select val).Count() == 0)
            {
                InProgress = false;
                if (FirstPlayerScore != SecondPlayerScore)
                {
                    Winner = FirstPlayerScore > SecondPlayerScore ? FirstPlayer : SecondPlayer;
                }
                GameEnded(this);
            }
        }

        private bool TakeCard(Card card, string newOwner, int chainStep)
        {
            if (card.OwnedBy != newOwner)
            {
                card.OwnedBy = newOwner;
                CardChain.Add(card);
                CurrentPlayer.CardsFlip += 1;
                return true;
            }

            return false;
        }
    }
}