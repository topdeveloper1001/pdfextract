using System;
using System.IO;
using System.Text;
using Spire.Pdf;


namespace ExtractPDF
{
    class Program
    {
        static void Main(string[] args)
        {

            //Create a pdf document.
            
            PdfDocument doc = new PdfDocument();
            
            doc.LoadFromFile("Spec Sample Pages.pdf");
            
            StringBuilder buffer = new StringBuilder();
            
            
            foreach (PdfPageBase page in doc.Pages)
            {
            
                buffer.Append(page.ExtractText());
            
               
                
            }
            
            
            doc.Close();
            
            

        }
    }
}
