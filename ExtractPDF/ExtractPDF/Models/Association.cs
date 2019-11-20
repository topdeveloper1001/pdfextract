using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class Association
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int AssociationTypeContentId { get; set; }
        public int AssociationValueContentId { get; set; }
        public int? AssociationValueModifierContentId { get; set; }

        public Article Article { get; set; }
        public ContentValue AssociationTypeContent { get; set; }
        public ContentValue AssociationValueContent { get; set; }
        public ContentValue AssociationValueModifierContent { get; set; }
    }
}
