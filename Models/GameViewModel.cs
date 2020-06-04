using BlackJack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.ViewModels
{
    public class GameViewModel
    {
        public Hand PlayerHand { get; set; }
        public Hand DealerHand { get; set; }

        public int DealerHandValueToDisplay { get; set; } // used when game status = ongoing

        public int PlayerBalance { get; set; }
        public int Bet { get; set; }

        public Status GameStatus { get; set; }
    }
}
