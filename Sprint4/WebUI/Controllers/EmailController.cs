using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class EmailController : Controller
    {
        private IEmailRepository emailRepository;

        public EmailController(IEmailRepository emailRepository)
        {
            this.emailRepository = emailRepository;
        }

        // GET: Email
        [HttpGet]
        public ActionResult Unsubscribe()
        {
            return View("Unsubscribe");
        }

        [HttpPost]
        public ActionResult Unsubscribe(EmailModel emailModel)
        {
            if (ModelState.IsValid)
            {
                    EmailAdress emailAdress = emailRepository.FindEmailAdress(emailModel.EmailAdress);
                if (emailAdress == null)
                {
                    return View("NoEmailFound");
                }
                else
                {
                    emailRepository.DeleteEmailAdress(emailAdress.Email);
                    return View("EmailRemoved");
                }
            }
            else
            {
                return View("Unsubscribe");
            }
        }
    }
}