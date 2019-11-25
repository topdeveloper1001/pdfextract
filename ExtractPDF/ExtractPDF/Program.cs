using System;
using System.IO;
using System.Text;

using System.Drawing;
using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace ExtractPDF
{
    class Program
    {

        private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
        static void Main(string[] args)
        {
            
            PDFNet.Initialize();

            // it is container of data. it will be used in parsing engine.
            List<Helper.LineDataOfPage> lstPages = new List<Helper.LineDataOfPage>();

            using (PDFDoc doc = new PDFDoc("Article Dev Data Set.pdf"))
            //using (PDFDoc doc = new PDFDoc("Spec Sample Pages.pdf"))
            {
                doc.InitSecurityHandler();
                int pagecount = doc.GetPageCount();
                
                for (int i = 1; i <= pagecount; i++)
                {
                    Page page = doc.GetPage(i);  //Get Page Data
                    if (page == null)
                    {
                        Console.WriteLine("Page not found.");
                        continue;
                    }
                    using (TextExtractor txt = new TextExtractor())
                    {
                        txt.Begin(page);  // Read the page.

                        // Get XML Content from the page
                        String text = txt.GetAsXML(TextExtractor.XMLOutputFlags.e_words_as_elements | TextExtractor.XMLOutputFlags.e_output_bbox | TextExtractor.XMLOutputFlags.e_output_style_info);
                        XmlSerializer serializer = new XmlSerializer(typeof(Helper.Page));
                        Helper.Page pg;
                        
                        using (TextReader reader = new StringReader(text))
                        {
                            // Deserializing XML content.
                            pg = (Helper.Page)serializer.Deserialize(reader);
                            // add pre-processed data to container.
                            lstPages.Add(Helper.Helper.ConvertPageToProcessingLineData(pg));
                        }                        

                    }
                }
                
            }

            // Run parsing module.
            ParsingService ps = new ParsingService(lstPages);
            ps.Process();

        }

    }
}
