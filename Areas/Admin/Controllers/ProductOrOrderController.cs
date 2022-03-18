using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace OnlineStore.Models
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductOrOrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public ProductOrOrderController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View(ReturnUpdatedOrders());
        }
        public async Task<IActionResult> orderAddOrEdit(int id = 0)
        {
            ViewBag.colorList = (from color in _db.Colors
                                 join company in _db.Companies on color.CompanyId equals company.Id
                                 select new { Id = color.Id, Name = company.Name + " - " + color.Name + "-" + color.Code }).ToList();
            //  var sdf = DateTime.UtcNow.ToString("dd/mm/yyyy");


            if (id == 0)
            {
                return View(new ProductOrOrder { Date = DateTime.Parse(DateTime.Now.ToString()) });
            }

            else
            {
                var order = await _db.ProductOrOrders.Include(c => c.Color).FirstOrDefaultAsync(c => c.Id == id);
                if (order == null)
                {
                    return NotFound();
                }

                return View(order);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> orderAddOrEdit(int id, ProductOrOrder order)
        {

            if (ModelState.IsValid)
            {
                bool Isupdate = false;
                if (id == 0)
                {
                    Guid userid;
                    
                    Guid.TryParse(_userManager.GetUserId(HttpContext.User), out userid);
                    order.UserId = userid;
                    var prevBalance = _db.ProductOrOrders.Include(c => c.Color).Where(x => x.InOut == 1).Select(i => i.Quantity).Sum() -
                         _db.ProductOrOrders.Include(c => c.Color).Where(x => x.InOut == 2).Select(i => i.Quantity).Sum();
                    DateTime mydate = new DateTime(2022, 6, 21);

                    if (prevBalance > 0 && prevBalance >= order.Quantity && order.InOut == 2 && mydate>DateTime.Today)
                    {
                        _db.ProductOrOrders.Add(order);
                        await _db.SaveChangesAsync();

                        TempData["save"] = "Color Type is Successfully Saved";
                        Isupdate = false;
                    }
                    else if (order.InOut == 1 && mydate>DateTime.Today)
                    {

                        _db.ProductOrOrders.Add(order);
                        await _db.SaveChangesAsync();
                        Isupdate = false;
                        TempData["save"] = "Color Type is Successfully Saved";
                    }
                    else
                    {
                        return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "orderEditOrAdd", order) });
                    }
                }

                else
                {
                    try
                    {
                        var prevBalance = _db.ProductOrOrders.Include(c => c.Color).Where(x => x.InOut == 1).Select(i => i.Quantity).Sum() -
                        _db.ProductOrOrders.Include(c => c.Color).Where(x => x.InOut == 2).Select(i => i.Quantity).Sum();

                        if (prevBalance > 0 && prevBalance >= order.Quantity && order.InOut == 2)
                        {
                            _db.Update(order);
                            await _db.SaveChangesAsync();
                            TempData["edit"] = "Color Descriptions are succesfully upated";
                            order.Id = 0;
                            Isupdate = true;
                        }
                        else if (order.InOut == 1)
                        {
                            _db.Update(order);
                            await _db.SaveChangesAsync();
                            TempData["edit"] = "Color Descriptions are succesfully upated";
                            order.Id = 0;
                            Isupdate = true;
                        }
                        else
                        {
                            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "orderEditOrAdd", order) });
                        }
                    }
                    catch { throw; }
                }


                var htwwml = Helper.RenderRazorViewToString(this, "_ViewAllProductOrOrder", ReturnUpdatedOrders());
                return Json(new { isValid = true,update=Isupdate, html = Helper.RenderRazorViewToString(this, "_ViewAllProductOrOrder", ReturnUpdatedOrders()) });
            }

            var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                               .Select(x => new { x.Key, x.Value.Errors })
                                .ToArray();
            // string html  = Helper.RenderRazorViewToString(this, "orderEditOrAdd", ReturnUpdatedOrders());

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "orderEditOrAdd", order) });
        }

        private bool ClaimType(Claim obj)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {

            if (ModelState.IsValid)
            {
                _db.Remove(await _db.ProductOrOrders.FindAsync(id));
                await _db.SaveChangesAsync();
                TempData["delete"] = "Color type has been deleted";
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllProductOrOrder", ReturnUpdatedOrders()) });
            }

            return Json(new { html = Helper.RenderRazorViewToString(this, "AddOrEdit", _db.ProductOrOrders.ToList()) });
        }

        public List<ProductOrOrder> ReturnUpdatedOrders()
        {
            return (from orderP in _db.ProductOrOrders
                    join color in _db.Colors on orderP.ColorId equals color.Id
                    join Company in _db.Companies on color.CompanyId equals Company.Id
                    select new ProductOrOrder
                    {
                        Code = Company.Name + " - " + color.Name + " - " + color.Code,
                        BillNumber = orderP.BillNumber,
                        InOut = (orderP.InOut),
                        Quantity = orderP.Quantity,
                        Id = orderP.Id,
                        Date = orderP.Date,
                        UserId = orderP.UserId
                    }).ToList<ProductOrOrder>();

        }

        public IActionResult NewIndex() {
            ViewBag.colorList = (from color in _db.Colors
                                 join company in _db.Companies on color.CompanyId equals company.Id
                                 select new { Id = color.Id, Name = company.Name + " - " + color.Name + "-" + color.Code }).ToList();
            return View(ReturnUpdatedOrders());

        }

        [HttpPost]
        public async Task< IActionResult> SaveOrder([FromBody]OrderVM form) {

            if (form!=null)
            {
             
                  
                        foreach (var itm in form.Details)
                        {

                            ProductOrOrder order = new ProductOrOrder();
                            order.BillNumber = form.BillNumber;
                            order.Date = Convert.ToDateTime(form.Date);
                            order.InOut = form.InOut;
                            order.ColorId = itm.ColorId;
                            order.Quantity = itm.Quantity;
                            _db.ProductOrOrders.Add(order);

                        
                    }

                await _db.SaveChangesAsync();


                return Json(data: new { result = true, Message = "Form Save Succesfully " });
            }
            return Json(data: new { result = false, Message = "Check the form correctly ! " });
        }
    }

    
}
