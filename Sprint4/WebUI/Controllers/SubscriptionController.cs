using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class SubscriptionController : Controller
    {
        private ISubscriptionRepository subscriptionRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            this.subscriptionRepository = subscriptionRepository;
        }

        // GET: Subscription
        [HttpGet]
        public ActionResult AddSubscription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSubscription(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                string fileExtension = Path.GetExtension(subscription.ImageFile.FileName);
                if (fileExtension != ".jpg")
                {
                    ViewBag.invalidFile = "Je kan alleen .jpg files uploaden.";
                    return View();
                }
                string fileName = Path.GetFileNameWithoutExtension(subscription.ImageFile.FileName);
                string extension = Path.GetFileName(subscription.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                subscription.ImagePath = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                subscription.ImageFile.SaveAs(fileName);
                subscription.ExpireDate = DateTime.Now.AddYears(1);
                subscriptionRepository.SaveSubscription(subscription);
                ModelState.Clear();
                return RedirectToAction("PaymentOptions", new { subscription });
            }
            // If modelstate is invalid
            return View();
        }

        public ViewResult PaymentOptions(Subscription subscription)
        {
            return View(subscription);
        }
    }
}