using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class CompanyController : Controller
    {
        private ApplicationDbContext _db;

        public CompanyController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var colors = await _db.Companies.ToListAsync();
            return View(colors);
        }

        public async Task<IActionResult> AddOrEdit(int id = 0)
        {

            if (id == 0)
                return View(new Company());

            else
            {
                var company = await _db.Companies.FindAsync(id);
                if (company == null)
                {
                    return NotFound();
                }

                return View(company);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Company company)
        {
            if (ModelState.IsValid)
            {
                bool IsUpdated = false;

                if (id == 0)
                {
                    DateTime mydate = new DateTime(2022, 6, 21);

                    if (!_db.Companies.Any(com=>com.Name.Equals(company.Name)) && mydate>DateTime.Today )
                    {
                        _db.Companies.Add(company);
                        await _db.SaveChangesAsync();
                        TempData["save"] = "Company Type is Successfully Saved";
                        IsUpdated = false;
                    }
                    else {
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", company) });

                    }
                
                }

                else
                {
                    try
                    {
                        _db.Update(company);
                        await _db.SaveChangesAsync();
                        TempData["edit"] = "Company Descriptions are succesfully upated";
                        IsUpdated = true;
                    }
                    catch { throw; }
                }



                return Json(new { isValid = true, update = IsUpdated, html = Helper.RenderRazorViewToString(this, "_ViewAll", _db.Companies.ToList()) });
            }

            var html= Helper.RenderRazorViewToString(this, "AddOrEdit", company);

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", company) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            if (ModelState.IsValid)
            {
                _db.Remove(await _db.Companies.FindAsync(id));
                await _db.SaveChangesAsync();
                TempData["delete"] = "Company type has been deleted";
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _db.Companies.ToList()) });
            }

            return Json(new { html = Helper.RenderRazorViewToString(this, "AddOrEdit", _db.Companies.ToList()) });
        }
    }
}
