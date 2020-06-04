using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlackJack.Models;
using BlackJack.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BlackJack.Controllers
{
    public class HomeController : Controller
    {
        public static List<Game> games = new List<Game>();

        public IActionResult Index()
        {
            ViewBag.OnlinePlayersCount = games.Count.ToString();
            return View();
        }

        [HttpPost]
        public IActionResult Index(string start)
        {
            if (start == "true")
            {
                Guid gameId = Guid.NewGuid();
                games.Add(new Game(gameId));
                HttpContext.Session.SetString("gameId", gameId.ToString());
                RemoveAbandonedGames();
                return RedirectToAction("PlaceBet");
            }
            ViewBag.OnlinePlayersCount = games.Count.ToString();
            return View();
        }



        public IActionResult PlaceBet()
        {
            Game game = this.GetGameFromSession();
            if (game == null || game.GameStatus != Status.PLACE_BET) return RedirectToAction("Index");

            ViewBag.Balance = game.Player.Balance;
            ViewBag.ErrorMessage = "";
            return View();
        }

        [HttpPost]
        public IActionResult PlaceBet(string bet)
        {
            Game game = this.GetGameFromSession();
            if (game == null || game.GameStatus != Status.PLACE_BET) return RedirectToAction("Index");

            bool isValidInt = Int32.TryParse(bet, out int playerBet);
            if (isValidInt && playerBet > 0 && playerBet <= game.Player.Balance && playerBet <= 100)
            {
                game.Player.PlaceBet(playerBet);
                return RedirectToAction("Game");
            }
            else
            {
                ViewBag.Balance = game.Player.Balance;
                ViewBag.ErrorMessage = "Bet must be a number between 0 and 100, and less than your balance.";
                return View();
            }
        }



        public IActionResult Game()
        {
            Game game = this.GetGameFromSession();
            if (game == null || game.GameStatus == Status.PLACE_BET) return RedirectToAction("Index");

            if (game.Player.Hand.Cards.Count == 0) // prevent page refresh from dealing again
            {
                game.Dealer.DealHands();
            }

            ViewBag.EndGameMessage = CreateEndGameMessage(game); // in case of BlackJack the game starts and ends here
            GameViewModel vm = CreateGameViewModel(game);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Game(string playerAction)
        {
            Game game = this.GetGameFromSession();
            if (game == null || game.GameStatus == Status.PLACE_BET) return RedirectToAction("Index");
            ViewBag.Stand = false; // by default, will be changed below if the player stands

            switch (playerAction)
            {
                case "Hit":
                    game.Player.Hit();
                    break;
                case "Stand":
                    game.Player.Stand();
                    ViewBag.Stand = true; // used to not animate player's cards after a stand (since they don't change)
                    break;
                case "PlayAgain":
                    game.Reset(); // soft reset
                    return RedirectToAction("PlaceBet");
                case "GameOver":
                    games.Remove(game); // hard reset
                    return RedirectToAction("Index");
                default:
                    break;
            }

            ViewBag.EndGameMessage = CreateEndGameMessage(game);
            GameViewModel vm = CreateGameViewModel(game);
            return View(vm);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        /// <summary>
        /// Create & populate the View Model to be passed on to the page
        /// </summary>
        /// <returns></returns>
        private static GameViewModel CreateGameViewModel(Game game)
        {
            return new GameViewModel
            {
                PlayerHand = game.Player.Hand,
                DealerHand = game.Dealer.Hand,
                DealerHandValueToDisplay = game.Dealer.Hand.Value - game.Dealer.Hand.Cards[0].Value(), // used when gamestatus = ongoing
                PlayerBalance = game.Player.Balance,
                Bet = game.Bet,
                GameStatus = game.GameStatus
            };
        }

        /// <summary>
        /// Provides the message to display to the player after the game is over
        /// </summary>
        /// <returns></returns>
        private static string CreateEndGameMessage(Game game)
        {
            switch (game.GameStatus)
            {
                case Status.PLAYER_WIN:
                    return "You Won!";
                case Status.DEALER_WIN:
                    return "You Lost";
                case Status.PLAYER_BLACKJACK_WIN:
                    return "You Won With BlackJack!";
                case Status.PUSH:
                    return "Push";
                default:
                    return "";
            }
        }

        private Game GetGameFromSession()
        {
            Guid gameId;
            try
            {
                gameId = new Guid(HttpContext.Session.GetString("gameId"));
                Game game = games.Where(g => g.GameId == gameId).First();
                return game;
            }
            catch
            {
                return null;
            }
        }

        private static void RemoveAbandonedGames()
        {
            games.RemoveAll(g => DateTime.Now.Subtract(g.GameStartTime).TotalMinutes > 1);
        }

    }
}
