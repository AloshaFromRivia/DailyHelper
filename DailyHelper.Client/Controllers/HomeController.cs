using System.Threading.Tasks;
using DailyHelper.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace DailyHelper.Client.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Note> _noteRepository;

       
        public HomeController(IRepository<Note> noteRepository)
        {
            _noteRepository = noteRepository;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}