using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityCaps.Data;
using QualityCaps.Models;

namespace QualityCaps.Controllers
{
    public class MemberCapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MemberCapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MemberCaps
        public async Task<IActionResult> Index(string sortOrder,
            string searchString,
            string currentFilter,
            int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var caps = from s in _context.Cap
                       .Include(c => c.Category)
                       .Include(b => b.Supplier)
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                caps = caps.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    caps = caps.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    caps = caps.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    caps = caps.OrderByDescending(s => s.Price);
                    break;
                default:
                    caps = caps.OrderBy(s => s.Name);
                    break;
            }



            int pageSize = 4;
            return View(await PaginatedList<Cap>.CreateAsync(caps.AsNoTracking(), page ?? 1, pageSize));

           // var applicationDbContext = _context.Cap.Include(c => c.Category).Include(c => c.Supplier);
           // return View(await applicationDbContext.ToListAsync());
        }

        // GET: MemberCaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Cap
                .Include(c => c.Category)
                .Include(c => c.Supplier)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (cap == null)
            {
                return NotFound();
            }

            return View(cap);
        }

        // GET: MemberCaps/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "ID", "ID");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "ID");
            return View();
        }

        // POST: MemberCaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Price,CategoryID,SupplierID,Image")] Cap cap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "ID", "ID", cap.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "ID", cap.SupplierID);
            return View(cap);
        }

        // GET: MemberCaps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Cap.SingleOrDefaultAsync(m => m.ID == id);
            if (cap == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "ID", "ID", cap.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "ID", cap.SupplierID);
            return View(cap);
        }

        // POST: MemberCaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Price,CategoryID,SupplierID,Image")] Cap cap)
        {
            if (id != cap.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapExists(cap.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "ID", "ID", cap.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "ID", cap.SupplierID);
            return View(cap);
        }

        // GET: MemberCaps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cap = await _context.Cap
                .Include(c => c.Category)
                .Include(c => c.Supplier)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (cap == null)
            {
                return NotFound();
            }

            return View(cap);
        }

        // POST: MemberCaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cap = await _context.Cap.SingleOrDefaultAsync(m => m.ID == id);
            _context.Cap.Remove(cap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapExists(int id)
        {
            return _context.Cap.Any(e => e.ID == id);
        }
    }
}
