
using System;
using Newtonsoft.Json;
using TripleTriadRefresh.Data;
using TripleTriadRefresh.Data.Domain;
using TripleTriadRefresh.Data.Models;
using TripleTriadRefresh.Server.Models.System;
namespace TripleTriadRefresh.Server.Models
{
    public class GameResult
    {
        private bool forWinner;

        public GameResult(Game game)
        {
            DbEntity = new DbGameResult()
            {
                GameId = game.GameId,
                Rules = game.Rules,
                TradeRules = game.TradeRule,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                CreatedBy = game.FirstPlayer.UserName,
                ModifiedBy = game.FirstPlayer.UserName,
                Resolved = false,
                Season = game.Season
            };

            Compute(game);

            DbRepository.Current.Add(DbEntity);
        }

        public GameResult(int gameResultId, int playerId)
            : this(DbRepository.Current.Single<DbGameResult>(gameResultId), playerId)
        {
        }

        public GameResult(DbGameResult result, int playerId)
        {
            DbEntity = result;
            forWinner = DbEntity.WinnerId == playerId;
        }

        [JsonIgnore]
        public DbGameResult DbEntity { get; private set; }
        [JsonProperty("gameId")]
        public string GameId { get { return DbEntity.GameId; } }
        [JsonProperty("score")]
        public int Score { get { return forWinner ? DbEntity.WinnerScore : 10 - DbEntity.WinnerScore; } }
        [JsonProperty("expGain")]
        public int ExpGain { get { return forWinner ? DbEntity.WinnerExpGain : DbEntity.DefeatedExpGain; } }
        [JsonProperty("cardPtsGain")]
        public int CardPtsGain { get { return forWinner ? DbEntity.WinnerCardPtsGain : DbEntity.DefeatedCardPtsGain; } }
        [JsonProperty("rules")]
        public Rules Rules { get { return DbEntity.Rules; } }
        [JsonProperty("tradeRules")]
        public TradeRules TradeRules { get { return DbEntity.TradeRules; } }
        [JsonProperty("result")]
        public string Result { get { return DbEntity.WinnerScore == 5 ? "Draw" : forWinner ? "Won" : "Lost"; } }

        public void Compute(Game game)
        {
            var winner = game.Winner ?? game.CurrentPlayer;
            var defeated = game.GetOpponent(game.Winner ?? game.CurrentPlayer);

            double expMod = 0;
            if (forWinner)
            {
                expMod = winner.Hand.HandStrength - defeated.Hand.HandStrength;
            }
            else
            {
                expMod = defeated.Hand.HandStrength - winner.Hand.HandStrength;
            }

            if (expMod < 0)
            {
                expMod = 1 / Math.Abs(expMod);
            }

            if (game.FirstPlayerScore == game.SecondPlayerScore)
            {
                DbEntity.WinnerExpGain = (int)(winner.CardsFlip * 2 / expMod);
                DbEntity.DefeatedExpGain = (int)(defeated.CardsFlip * 2 / expMod);
                DbEntity.WinnerScore = game.FirstPlayerScore;
                DbEntity.WinnerHandStrength = game.FirstPlayer.Hand.HandStrength;
                DbEntity.DefeatedHandStrength = game.SecondPlayer.Hand.HandStrength;
            }
            else
            {
                DbEntity.Winner = game.Winner.DbEntity;
                DbEntity.WinnerHandStrength = game.Winner.Hand.HandStrength;
                DbEntity.Defeated = game.GetOpponent(game.Winner).DbEntity;
                DbEntity.DefeatedHandStrength = game.GetOpponent(game.Winner).Hand.HandStrength;
                DbEntity.WinnerScore = game.GetWinnerScore();

                DbEntity.WinnerCardPtsGain = Math.Abs(game.FirstPlayerScore - game.SecondPlayerScore);
                DbEntity.WinnerExpGain = (int)(winner.CardsFlip * 5 / expMod);
                DbEntity.DefeatedExpGain = (int)(defeated.CardsFlip / expMod);
            }

            DbEntity.WinnerCardsFlip = winner.CardsFlip;
            DbEntity.DefeatedCardsFlip = defeated.CardsFlip;
        }

        public GameResult GetFor(IPlayer player)
        {
            return new GameResult(this.DbEntity, player.DbEntity.Id);
        }
    }
}
