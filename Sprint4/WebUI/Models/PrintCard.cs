using Domain.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class PrintCard
    {
        private Document doc;
        private PdfWriter writer;
        private MemoryStream ms;
        private Font normalFont;
        private Font largeFont;
        private Font smallFont;

        public PrintCard(Subscription subscription)
        {
            normalFont = FontFactory.GetFont("Segoe UI", 8.0f, BaseColor.BLACK);
            smallFont = FontFactory.GetFont("Segoe UI", 6.0f, BaseColor.BLACK);
            largeFont = FontFactory.GetFont("Segoe UI", 10.0f, BaseColor.BLACK);

            doc = new Document(PageSize.A8);
            ms = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, ms);

            Write(subscription);
        }

        public bool Write(Subscription subscription)
        {
            writer.Open();
            doc.Open();

            addText("Avans Cinema", largeFont);
            addText("Naam: " + subscription.Name, smallFont);
            addText("Adres: " + subscription.Street + " " + subscription.HouseNumber + subscription.HouseNumberExtras + " " + subscription.HomeTown, smallFont);
            addText("Geboortedatum: " + subscription.BirthDate.ToShortDateString(), smallFont);

            AddEmptyLine();

            addText("Barcode: " + subscription.Barcode, smallFont);

            AddEmptyLine();

            System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(subscription.ImagePath));
            image = resizeImage(image, new System.Drawing.Size(48, 48));
            BaseColor color = null;
            Image displayImage = Image.GetInstance(image, color);
            doc.Add(displayImage);

            doc.Close();
            writer.Close();
            return true;
        }

        public MemoryStream getMemoryStream()
        {
            return ms;
        }

        public void addText(String txt, Font font)
        {
            doc.Add(new Paragraph(txt, font));
        }

        public void addText(String txt)
        {
            doc.Add(new Paragraph(txt, normalFont));
        }

        public void AddEmptyLine()
        {
            addText(" ");
        }

        public ActionResult SendPdf()
        {
            MemoryStream stream = getMemoryStream();
            FileStreamResult result = new FileStreamResult(new MemoryStream(stream.GetBuffer()), "pdf/application");
            result.FileDownloadName = "card.pdf";
            return result;
        }

        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            return (System.Drawing.Image)(new System.Drawing.Bitmap(imgToResize, size));
        }
    }
}