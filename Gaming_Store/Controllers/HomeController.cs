using Gaming_Store.Models;
using Gaming_Store.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Ensure this namespace is included
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Gaming_Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var games = _context.Games.ToList();
            return View(games);
        }

        public IActionResult Login()
        {
            var user = new LoginVM();
            return View(user);
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var client = _context.Clients
                    .SingleOrDefault(c => c.Email == model.Email && c.Password == model.Password);

                if (client != null)
                {
                    HttpContext.Session.SetString("UserRole", "Client");
                    HttpContext.Session.SetString("UserEmail", client.Email);
                    return RedirectToAction("Index");
                }

                var admin = _context.Admins
                    .SingleOrDefault(a => a.Email == model.Email && a.Password == model.Password);

                if (admin != null)
                {
                    HttpContext.Session.SetString("UserRole", "Admin");
                    HttpContext.Session.SetString("UserEmail", admin.Email);
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserRole");
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var client = new Client();
            return View(client);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Client client, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(client);
            }

            if (image != null && image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await image.CopyToAsync(ms);
                    client.Image = ms.ToArray();
                }
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
