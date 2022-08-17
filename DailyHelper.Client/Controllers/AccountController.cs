using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DailyHelper.Client.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyHelper.Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpContext _context;
        
        public AccountController(HttpContext httpContent)
        {
            _context = httpContent;
        }
        
        // GET
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View(new UserInfo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserInfo userInfo)
        {
            var claims = new List<Claim> { new(ClaimTypes.Email, userInfo.Email) };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await _context.SignInAsync(claimsPrincipal);
            return RedirectToAction("Index", "Home");
        }
    }
}