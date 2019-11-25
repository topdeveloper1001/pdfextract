using System;
using System.Collections.Generic;
using System.Text;
using ExtractPDF.Helper;
using ExtractPDF.Models;

namespace ExtractPDF
{
    public class ParsingService
    {
        private List<LineDataOfPage> pageData;  // entire page data
        private LineDataOfPage page;    // current processing page
        private Line line;  // current processing line.  
        private string article; // current article. when article is empty, all non-article data will be ignored.
        private string articleEx;   // current extend article.
        private StringBuilder result;   // variable for log.
        private ProcessingColumn left;   //  left column data.
        private ProcessingColumn right;  //  right column data.

        private const string Regular = "ArialMT"; // this font is used in association value.
        private const string Italic = "Arial-ItalicMT";   //  this font is used in association type. and some case of association value modifier.
        private const string Bold = "Arial-BoldItalicMT"; //  this font is used in article extension.
        public ParsingService(List<LineDataOfPage> data)
        {
            //  initializing.
            pageData = data;
            article = "";
            articleEx = "";
            result = new StringBuilder();
            left = InitializeProcessingColumn();
            right = InitializeProcessingColumn();
        }
        private ProcessingColumn InitializeProcessingColumn()
        {
            return new ProcessingColumn
            {
                Type = "",
                Value = "",
                Values = new List<string>(),
                IsModifier = false,
                
            };
        }
        //  process all pages
        public void Process()
        {
            foreach(var pg in pageData)
            {
                page = pg;

                ProcessPage();
            }
            ProcessColumn(left);
            ProcessColumn(right);
        }
        // process current page.
        private void ProcessPage()
        {
            foreach(var ln in page.LineList)
            {
                line = ln;
                ProcessLine();
            }
        }
        //process current line;
        private void ProcessLine()
        {
            //  check whether current line is for article or not.
            if (CheckArticleLine())
            {
                ProcessColumn(left);
                ProcessColumn(right);
                article = GetEntireLineText();

                result.AppendLine($">>Article Added. Article : {article}");

                return;
            }

            // ignoring all non-article lines if article was not set yet.
            if (string.IsNullOrEmpty(article))
                return;

            //  check whether current line is for article extension or not.
            if (CheckExtendArticleLine())
            {
                ProcessColumn(left);
                ProcessColumn(right);
                articleEx = GetEntireLineText();

                result.AppendLine($">>Extend Article Added. Extend Article : {articleEx}, Article : {article}");

                return;
            }
            
            

            // check whether this line is for first column or second column by line's X position.
            if(line.Box.X < 100)
            {
                left = LineDataProcess(left);
            }
            else
            {
                right = LineDataProcess(right);
            }
            
        }

        private ProcessingColumn LineDataProcess(ProcessingColumn column)
        {
            foreach (var word in line.WordList)
            {
                switch (word.Font)
                {
                    case Italic:
                        {
                            if(!column.IsValueEmpty)
                            {
                                if(column.IsModifier == true || word.Text.IndexOf('(') >= 0)    // In case of association value modifier
                                {
                                    column.Value += (column.Value == "" ? "" : " ") + word.Text;
                                    
                                    column.IsModifier = true;
                                    
                                    if (column.Value.IndexOf(')') >= 0)
                                    {
                                        column.IsModifier = false;
                                    }
                                    if (column.IsModifier == false)
                                    {
                                        if (column.Value[column.Value.Length - 1] == ',')
                                        {
                                            column.Values.Add(column.Value.Substring(0, column.Value.Length - 1));
                                            column.Value = "";
                                        }
                                    }
                                    break;
                                }
                                // In case of new association type. should process all association values for original type.
                                column = ProcessColumn(column);

                                column.Type = word.Text;
                                break;
                            }

                            column.Type += " " + word.Text;   // In case of continueing type field.

                            break;
                        }
                    case Regular:
                        {
                            if (column.Type == "")  // In case of association value without assocciation type. this is buggy.
                            {
                                result.AppendLine($"--------------Error is occurred : association value setting without association type. value : {word.Text}");
                                break;
                            }
                            if(column.IsValueEmpty) // In case of type field end and start value field.
                            {
                                if (column.Type[column.Type.Length - 1] == ':')
                                {
                                    column.Type = column.Type.Substring(0, column.Type.Length - 1);
                                    result.AppendLine($">>>> Association Type Added. Type : {column.Type},  Article : {article}");
                                }
                                else
                                {
                                    result.AppendLine($"--------------Error is occurred : wrong association type. type : {word.Text}");
                                }
                            }

                            // In case of association value
                            column.Value += (column.Value == "" ? "" : " ") + word.Text;
                            if(column.Value.IndexOf('(') >= 0)
                            {
                                column.IsModifier = true;
                            }
                            if (column.Value.IndexOf(')') >= 0)
                            {
                                column.IsModifier = false;
                            }
                            if (column.IsModifier == false)
                            {
                                if (column.Value[column.Value.Length - 1] == ',')
                                {
                                    column.Values.Add(column.Value.Substring(0, column.Value.Length - 1));
                                    column.Value = "";
                                }
                            }
                            break;
                        }
                    
                }
            }
            return column;
        }
        private ProcessingColumn ProcessColumn(ProcessingColumn column)
        {
            if (column.IsValueEmpty)
                return column;
            if (column.Value != "")
            {
                column.Values.Add(column.Value);
                column.Value = "";
            }
            foreach (var v in column.Values)
            {
                result.AppendLine($">>>>>> Association Value Added. Value : {v}, Type : {column.Type},  Article : {article}");
            }
            return InitializeProcessingColumn();
        }
        // check whether current line is for article or not.
        // it is checked whether current line is located in center or not.
        private bool CheckArticleLine()
        {
            bool res = false;
            if (line.Box.X < 100)
                return res;
            double guessingWidth = line.Box.X * 2 + line.Box.Width;
            if (guessingWidth > page.PageWidth - 2 && guessingWidth < page.PageWidth + 2)
                res = true;
            return res;
        }
        private bool CheckExtendArticleLine()
        {
            if (line.Font == Bold)
                return true;
            return false;
        }
        private string GetEntireLineText()
        {
            string lineText = "";
            foreach(var wd in line.WordList)
            {
                if (!string.IsNullOrEmpty(lineText))
                    lineText += " ";
                lineText += wd.Text;
            }
            return lineText;
        }
    }
    public class ProcessingColumn
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }
        public bool IsModifier { get; set; }
        public bool IsValueEmpty
        {
            get
            {
                bool isEmpty = false;
                if (Value == "" && Values.Count == 0)
                    return true;
                return isEmpty;
            }
        }
    }
}
