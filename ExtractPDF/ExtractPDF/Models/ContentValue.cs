using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class ContentValue
    {
        public ContentValue()
        {
            ArticleArticleContent = new HashSet<Article>();
            ArticleArticleExtensionContent = new HashSet<Article>();
            ArticleArticleModifierContent = new HashSet<Article>();
            AssociationAssociationTypeContent = new HashSet<Association>();
            AssociationAssociationValueContent = new HashSet<Association>();
            AssociationAssociationValueModifierContent = new HashSet<Association>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<Article> ArticleArticleContent { get; set; }
        public ICollection<Article> ArticleArticleExtensionContent { get; set; }
        public ICollection<Article> ArticleArticleModifierContent { get; set; }
        public ICollection<Association> AssociationAssociationTypeContent { get; set; }
        public ICollection<Association> AssociationAssociationValueContent { get; set; }
        public ICollection<Association> AssociationAssociationValueModifierContent { get; set; }
    }
}
