using System;
using System.Collections.Generic;
using System.Text;
using ExtractPDF.Helper;
using ExtractPDF.Models;
using System.IO;
namespace ExtractPDF
{
    public class ParsingService
    {
        private List<LineDataOfPage> pageData;  // entire page data
        private LineDataOfPage page;    // current processing page
        private Line line;  // current processing line.  
        private ProcessingArticle article; // current article. when article is empty, all non-article data will be ignored.
        private string tempArticle; //  it will be used for multiple-line article names
        private ProcessingColumn left;   //  left column data.
        private ProcessingColumn right;  //  right column data.

        private const string Regular = "ArialMT"; // this font is used in association value.
        private const string Italic = "Arial-ItalicMT";   //  this font is used in association type. and some case of association value modifier.
        private const string Bold = "Arial-BoldItalicMT"; //  this font is used in article extension.
        private const string Continue = "continued";
        private StreamWriter writer;
        public ParsingService(List<LineDataOfPage> data)
        {
            //  initializing.
            pageData = data;
            article = InitializeProcessingArticle();
            tempArticle = "";
            left = InitializeProcessingColumn();
            right = InitializeProcessingColumn();
           
            
        }
        public void Log(string logMessage)
        {
            
            writer.WriteLine(logMessage);
            
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
        private ProcessingArticle InitializeProcessingArticle()
        {
            return new ProcessingArticle
            {
                Article = "",
                Extension = "",
                Modifiers = new List<string>()

            };
        }
        //  process all pages
        public void Process()
        {

            using (writer = File.AppendText("log.txt"))
            {
                Log($"Script Running! ==================={DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}===============");
                foreach (var pg in pageData)
                {
                    page = pg;

                    ProcessPage();
                }
                left = ProcessColumn(left);
                right = ProcessColumn(right);
            }
        }
        // process current page.
        private void ProcessPage()
        {
            foreach(var ln in page.LineList)
            {
                line = ln;
                //  check whether current line is for article or not.
                if (CheckArticleLine())
                {
                    // if current line is article line, then should check it has multi-lines or not. so save it to tempArticle temperarilly.
                    if (tempArticle == "")
                    {
                        //  should process columns when new article start.
                        left = ProcessColumn(left);
                        right = ProcessColumn(right);
                        // save article name to tempArticle.
                        tempArticle = GetEntireLineText();
                    }
                    else
                    {
                        // In case of previous line is article line. so current article is surely multi-line at this point.
                        tempArticle += " " + GetEntireLineText();
                    }
                    
                }
                else
                {
                    if (tempArticle != "")
                    {
                        // In case of article lines were ended at previous line. so should process article.
                        ProcessArticle();
                        tempArticle = "";
                    }
                    
                    //  process non-article line.
                    ProcessLine();
                    
                }
            }
        }
        //process current line;
        private void ProcessLine()
        {
            

            // ignoring all non-article lines if article was not set yet.
            if (string.IsNullOrEmpty(article.Article))
                return;

            //  check whether current line is for article extension or not.
            if (CheckExtendArticleLine())
            {
                left = ProcessColumn(left);
                right = ProcessColumn(right);
                article.Extension = GetEntireLineText();

                Log($">>Extend Article Added. Extend Article : {article.Extension}, Parent Article : {article.Article}");

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
                                    // If find '(' character, then it will be modifier.
                                    column.Value += (column.Value == "" ? "" : " ") + word.Text;
                                    
                                    column.IsModifier = true;
                                    
                                    if (column.Value.IndexOf(')') >= 0)
                                    {
                                        // If find ')' character, then modifier will be ended.
                                        column.IsModifier = false;
                                    }
                                    if (column.IsModifier == false)
                                    {
                                        // If find ',' character, then association value is ended. so add value.
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
                                Log($"--------------Error is occurred : association value setting without association type. value : {word.Text}");
                                break;
                            }
                            if(column.IsValueEmpty) // In case of type field end and start value field.
                            {
                                if (column.Type[column.Type.Length - 1] == ':')
                                {
                                    column.Type = column.Type.Substring(0, column.Type.Length - 1);
                                    Log($">>>> Association Type Added. Type : {column.Type},  Article : {article.Article}");
                                }
                                else
                                {
                                    Log($"--------------Error is occurred : wrong association type. type : {word.Text}");
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
        private void ProcessArticle()
        {
            string art = tempArticle;
            article = InitializeProcessingArticle();
            if(art.IndexOf('(') >= 0)   // In case of there's some modifier or continued for article
            {
                
                string modifier = art.Substring(art.IndexOf('(') + 1, art.IndexOf(')') - art.IndexOf('(') - 1);
                article.Article = art.Substring(0, art.IndexOf('(') - 1);
                if (modifier == Continue)
                {
                    Log($">>Article Continuing. Article : {article.Article}");
                    return;
                }
                else
                {
                    Log($">>Article Added. Article : {article.Article}");
                }
                foreach (var item in modifier.Split(','))
                {
                    article.Modifiers.Add(item);
                    Log($">>Article Modifier Added. Modifier : {item}, Article : {article.Article}");
                }
            }
            else
            {
                article.Article = art;
                Log($">>Article Added. Article : {article.Article}");
            }

            
            

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
                if (v.IndexOf('(') >= 0)    // In case of there's some modifier or continued for association value
                {
                    string modifier = v.Substring(v.IndexOf('(') + 1, v.IndexOf(')') - v.IndexOf('(') - 1);
                    string val = v.Substring(0, v.IndexOf('(') - 1);
                    if (article.Extension == "") // no extension
                    {
                        Log($">>>>>> Association Value Added. Value : {val}, Type : {column.Type}, Article : {article.Article}");
                    }
                    else
                    {
                        // In case of article is extension.
                        Log($">>>>>> Association Value Added. Value : {val}, Type : {column.Type}, Extend Article : {article.Extension}, Parent Article : {article.Article}");
                    }
                    
                    foreach (var item in modifier.Split(','))
                    {
                        if (article.Extension == "") // no extension
                        {
                            Log($">>>>>> Association Value Modifier Added. Modifier : {item}, Value : {val}, Type : {column.Type},  Article : {article.Article}");
                        }
                        else
                        {
                            // In case of article is extension.
                            Log($">>>>>> Association Value Modifier Added. Modifier : {item}, Value : {val}, Type : {column.Type}, Extend Article : {article.Extension}, Parent Article : {article.Article}");
                        }
                    }
                }
                else
                {
                    if (article.Extension == "") // no extension
                    {
                        Log($">>>>>> Association Value Added. Value : {v}, Type : {column.Type}, Article : {article.Article}");
                    }
                    else
                    {
                        Log($">>>>>> Association Value Added. Value : {v}, Type : {column.Type}, Extend Article : {article.Extension}, Parent Article : {article.Article}");
                    }
                }
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
            if (guessingWidth > page.PageWidth - 7 && guessingWidth < page.PageWidth + 7)
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
    public class ProcessingArticle
    {
        public string Article { get; set; }
        public string Extension { get; set; }
        public List<string> Modifiers { get; set; }
    }
}
