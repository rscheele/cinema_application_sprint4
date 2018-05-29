using Domain.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class PrintRidesCard
    {
        private Document doc;
        private PdfWriter writer;
        private MemoryStream ms;
        private Font normalFont;
        private Font largeFont;
        private Font smallFont;

        public PrintRidesCard()
        {
            normalFont = FontFactory.GetFont("Segoe UI", 8.0f, BaseColor.BLACK);
            smallFont = FontFactory.GetFont("Segoe UI", 6.0f, BaseColor.BLACK);
            largeFont = FontFactory.GetFont("Segoe UI", 10.0f, BaseColor.BLACK);

            doc = new Document(PageSize.A6);
            ms = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, ms);

            Write();
        }

        public bool Write()
        {
            writer.Open();
            doc.Open();

            addText("Avans Cinema 10-rittenkaart", largeFont);
            addText("Tien keer gratis naar de bioscoop!", normalFont);

            AddEmptyLine();

            System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/Avans.jpg"));
            image = resizeImage(image, new System.Drawing.Size(64, 64));
            BaseColor color = null;
            Image displayImage = Image.GetInstance(image, color);
            doc.Add(displayImage);

            AddEmptyLine();

            addText("Niet geldig in combinatie met andere acties*", smallFont);

            AddEmptyLine();
            AddEmptyLine();
            AddEmptyLine();

            PdfPTable table = new PdfPTable(5);
            
            for (int i = 1; i < 11; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.AddElement(new Paragraph("Rit " + i));
                cell.FixedHeight = 55.0f;
                table.AddCell(cell);
            }

            doc.Add(table);

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
            result.FileDownloadName = "10ridesticket.pdf";
            return result;
        }

        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            return (System.Drawing.Image)(new System.Drawing.Bitmap(imgToResize, size));
        }

    }
}