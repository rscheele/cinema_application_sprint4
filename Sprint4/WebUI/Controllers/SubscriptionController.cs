using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class SubscriptionController : Controller
    {
        // GET: Subscription
        public ActionResult AddSubscription()
        {
            return View();
        }
    }
}