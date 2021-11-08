
using IronPdf;
using System.Drawing;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
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
        // SelectPdf
        public static string ExtractTextFromPdf(string path)
        {
            PDDocument doc = null;
            try
            {
                doc = PDDocument.load(path);
              PDFTextStripper stripper = new PDFTextStripper();
                return stripper.getText(doc);
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }
        }
        public static string pdfText(string path)
        {
            //Initialize the OCR processor by providing the path of tesseract 
            using (OCRProcessor processor = new OCRProcessor(@"TesseractBinaries\Windows"))
            {
                //Load a PDF document
                FileStream stream = new FileStream(path, FileMode.Open);

                PdfLoadedDocument document = new PdfLoadedDocument(stream);

                // Sets Unicode font to preserve the Unicode characters in a PDF document.
                FileStream fontStream = new FileStream(@"ARIALUNI.ttf", FileMode.Open);

                processor.UnicodeFont = new PdfTrueTypeFont(fontStream, 8);

                //Set OCR language to process
                processor.Settings.Language = Languages.English;

                //Process OCR by providing the PDF document, data dictionary, and language
                processor.PerformOCR(document, @"tessdata\");


                //Creating the stream object 
                MemoryStream streamobj = new MemoryStream();

                //Save the document into stream.
                document.Save(streamobj);

                //If the position is not set to '0' then the PDF will be empty. 
                streamobj.Position = 0;

                //Close the documents. 
                document.Close(true);

                //Defining the ContentType for pdf file.
                string contentType = "application/pdf";

                //Define the file name.
                string fileName = "Output.pdf";

                //Creates a FileContentResult object by using the file contents, content type, and file name. return File(stream, contentType, fileName);
            }
            return "";
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
