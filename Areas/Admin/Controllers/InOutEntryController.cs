using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InOutEntryController : Controller
    {
        private ApplicationDbContext _db;

        public InOutEntryController(ApplicationDbContext db) {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
