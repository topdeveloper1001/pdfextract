using System;
using System.Collections.Generic;
using System.Text;

namespace ExtractPDF.Helper
{

    public class LineDataOfPage
    {
        public List<Line> LineList { get; set; }
        public int PageWidth { get; set; }
    }
    public class Line
    {
        public Box Box { get; set; }
        public string Font { get; set; }
        public List<Word> WordList { get; set; }
    }

    public class Word
    {
        public Box Box { get; set; }
        public string Font { get; set; }
        public string Text { get; set; }
    }

    public class Box
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }

    }
}
