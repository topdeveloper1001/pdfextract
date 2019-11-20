using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class Association
    {
        public Association()
        {
            AssociationValue = new HashSet<AssociationValue>();
        }

        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int AssociationContentId { get; set; }

        public Article Article { get; set; }
        public AssociationContent AssociationContent { get; set; }
        public ICollection<AssociationValue> AssociationValue { get; set; }
    }
}
