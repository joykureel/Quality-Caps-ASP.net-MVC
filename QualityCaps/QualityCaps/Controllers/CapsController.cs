using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityCaps.Data;
using QualityCaps.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using QualityCaps;

namespace QualityCaps.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CapsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;
        public CapsController(ApplicationDbContext context, IHostingEnvironment hEnv)
        {
            _context = context;
            _hostingEnv = hEnv;
        }

        // GET: Caps
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



            int pageSize = 6;
            return View(await PaginatedList<Cap>.CreateAsync(caps.AsNoTracking(), page ?? 1, pageSize));

            //var applicationDbContext = _context.Cap.Include(c => c.Category).Include(c => c.Supplier);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Caps/Details/5
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

        // GET: Caps/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categorys, "ID", "ID");
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "ID");
            return View();
        }

        // POST: Caps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Price,CategoryID,SupplierID,Image")] Cap cap, IList<IFormFile> _files)
        {
            var relativeName = "";
            var fileName = "";

            if (_files.Count < 1)
            {
                relativeName = "/images/error.jpg";
            }
            else
            {
                foreach (var file in _files)
                {
                    fileName = ContentDispositionHeaderValue
                                      .Parse(file.ContentDisposition)
                                      .FileName
                                      .Trim('"');

                    fileName = Path.GetExtension(fileName);
                    //Path for localhost
                    relativeName = "/images/CapsImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;

                    using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                    {
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }
                }
            }
            cap.Image = relativeName;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(cap);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " + "see your system administrator.");
            }

            ViewData["CategoryID"] = new SelectList(_context.Categorys, "ID", "ID", cap.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "ID", "ID", cap.SupplierID);
            return View(cap);
        }

        // GET: Caps/Edit/5
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

        // POST: Caps/Edit/5
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

        // GET: Caps/Delete/5
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

        // POST: Caps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cap = await _context.Cap.SingleOrDefaultAsync(m => m.ID == id);
            _context.Cap.Remove(cap);
            try
            {

                await _context.SaveChangesAsync();
        }
            catch (DbUpdateException s)
            {
                TempData["CapsUsed"] = "The Caps being deleted has been used in previous orders.Delete those orders before trying again.";
                return RedirectToAction("Delete");
    }

            return RedirectToAction(nameof(Index));
        }

        private bool CapExists(int id)
        {
            return _context.Cap.Any(e => e.ID == id);
        }
    }
}
