using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class Association
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int AssociationTypeId { get; set; }
        public int AssociationValueId { get; set; }

        public Article Article { get; set; }
        public AssociationType AssociationType { get; set; }
        public AssociationValue AssociationValue { get; set; }
    }
}
