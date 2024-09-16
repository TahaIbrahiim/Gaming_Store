using Gaming_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gaming_Store.Controllers
{
    public class ClientsController : Controller
    {
        private readonly Context _context;
        public ClientsController(Context context)
        {
            _context = context;
            
        }
        public IActionResult Index()
        {
            var clients = _context.Clients.ToList();
            return View(clients);
        }

        public IActionResult GetClientById(int id)
        {
            var clients = _context.Clients.SingleOrDefault(x => x.Id == id);
            return View(clients);
        }

        public IActionResult EditClient(int id)
        {
            var client = _context.Clients.SingleOrDefault(client => client.Id == id);
            return View(client);
        }

        [HttpPost]
        public IActionResult EditClient(int id, Client client, IFormFile image)
        {
            if (id == 0)
            {
                return BadRequest("No Id detected !!!!");
            }
            if (id != client.Id)
            {
                return BadRequest("Not the same ID !!!!");
            }

            if (image != null && image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    client.Image = ms.ToArray();
                }
            }

            _context.Entry(client).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteClient(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return BadRequest();
            }

            _context.Clients.Remove(client);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
