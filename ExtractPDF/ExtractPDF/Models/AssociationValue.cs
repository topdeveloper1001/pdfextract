using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class AssociationValue
    {
        public AssociationValue()
        {
            Association = new HashSet<Association>();
        }

        public int Id { get; set; }
        public int AssociationValueContentId { get; set; }

        public AssociationValueContent AssociationValueContent { get; set; }
        public ICollection<Association> Association { get; set; }
    }
}
