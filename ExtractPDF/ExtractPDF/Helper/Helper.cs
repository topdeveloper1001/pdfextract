using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ExtractPDF.Helper
{
    public class Helper
    {
        // do some pre-processing from deserializzed xml data.
        // xml's structure is something like Page->Flow->Line->Word
        // and all data is based on string. so do some pre-processing.
        public static LineDataOfPage ConvertPageToProcessingLineData(Page page)
        {
            LineDataOfPage pageData = new LineDataOfPage();
            List<Line> lst = new List<Line>();

            
            foreach(var flow in page.Flow)
            {
                foreach(var para in flow.Para)
                {
                    foreach(var line in para.Line)
                    {
                        Line ln = new Line();
                        ln.Box = GetBoxData(line.box);  // get line's position and width
                        ln.Font = GetFontData(line.style);  // get font of line.

                        ln.WordList = new List<Word>();

                        foreach(var word in line.Word)
                        {
                            Word wd = new Word();
                            wd.Box = GetBoxData(word.box);  // get word's position and width
                            wd.Font = GetFontData(word.style);  //get word's font.
                            if (string.IsNullOrEmpty(wd.Font))  // if word's font is empty, then it's font will be line's font.
                                wd.Font = ln.Font;
                            wd.Text = word.Value;   // word's text
                            ln.WordList.Add(wd);
                        }

                        lst.Add(ln);


                    }
                }
            }

            lst = lst.OrderByDescending(x => x.Box.Y).ToList(); // sort data by Y position.

            pageData.LineList = lst;

            pageData.PageWidth = GetPageWidth(page.crop_box);   // get page's total width. it will be used to check for article. article line will always be located in center.

            return pageData;
        }

        // get page's width from crop_box string
        public static int GetPageWidth(string value)
        {
            int pagewidth = 0;
            string[] items = value.Split(',');
            if (items.Length > 3)
                pagewidth = Convert.ToInt32(items[2]);

            return pagewidth;
        }
        //  get box data from box string
        public static Box GetBoxData(string value)
        {
            Box box = new Box();
            
            string[] items = value.Split(',');
            if (items.Length < 4)
                return null;
            box.X = Convert.ToDouble(items[0]);
            box.Y = Convert.ToDouble(items[1]);
            box.Width = Convert.ToDouble(items[2]);
            return box;
        }
        //  get font from style string
        public static string GetFontData(string value)
        {
            string font = "";
            if (string.IsNullOrEmpty(value))
                return font;

            string[] items = value.Split(';');
            if (items.Length < 1)
                return font;

            string[] fontvalue = items[0].Split(':');
            if (fontvalue.Length < 2)
                return font;

            font = fontvalue[1];

            return font;
        }
    }
}
