using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TripleTriadRefresh.Server.Models;
using TripleTriadRefresh.Server.Models.System;

public class Mind
{
    protected Random random = new Random();
    protected Game game;
    protected double errorChance;
    protected double mistakeChance;
    
    protected Hand Hand { get { return game.CurrentPlayer.Hand; } }

    public Mind()
    {
        errorChance = 0.1;
        mistakeChance = 0.5;
    }

    public void FocusOn(Game game)
    {
        this.game = game;
    }

    public virtual IEnumerable<CardCommand> GetOptions()
    {
        var options = new List<CardAiCommand>();
        if (!IsAnythingOnBoard())
        {
            options.Add(GetCommand(Hand.GetStrongestCard(CardBorder.Bottom | CardBorder.Right), (int)BoardTile.TopLeft));
            options.Add(GetCommand(Hand.GetStrongestCard(CardBorder.Bottom | CardBorder.Left), (int)BoardTile.TopRight));
            options.Add(GetCommand(Hand.GetStrongestCard(CardBorder.Top | CardBorder.Right), (int)BoardTile.BottomLeft));
            options.Add(GetCommand(Hand.GetStrongestCard(CardBorder.Top | CardBorder.Left), (int)BoardTile.BottomRight));
        }

        options.AddRange(GetAttackOptions());

        if (!options.Any())
        {
            options.AddRange(GetDefenceOptions());
        }

        return options.OrderByDescending(x => x.Value);
    }

    public virtual CardCommand GetDecision()
    {
        var options = GetOptions();
        var decisionIndex = 0;
        if (random.NextDouble() > errorChance)
        {
            if (random.NextDouble() > mistakeChance)
            {
                // make better decision - third of best options are possible
                decisionIndex = random.Next((options.Count() - 1) / 3);
            }
            else
            {
                // make good decision - half of worst options are discarded
                decisionIndex = random.Next((options.Count() - 1) / 2);
            }
        }
        else
        {
            // make bad decision - all options are candidates
            decisionIndex = random.Next(options.Count() - 1);
        }

        return options.ElementAt(decisionIndex);
    }

    protected IEnumerable<CardAiCommand> GetAttackOptions()
    {
        var options = new List<CardAiCommand>();

        var opponentCardsOnBoard = (from Card c in game.Board where c != null && c.OwnedBy == game.GetOpponent().ConnectionId select c);
        var cardsToPlay = game.CurrentPlayer.Hand.PlayCards;

        foreach (var opponentCard in opponentCardsOnBoard)
        {
            var adjacentTiles = Board.GetAdjacentTiles((BoardTile)opponentCard.Position);
            foreach (var tile in adjacentTiles)
            {
                if (game.Board[tile.Row(), tile.Col()] != null)
                {
                    continue;
                }

                var borders = Board.GetCardBorders((BoardTile)opponentCard.Position, tile);
                var attackCards = cardsToPlay.Where(c => c.Strength.GetBorderStrength(borders.Item2) > opponentCard.Strength.GetBorderStrength(borders.Item1));

                foreach (var card in attackCards)
                {
                    options.Add(new CardAiCommand(card.Id, (int)tile, GetCommandValue(card, (int)tile)));
                }
            }
        }

        return options;
    }

    protected IEnumerable<CardAiCommand> GetDefenceOptions()
    {
        var options = new List<CardAiCommand>();

        var emptyTiles = Enumerable.Range(1, 9).Except(from Card c in game.Board where c != null select c.Position).Cast<BoardTile>();

        var opponentCards = game.GetOpponent().Hand.PlayCards;

        foreach (var emptyTile in emptyTiles)
        {
            foreach (var card in game.CurrentPlayer.Hand.PlayCards)
            {
                var adjacentTiles = Board.GetAdjacentTiles(emptyTile);
                foreach (var tile in adjacentTiles)
                {
                    if (game.Board[tile.Row(), tile.Col()] != null)
                    {
                        continue;
                    }

                    var borders = Board.GetCardBorders(emptyTile, tile);
                    var opponentAttackCards = opponentCards.Where(c => c.Strength.GetBorderStrength(borders.Item2) > card.Strength.GetBorderStrength(borders.Item1));

                    foreach (var opponentCard in opponentCards)
                    {
                        // computed command value refers to opponent card so when less value for opponent the greater value for ai
                        options.Add(new CardAiCommand(card.Id, (int)emptyTile, -GetCommandValue(opponentCard, (int)tile)));
                    }
                }
            }
        }

        return options;
    }

    protected double GetCommandValue(Card card, int position)
    {
        var score = 0.0;
        var adjacentTiles = Board.GetAdjacentTiles((BoardTile)position);
        
        foreach (var tile in adjacentTiles)
        {
            var row = tile.Row();
            var col = tile.Col();

            var borders = Board.GetCardBorders((BoardTile)position, tile);
            var cardStrength = card.Strength.GetBorderStrength(borders.Item1);
            var adjacentCard = game.Board[row, col];
            if (adjacentCard != null)
            {
                var adjCardStrength = adjacentCard.Strength.GetBorderStrength(borders.Item2);
                if (cardStrength > adjCardStrength)
                {
                    // the less needed to flip card the better
                    score += 10 - (cardStrength - adjCardStrength);
                }
            }
            else
            {
                // empty tile - need to test oponent hand
                score -= GetOpponentHandValue(borders.Item2, cardStrength);
            }
        }
        return score;
    }

    protected double GetOpponentHandValue(CardBorder border, int cardStrength)
    {
        var enemyHand = game.GetOpponent().Hand;
        var strongerCardsCount = enemyHand.PlayCards.Count(c => c.Strength.GetBorderStrength(border) > cardStrength);
        return strongerCardsCount;
    }

    protected CardAiCommand GetCommand(Card card, int position)
    {
        return new CardAiCommand(card.Id, position, GetCommandValue(card, position));
    }

    protected bool IsAnythingOnBoard()
    {
        return (from Card val in game.Board where val != null select val).Any();
    }
}
