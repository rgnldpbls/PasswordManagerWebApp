using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PasswordManagerWebApp.Data;
using PasswordManagerWebApp.Models;

namespace PasswordManagerWebApp.Controllers
{
    public class PWManagersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PWManagersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PWManagers
        public async Task<IActionResult> Index()
        {
            return View(await _context.PWManager.ToListAsync());
        }

        // GET: PWManagers/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: PWManagers/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String Search)
        {
            return View("Index", await _context.PWManager.Where(j => j.WebsiteName.Contains(Search)).ToListAsync());
        }

        // GET: PWManagers/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pWManager = await _context.PWManager
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pWManager == null)
            {
                return NotFound();
            }

            return View(pWManager);
        }

        // GET: PWManagers/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PWManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WebsiteName,Username,Password")] PWManager pWManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pWManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pWManager);
        }

        // GET: PWManagers/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pWManager = await _context.PWManager.FindAsync(id);
            if (pWManager == null)
            {
                return NotFound();
            }
            return View(pWManager);
        }

        // POST: PWManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,WebsiteName,Username,Password")] PWManager pWManager)
        {
            if (id != pWManager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pWManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PWManagerExists(pWManager.Id))
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
            return View(pWManager);
        }

        // GET: PWManagers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pWManager = await _context.PWManager
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pWManager == null)
            {
                return NotFound();
            }

            return View(pWManager);
        }

        // POST: PWManagers/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pWManager = await _context.PWManager.FindAsync(id);
            if (pWManager != null)
            {
                _context.PWManager.Remove(pWManager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PWManagerExists(int id)
        {
            return _context.PWManager.Any(e => e.Id == id);
        }
    }
}
