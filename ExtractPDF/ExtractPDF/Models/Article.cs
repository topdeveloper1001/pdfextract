using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class Article
    {
        public Article()
        {
            ArticleModifier = new HashSet<ArticleModifier>();
            Association = new HashSet<Association>();
            InverseParent = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public Article Parent { get; set; }
        public ICollection<ArticleModifier> ArticleModifier { get; set; }
        public ICollection<Association> Association { get; set; }
        public ICollection<Article> InverseParent { get; set; }
    }
}
