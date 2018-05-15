using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult FindLocation(string Name)
        {
            ViewBag.Name = Name;
            return View("Contact");
        }
    }
}