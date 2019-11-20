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
namespace ExtractPDF
{
    class Program
    {

        private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
        static void Main(string[] args)
        {

            PDFNet.Initialize();

            using (PDFDoc doc = new PDFDoc("Article Dev Data Set.pdf"))
            {
                doc.InitSecurityHandler();
                int pagecount = doc.GetPageCount();
                for(int i = 1; i <= pagecount; i++)
                {
                    Page page = doc.GetPage(i);
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
                        }                        

                    }
                }
                
            }
        }
    }
}
