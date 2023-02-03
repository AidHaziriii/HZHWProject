using HZHWProject.Configuration;
using HZHWProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Text;

namespace HZHWProject.Controllers
{
    public class AccountController : Controller
    {

        private readonly UPD8DbContext _context;

        public AccountController(UPD8DbContext context)
        {
            _context = context;
        }

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        void ConnectionString()
        {
            con.ConnectionString = "data source = localhost; database = HZHW; integrated security = SSPI;";
        }
        [HttpPost]
        public object Verify(Account account)
        {
            ConnectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from Account where username ='" + account.username + "' and password= '" + account.password + "'";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                HttpContext.Session.SetString("SessionKey", account.username);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                con.Close();
                return View("Error");
            }

        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Account model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = new Account
            {
                username = model.username,
                password = model.password,
                email = model.email,
                role = model.role
            };

            _context.Account.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        // public async Task<IActionResult> Index()
        //{
        //       return View(await _context.Account.ToListAsync());
        //}

        public async Task<IActionResult> Index()
        {
            // Retrieve user information from the session
            var data = HttpContext.Session.Get("SessionKey");
            if (data != null)
            { 
                ViewBag.Username = data;
            }
            return View(await _context.Account.ToListAsync());
        }


            // GET: Users/Details/5
            public async Task<IActionResult> Details(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,username,password,email,role")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(ID);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int ID, [Bind("ID,username,password,email,role")] Account account)
        {
            if (ID != account.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(account.ID))
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
            return View(account);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var account = await _context.Account.FindAsync(ID);
            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int ID)
        {
            return _context.Account.Any(e => e.ID == ID);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("SessionKey");
            return RedirectToAction("Login", "Account");
        }
    }
}
