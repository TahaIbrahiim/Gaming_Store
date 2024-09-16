using Gaming_Store.Models;
using Gaming_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace Gaming_Store.Controllers
{
    public class GamesController : Controller
    {
        private readonly Context _context;

        public GamesController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var games = _context.Games.Include(s=>s.ClientGames).ToList();
            return View(games);
        }


        [HttpGet]
        public async Task<IActionResult> GetGameById(int id)
        {
            var clientEmail = HttpContext.Session.GetString("UserEmail");
            var userRole = HttpContext.Session.GetString("UserRole");

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Email == clientEmail);

            var game = await _context.Games.FirstOrDefaultAsync(i=>i.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            bool isEnrolled = false;

            if (client != null)
            {
                isEnrolled = await _context.ClientGames.AnyAsync(cg => cg.ClientId == client.Id && cg.GameId == id);
            }

            Console.WriteLine(isEnrolled);

            var viewModel = new GameDetailsVM
            {
                Game = game,
                IsEnrolled = isEnrolled,
                UserRole = userRole,
            };

            Console.WriteLine(userRole);


            return View(viewModel);
        }


        [HttpGet]
        public IActionResult AddGame()
        {
            var games = new Game();
            return View(games);
        }

        [HttpPost]
        public IActionResult AddGame(Game game, IFormFile image, IFormFile video)
        {
            if (game == null)
            {
                return BadRequest();
            }

         
            if (image != null && image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    game.Image = ms.ToArray(); 
                }
            }

      
            if (video != null && video.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    video.CopyTo(ms);
                    game.Video = ms.ToArray(); 
                }
            }

            game.Date = DateTime.Now;
            _context.Games.Add(game);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditGame(int id)
        {
            var game = _context.Games.FirstOrDefault(x => x.Id == id);
            return View(game);
        }


        public IActionResult EditGame(int id, Game game, IFormFile image, IFormFile video)
        {
           
            if (image != null && image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    game.Image = ms.ToArray();
                }
            }


            if (video != null && video.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    video.CopyTo(ms);
                    game.Video = ms.ToArray();
                }
            }

            _context.Entry(game).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


     
        public IActionResult DeleteGame(int id)
        {

            if (id == 0)
            {
                return BadRequest("No Id detected !!!!");
            }
            if (_context.Games is null)
            {
                return BadRequest("No Game detected !!!!");
            }

            var game = _context.Games.Find(id);

            _context.Games.Remove(game);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }


       
        public IActionResult Enroll(int id)
        {
            var clientEmail = User.Identity.Name;
            var client = _context.Clients.FirstOrDefaultAsync(c => c.Email == clientEmail);

            if (client == null)
            {
                return NotFound("Client not found");
            }

            //var existingEnrollment = _context.ClientGames
            //    .FirstOrDefaultAsync(cg => cg.ClientId == client.Id && cg.GameId == id);

            //if (existingEnrollment != null)
            //{
            //    TempData["Message"] = "You are already enrolled in this game.";
            //    return RedirectToAction("GetGameById", new { id });
            //}

            var clientGame = new ClientGame
            {
                ClientId = client.Id,
                GameId = id,
                PurchaseDate = DateTime.Now
            };

            _context.ClientGames.Add(clientGame);
            _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
