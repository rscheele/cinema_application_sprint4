﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Kassa")]
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult SalesIndex()
        {
            return View();
        }
    }
}