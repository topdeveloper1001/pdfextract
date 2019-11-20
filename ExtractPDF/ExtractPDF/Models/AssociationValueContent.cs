using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class AssociationValueContent
    {
        public AssociationValueContent()
        {
            AssociationValue = new HashSet<AssociationValue>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<AssociationValue> AssociationValue { get; set; }
    }
}
