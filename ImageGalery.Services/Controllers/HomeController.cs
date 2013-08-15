using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageGallery.Data;
using ImageGallery.Data.Migrations;

namespace ImageGallery.Services.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ImageGaleryContext, Configuration>());
            return View();
        }
    }
}
