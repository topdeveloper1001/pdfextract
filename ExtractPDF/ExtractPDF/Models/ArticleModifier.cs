using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class ArticleModifier
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int ArticleModifierContentId { get; set; }

        public Article Article { get; set; }
        public ArticleModifierContent ArticleModifierContent { get; set; }
    }
}
