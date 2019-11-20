using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class Article
    {
        public Article()
        {
            Association = new HashSet<Association>();
        }

        public int Id { get; set; }
        public int ArticleContentId { get; set; }
        public int? ArticleExtensionContentId { get; set; }
        public int? ArticleModifierContentId { get; set; }

        public ContentValue ArticleContent { get; set; }
        public ContentValue ArticleExtensionContent { get; set; }
        public ContentValue ArticleModifierContent { get; set; }
        public ICollection<Association> Association { get; set; }
    }
}
