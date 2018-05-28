using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

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
                Random random = new Random();
                subscription.Barcode = random.Next(100000000, 999999999);
                subscriptionRepository.SaveSubscription(subscription);
                ModelState.Clear();
                return RedirectToAction("PaymentOptions", subscription);
            }
            // If modelstate is invalid
            return View();
        }

        [HttpGet]
        public ViewResult PaymentOptions(Subscription subscription)
        {
            return View(subscription);
        }

        [HttpGet]
        public ActionResult PayPIN(Subscription subscription)
        {
            return RedirectToAction("SuccesfulPayment", subscription);
        }

        [HttpGet]
        public ActionResult PayCash(Subscription subscription)
        {
            return RedirectToAction("SuccesfulPayment", subscription);
        }

        [HttpGet]
        public ViewResult SuccesfulPayment(Subscription subscription)
        {
            return View(subscription);
        }

        [HttpGet]
        public ActionResult PrintCard(Subscription subscription)
        {
            var pdf = new PrintCard(subscription);
            return pdf.SendPdf();
        }
    }
}