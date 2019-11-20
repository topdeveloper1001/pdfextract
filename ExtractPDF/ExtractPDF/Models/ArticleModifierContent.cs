using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class ArticleModifierContent
    {
        public ArticleModifierContent()
        {
            ArticleModifier = new HashSet<ArticleModifier>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<ArticleModifier> ArticleModifier { get; set; }
    }
}
