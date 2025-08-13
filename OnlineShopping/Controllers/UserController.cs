using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Data;
using OnlineShopping.Models;

namespace OnlineShopping.Controllers
// Tryin to commit the code for UserController
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly OnlineShoppingContext _context;

        public UserController(OnlineShoppingContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,UserType")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register([Bind("Username,Password,ConfirmPassword")] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userExist = (from u in _context.User
                                where u.Username == registerViewModel.Username
                                select u).ToList();
                if (userExist.Count > 0)
                {
                    ViewData["ErrorMessage"] = "Username already exists ";
                }
                else
                {
                    User user = new User();
                    user.Username = registerViewModel.Username; 
                    user.Password = registerViewModel.Password;
                    user.UserType = "normal"; 


                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); //needs to redirect to login page
                }
            }
            return View(registerViewModel);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Username,Password,")] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var userExist = (from u in _context.User
                                 where u.Username == loginViewModel.Username
                                    && u.Password == loginViewModel.Password
                                 select u).ToList();
                if (userExist.Count > 0)
                {
                    List <Claim> claims = new List<Claim>();  
                    Claim claim = new Claim(ClaimTypes.Email, loginViewModel.Username);
                    claims.Add(claim);
                    Claim claim1 = new Claim(ClaimTypes.Role, userExist[0].UserType);
                    claims.Add(claim1);
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                   await  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    return RedirectToAction("ProductDashboard", "Product");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Username or Passsword invalid ";
                    
                }
            }
            return View(loginViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("ProductDashboard", "Product");
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,UserType")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
