using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Kassa")]
    public class SalesController : Controller
    {
        // GET: Sales
        [HttpGet]
        public ActionResult SalesIndex()
        {
            return View();
        }

        [HttpGet]
        public ViewResult GiftCard()
        {
            return View();
        }

        [HttpGet]
        public ViewResult GiftCardSale(int value)
        {
            if (value != 0) {
                TempData["Value"] = value;
            }
            else
            {
                value = (int)TempData["Value"];
                TempData["Value"] = value;
            }
            ViewBag.Value = value;
            return View();
        }

        [HttpGet]
        public ViewResult SuccesfulPayment(int value)
        {
            if (value != 0)
            {
                TempData["Value"] = value;
            }
            else
            {
                value = (int)TempData["Value"];
                TempData["Value"] = value;
            }
            ViewBag.Value = value;
            return View();
        }

        [HttpGet]
        public ActionResult PrintGiftCard(int value)
        {
            var pdf = new PrintGiftCard(value);
            return pdf.SendPdf();
        }

        [HttpGet]
        public ViewResult TenRides()
        {
            return View();
        }

        [HttpGet]
        public ViewResult SuccesFulRidesPayment()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PrintRidesTicket()
        {
            var pdf = new PrintRidesCard();
            return pdf.SendPdf();
        }
    }
}