using Licoreria.Models;
using Licoreria.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Licoreria.Controllers
{
    public class HomeController : Controller
    {
        RepositoryLicoreria repo;

        public HomeController(RepositoryLicoreria repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
