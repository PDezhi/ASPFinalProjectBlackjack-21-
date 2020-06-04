using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class Player
    {
        public Hand Hand { get; set; }
        public Dealer Dealer { get; set; }
        public int Balance { get; set; }
        public Game Game { get; set; }

        // Constructor
        public Player(Game game, int initialBalance)
        {
            this.Game = game;
            this.Balance = initialBalance;
            this.Hand = new Hand();
        }

        /// <summary>
        /// Remove the Bet amount from player's balance and send it to the Game object's Bet variable
        /// </summary>
        /// <param name="Bet"></param>
        public void PlaceBet(int Bet)
        {
            if (this.Game.GameStatus != Status.PLACE_BET) return; // if user opens the place bet page during a game

            if (this.Balance >= Bet)
            {
                this.Balance -= Bet;
                this.Game.Bet = Bet;
                this.Game.GameStatus = Status.ONGOING;
            }
        }

        /// <summary>
        /// If the player's hand value is 21, automatically make the player stand
        /// If the player's hand is bust, inform the Game object
        /// </summary>
        public void CheckHandStatus()
        {
            if (this.Hand.Value == 21)
            {
                this.Stand();
            }
            else if (this.Hand.IsBust)
            {
                this.Game.PlayerHandIsBust();
            }
        }

        /// <summary>
        /// Ask the dealer for a new card, and check the player's hand status aferward
        /// </summary>
        public void Hit()
        {
            if (this.Game.GameStatus != Status.ONGOING) return; // if user hits refresh after the game ends
            this.Dealer.PlayerHit();
            this.CheckHandStatus();
        }

        /// <summary>
        /// Inform the Game object that the player decided to stand
        /// </summary>
        public void Stand()
        {
            if (this.Game.GameStatus != Status.ONGOING) return; // if user hits refresh after the game ends
            this.Game.PlayerStands();
        }
    }
}
