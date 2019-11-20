using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class AssociationType
    {
        public AssociationType()
        {
            Association = new HashSet<Association>();
        }

        public int Id { get; set; }
        public int AssociationTypeContentId { get; set; }

        public AssociationTypeContent AssociationTypeContent { get; set; }
        public ICollection<Association> Association { get; set; }
    }
}
