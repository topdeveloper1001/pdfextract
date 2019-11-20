using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class AssociationTypeContent
    {
        public AssociationTypeContent()
        {
            AssociationType = new HashSet<AssociationType>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<AssociationType> AssociationType { get; set; }
    }
}
