using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class AssociationValueModifierContent
    {
        public AssociationValueModifierContent()
        {
            AssociationValueModifier = new HashSet<AssociationValueModifier>();
        }

        public int Id { get; set; }
        public string Value { get; set; }

        public ICollection<AssociationValueModifier> AssociationValueModifier { get; set; }
    }
}
