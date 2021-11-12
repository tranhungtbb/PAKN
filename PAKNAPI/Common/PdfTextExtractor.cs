
using IronPdf;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Syncfusion.OCRProcessor;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Syncfusion.Pdf.Parsing;
using Line = Syncfusion.OCRProcessor.Line;
using Syncfusion.Pdf.Graphics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace PAKNAPI.Common
{
    public static class PdfTextExtractorCustom
    {
        public static string ReadPdfFile(string fileName)
        {
            StringBuilder text = new StringBuilder();

            if (File.Exists(fileName))
            {
                PdfReader pdfReader = new PdfReader(fileName);

                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
                pdfReader.Close();
            }
            return text.ToString();
        }
        public static string PerformOCR(string path, IWebHostEnvironment _hostingEnvironment)
        {
            try
            {
                string resulttext = string.Empty; 
                string docPath = path;
                FileStream docStream = new FileStream(docPath, FileMode.Open, FileAccess.Read);

                //Load the PDF document 
                PdfLoadedDocument loadedDocument = new PdfLoadedDocument(docStream);

                string tesseractPath = _hostingEnvironment.WebRootPath + "/Data/Tesseractbinaries/Windows";

                //Initialize the OCR processor by providing the path of tesseract binaries
                using (OCRProcessor processor = new OCRProcessor(tesseractPath))
                {
                    //Language to process the OCR
                    processor.Settings.Language = "vie";

                    //string fontPath = _hostingEnvironment.WebRootPath + "/Data/ARIALUNI.ttf";
                    //FileStream fontStream = new FileStream(fontPath, FileMode.Open, FileAccess.Read);
                    //processor.UnicodeFont = new PdfTrueTypeFont(fontStream, 8);

                    string tessdataPath = _hostingEnvironment.WebRootPath + "/Data/tessdata";
                    //Process OCR by providing loaded PDF document, Data dictionary, and language
                    //processor.PerformOCR(loadedDocument, tessdataPath);

                    OCRLayoutResult hocrBounds = new OCRLayoutResult();

                    processor.PerformOCR(loadedDocument, tessdataPath, out hocrBounds);
                    StreamWriter writer = new StreamWriter("data.txt");

                    foreach (Page pages in hocrBounds.Pages)
                    {
                        foreach (Line line in pages.Lines)
                        {
                            resulttext += line.Text;
                        }
                    }

                    writer.Close();
                }

                ////Saving the PDF to the MemoryStream
                //MemoryStream stream = new MemoryStream();
                //loadedDocument.Save(stream);

                ////Close the PDF document 
                //loadedDocument.Close(true);

                ////Set the position as '0'
                //stream.Position = 0;

                ////Download the PDF document in the browser
                //FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

                //fileStreamResult.FileDownloadName = "OCR.pdf";

                return resulttext;
            }
            catch (System.Exception ex)
            {
                return "";
            }
            
        }
    }
}
