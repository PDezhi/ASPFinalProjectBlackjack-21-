using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.Models
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        // Constructor
        public Deck()
        {
            this.GenerateDeck();
            this.Shuffle();
        }

        /// <summary>
        /// Fill the deck with 52 cards
        /// </summary>
        public void GenerateDeck()
        {
            Cards = new List<Card>();
            var suitValues = Enum.GetValues(typeof(Suit)).Cast<Suit>();
            var rankValues = Enum.GetValues(typeof(Rank)).Cast<Rank>();

            foreach (var suit in suitValues)
            {
                foreach (var rank in rankValues)
                {
                    Cards.Add(new Card(suit, rank));
                }
            }
        }

        /// <summary>
        /// Randomly shuffle the deck by reordering the cards list
        /// </summary>
        public void Shuffle()
        {
            if (Cards.Count > 0)
            {
                Random rnd = new Random();
                this.Cards = this.Cards.OrderBy(x => rnd.Next()).ToList<Card>();
            }
        }

        /// <summary>
        /// Return the first card in the deck and remove it from the deck
        /// </summary>
        /// <returns></returns>
        public Card DrawCard()
        {
            if (Cards.Count > 0)
            {
                Card cardToDraw = Cards[0];
                Cards.RemoveAt(0);
                return cardToDraw;
            }
            return null;
        }
    }
}
