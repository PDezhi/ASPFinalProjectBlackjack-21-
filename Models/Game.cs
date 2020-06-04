using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Models
{

    public enum Status
    {
        PLACE_BET, ONGOING, PLAYER_WIN, PLAYER_BLACKJACK_WIN, DEALER_WIN, PUSH
    }

    public class Game
    {
        public Guid GameId { get; set; }
        public DateTime GameStartTime { get; set; } // used to remove inactive games after some time
        public Dealer Dealer { get; set; }
        public Player Player { get; set; }
        public int Bet { get; set; }
        public Status GameStatus { get; set; }
        public int INITIAL_BALANCE { get; set; } = 500;

        // Constructor
        public Game(Guid id)
        {
            this.GameId = id;
            this.GameStartTime = DateTime.Now;
            this.Init();
        }

        /// <summary>
        /// Initialize the Game: 1. create a dealer and a player, 2. introduce them to eachother
        /// </summary>
        public void Init()
        {
            this.Dealer = new Dealer(new Deck());
            this.Player = new Player(this, this.INITIAL_BALANCE);

            this.Dealer.Player = this.Player;
            this.Player.Dealer = this.Dealer;

            this.GameStatus = Status.PLACE_BET;
            // Wait on Player to Stand or Player Hand to Bust
        }

        /// <summary>
        /// The player hand value is over 21: The player has lost
        /// </summary>
        public void PlayerHandIsBust()
        {
            this.GameStatus = Status.DEALER_WIN;
        }

        /// <summary>
        /// The player decided to stand:
        /// First check for blackjack winner,
        /// Then have the dealer draw cards if necessary,
        /// Then find the winner and update the player's balance accordingly
        /// </summary>
        public void PlayerStands()
        {
            if (Player.Hand.IsBlackJack)
            {
                if (Dealer.Hand.IsBlackJack)
                {
                    // Player Loses
                    this.GameStatus = Status.DEALER_WIN;
                    return;
                }
                else
                {
                    // player wins 1.5 times
                    this.Player.Balance += (int)(2.5 * this.Bet); // players original Bet + 1.5 times the Bet
                    this.GameStatus = Status.PLAYER_BLACKJACK_WIN;
                    return;
                }
            }

            Dealer.DealerHit();

            if (Dealer.Hand.IsBust)
            {
                // Player wins
                this.Player.Balance += (2 * this.Bet); // players original Bet + 1 times the Bet
                this.GameStatus = Status.PLAYER_WIN;
                return;
            }

            if (Dealer.Hand.Value > Player.Hand.Value)
            {
                // Player loses
                this.GameStatus = Status.DEALER_WIN;
                return;
            }

            if (Dealer.Hand.Value == Player.Hand.Value)
            {
                // Push
                this.Player.Balance += (this.Bet); // players original Bet
                this.GameStatus = Status.PUSH;
                return;
            }

            if (Dealer.Hand.Value < Player.Hand.Value)
            {
                // Player wins
                this.Player.Balance += (2 * this.Bet); // players original Bet + 1 times the Bet
                this.GameStatus = Status.PLAYER_WIN;
                return;
            }

        }

        /// <summary>
        /// Reset to start a new game, this is not a full reset. The player is balance is untouched.
        /// This method is called when the player hits the "Play Again" button, to continue playing with the remainder of his balance
        /// </summary>
        public void Reset()
        {
            this.GameStartTime = DateTime.Now;
            this.Dealer.Deck = new Deck();
            this.Dealer.Hand = new Hand();
            this.Player.Hand = new Hand();
            this.Bet = 0;
            this.GameStatus = Status.PLACE_BET;
        }

    }
}
