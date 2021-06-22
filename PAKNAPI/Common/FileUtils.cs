using Aspose.Pdf.Text;
using DocumentFormat.OpenXml.Packaging;
using EPocalipse.IFilter;
using SimpleImpersonation;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PAKNAPI.Common
{
    public static class FileUtils
    {
        public static string ReadFileDocExtension(string path)
        {
            try
            {
                using (var doc = WordprocessingDocument.Open(path, false))
                {
                    //var content = doc.MainDocumentPart.Document.Body.InnerText;
                    var content = doc.MainDocumentPart.Document.Body.ChildElements.ToList();
                    List<string> contentxml = new List<string>();

                    foreach (var item in content)
                    {
                        if (!String.IsNullOrEmpty(item.InnerText))
                        {
                            contentxml.Add(item.InnerText);
                        }

                    }

                    return String.Join(' ', contentxml);
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string ExtractData(string path)
        {
            var pdfDocument = new Aspose.Pdf.Document(path);
            TextAbsorber textAbsorber = new TextAbsorber();
            pdfDocument.Pages.Accept(textAbsorber);
            String extractedText = textAbsorber.Text;
            return extractedText;
        }

        public static string ExtractData1(string path)
        {
            var pdfDocument = new Aspose.Pdf.Document(path);
            TextAbsorber textAbsorber = new TextAbsorber();
            pdfDocument.Pages[1].Accept(textAbsorber);
            String extractedText = textAbsorber.Text;
            return extractedText;
        }


        public static string ExtractData2(string path)
        {
            var pdfDocument = new Aspose.Pdf.Document(path);
            TextAbsorber textAbsorber = new TextAbsorber
            {
                TextSearchOptions =
                {
                    LimitToPageBounds = true,
                    Rectangle = new Aspose.Pdf.Rectangle(
                        0,
                        pdfDocument.PageInfo.Height / 2,
                        pdfDocument.PageInfo.Width,
                        pdfDocument.PageInfo.Height)
                }
            };
            pdfDocument.Pages.Accept(textAbsorber);
            String extractedText = textAbsorber.Text;
            return extractedText;
        }


        public static string ExtractDataFromPDFFile(string path)
        {
            PdfDocument pdf = new PdfDocument();

            byte[] bytes = null;
            bytes = System.IO.File.ReadAllBytes(path);
            pdf.LoadFromBytes(bytes);
            string def = "Evaluation Warning : The document was created with Spire.PDF for .NET.";
            string value = "";
            int count = pdf.Pages.Count;
            for (var i = 0; i < count; i++)
            {
                PdfPageBase page = pdf.Pages[i];
                //string text = page.ExtractText(new RectangleF(50, 50, 500, 100));
                string text = page.ExtractText();
                text = text.Replace(def, "").Trim();
                text = text.Replace(Environment.NewLine, string.Empty);
                text = text.Replace("          ", " ").Replace("         ", " ").Replace("        ", " ").Replace("       ", " ").Replace("      ", " ").Replace("     ", " ").Replace("    ", " ").Replace("   ", " ").Replace("  ", " ");
                value += text;
            }
            return value.ToString();
        }


        public static string ExtractDocFile(string path)
        {
            try
            {
                TextReader reader = new FilterReader(path);
                using (reader)
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}
