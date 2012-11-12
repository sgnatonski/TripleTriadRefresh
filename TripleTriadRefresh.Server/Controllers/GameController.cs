using System;
using System.Linq;
using System.Web.Mvc;
using TripleTriadRefresh.Server.Controllers.Result;
using TripleTriadRefresh.Server.Framework;
using TripleTriadRefresh.Server.Models;

namespace TripleTriadRefresh.Server.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameContainer gameContainer;

        public GameController(IGameContainer gameContainer)
        {
            this.gameContainer = gameContainer;
        }

        public ActionResult Game()
        {
            return View();
        }

        [Authorize]
        public JsonNetResult GetStanding()
        {
            var standing = new PlayerStanding(new Player().DbEntity.Standings.First());
            var json = new JsonNetResult(standing);
            return json;
        }

        [Authorize]
        public JsonNetResult GetDeck()
        {
            var cards = new Player().DbEntity.Deck.Select(c => new Card(c)).ToList();
            var json = new JsonNetResult(cards);
            return json;
        }

        public JsonNetResult GetGameList()
        {
            var json = new JsonNetResult(this.gameContainer.GetGameList().ToList());
            return json;
        }
    }
}
