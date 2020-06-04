using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public enum Suit
    {
        CLUBS, DIAMONDS, HEARTS, SPADES
    }

    public enum Rank
    {
        DEUCE, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, ACE
    }

    public class Card
    {
        public Suit Suit { get; set; }
        public Rank Rank { get; set; }

        // Constructor
        public Card(Suit suit, Rank rank)
        {
            this.Rank = rank;
            this.Suit = suit;
        }

        /// <summary>
        /// Return the value of this card (Ace = 11 for now)
        /// </summary>
        /// <returns></returns>
        public int Value()
        {
            switch (this.Rank)
            {
                case Rank.DEUCE:
                    return 2;
                case Rank.THREE:
                    return 3;
                case Rank.FOUR:
                    return 4;
                case Rank.FIVE:
                    return 5;
                case Rank.SIX:
                    return 6;
                case Rank.SEVEN:
                    return 7;
                case Rank.EIGHT:
                    return 8;
                case Rank.NINE:
                    return 9;
                case Rank.TEN:
                    return 10;
                case Rank.JACK:
                    return 10;
                case Rank.QUEEN:
                    return 10;
                case Rank.KING:
                    return 10;
                case Rank.ACE:
                    return 11;
                default:
                    return 0;
            }
        }
    }
}
