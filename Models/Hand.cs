using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class Hand
    {
        public List<Card> Cards { get; set; }
        public int Value { get; set; }
        public bool IsBust { get; set; }
        public bool IsBlackJack { get; set; }

        // Constructor
        public Hand()
        {
            this.Cards = new List<Card>();
        }

        /// <summary>
        /// Find the value of this hand
        /// First calculate the value (Ace = 11)
        /// Then if the value is over 21, start converting Ace values one by one to 1 until value is <= 21 or we run out of Aces
        /// </summary>
        /// <returns></returns>
        public int CalculateHandValue()
        {
            int sum = 0;
            int aceCount = 0;

            foreach (Card card in Cards)
            {
                if (card.Rank == Rank.ACE)
                {
                    aceCount++;
                }

                sum += card.Value();
            }

            if (sum > 21)
            {
                while (aceCount > 0)
                {
                    sum -= 10; // Change Ace value from 11 to 1 (11-1=10)
                    aceCount--;
                    if (sum <= 21) break;
                }
            }

            return sum;
        }

        /// <summary>
        /// Adding a card to the cardlist and checking the status of this hand
        /// </summary>
        /// <param name="card"></param>
        public void AddCardToHand(Card card)
        {
            this.Cards.Add(card);
            this.CheckHand();
        }

        /// <summary>
        /// Check whether this hand is bust, find the value of this hand
        /// </summary>
        public void CheckHand()
        {
            this.Value = CalculateHandValue();
            this.IsBust = (this.Value > 21);
        }

    }

}
