using Domain.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class PrintGiftCard
    {
        private Document doc;
        private PdfWriter writer;
        private MemoryStream ms;
        private Font normalFont;
        private Font largeFont;
        private Font smallFont;

        public PrintGiftCard(int value)
        {
            normalFont = FontFactory.GetFont("Segoe UI", 8.0f, BaseColor.BLACK);
            smallFont = FontFactory.GetFont("Segoe UI", 6.0f, BaseColor.BLACK);
            largeFont = FontFactory.GetFont("Segoe UI", 10.0f, BaseColor.BLACK);

            doc = new Document(PageSize.A6);
            ms = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, ms);

            Write(value);
        }

        public bool Write(int value)
        {
            writer.Open();
            doc.Open();

            addText("Avans Cinema Bioscoopbon", largeFont);
            addText("€ " + value + ",00", largeFont);
            addText("Korting op je volgende bioscoopbezoek!", normalFont);

            AddEmptyLine();

            System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/Avans.jpg"));
            image = resizeImage(image, new System.Drawing.Size(64, 64));
            BaseColor color = null;
            Image displayImage = Image.GetInstance(image, color);
            doc.Add(displayImage);

            AddEmptyLine();

            addText("Niet geldig in combinatie met andere acties*", smallFont);
            addText("Geen wisselgeld wordt terug gegeven**", smallFont);

            AddEmptyLine();
            AddEmptyLine();
            AddEmptyLine();

            addText("BARCODE RUIMTE", largeFont);
            AddEmptyLine();
            Random random = new Random();
            int barCode = random.Next(100000000, 999999999);
            addText(barCode + "", largeFont);

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
            result.FileDownloadName = "giftcard.pdf";
            return result;
        }

        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            return (System.Drawing.Image)(new System.Drawing.Bitmap(imgToResize, size));
        }
    }
}