using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class Dealer
    {
        public Deck Deck { get; set; }
        public Hand Hand { get; set; }
        public Player Player { get; set; }

        // Constructor
        public Dealer(Deck deck)
        {
            this.Deck = deck;
            this.Hand = new Hand();
        }

        /// <summary>
        /// Draw cards from the deck and deal 2 cards to the dealer and 2 cards to the player
        /// Then check whether the player's hand is blackjack or not?
        /// </summary>
        public void DealHands()
        {
            this.Hand.AddCardToHand(this.Deck.DrawCard());
            this.Hand.AddCardToHand(this.Deck.DrawCard());

            this.Player.Hand.AddCardToHand(this.Deck.DrawCard());
            this.Player.Hand.AddCardToHand(this.Deck.DrawCard());

            CheckBlackJack();
        }

        /// <summary>
        /// Check whether the dealer and player have blackjack
        /// if the player's hand is blackjack, automatically make the player stand
        /// </summary>
        public void CheckBlackJack()
        {
            Player.Hand.IsBlackJack = (Player.Hand.Value == 21);
            this.Hand.IsBlackJack = (this.Hand.Value == 21);

            if (Player.Hand.IsBlackJack)
            {
                Player.Stand();
            }
        }

        /// <summary>
        /// Draw 1 card from deck and pass it to the player's hand
        /// </summary>
        public void PlayerHit()
        {
            this.Player.Hand.AddCardToHand(this.Deck.DrawCard());
        }

        /// <summary>
        /// Draw 1 or more cards from the deck and pass it to the dealer's hand
        /// until the dealer's hand value is 17 or higher.
        /// </summary>
        public void DealerHit() // after player stands
        {
            while (this.Hand.Value < 17)
            {
                this.Hand.AddCardToHand(this.Deck.DrawCard());
            }

        }

    }
}
