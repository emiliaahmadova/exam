using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Senkadagala.DAL;
using Senkadagala.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Senkadagala.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext db;


        public HomeController(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IActionResult>  Index()
        {
            HomeViewModel hvm = new HomeViewModel()
            {
                services = await db.Services.ToListAsync(),
            };
            return View(hvm);
        }

     
    }
}
