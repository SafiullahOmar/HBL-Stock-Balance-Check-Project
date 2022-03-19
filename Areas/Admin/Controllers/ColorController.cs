using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class ColorController : Controller
    {
        private ApplicationDbContext _db;

        public ColorController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var colors = await _db.Colors.Include(c=>c.Company).ToListAsync();
            
            return View(colors);
        }
        public async Task<IActionResult> AddOrEdit(int id = 0)
          {
            ViewData["company"] = new SelectList(_db.Companies.ToList(), "Id", "Name");

            if (id == 0)
                return View(new Color());

            else
            {
                var color = await _db.Colors.Include(c=>c.Company).FirstOrDefaultAsync(c=>c.Id==id);
                if (color == null)
                {
                    return NotFound();
                }

                return View(color);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Color color)
        {
            if (ModelState.IsValid)
            {
                bool Isupdate = false;

                if (id == 0)
                {
                    
                        _db.Colors.Add(color);
                        await _db.SaveChangesAsync();
                        TempData["save"] = "Color Type is Successfully Saved";
                    Isupdate = false;
                   
                    
                }

                else {
                    try {
                        _db.Update(color);
                        await _db.SaveChangesAsync();
                        TempData["edit"] = "Color Descriptions are succesfully upated";
                        Isupdate = true;
                    }
                    catch { throw; }
                }


                return Json(new { isValid = true, update=Isupdate, html = Helper.RenderRazorViewToString(this, "_ViewAll", _db.Colors.Include(c=>c.Company).ToList()) });
            }
            ViewData["company"] = new SelectList(_db.Companies.ToList(), "Id", "Name");
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", color) });
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
           
            if (ModelState.IsValid)
            {
                _db.Remove(await _db.Colors.FindAsync(id));
                await _db.SaveChangesAsync();
                TempData["delete"] = "Color type has been deleted";
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _db.Colors.Include(c=>c.Company).ToList()) });
            }

            return Json(new {  html = Helper.RenderRazorViewToString(this, "AddOrEdit", _db.Colors.Include(c=>c.Company).ToList()) });
        }
    }
}
