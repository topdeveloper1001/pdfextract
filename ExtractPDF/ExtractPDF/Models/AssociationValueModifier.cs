using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class AssociationValueModifier
    {
        public int Id { get; set; }
        public int AssociationValueId { get; set; }
        public int AssociationValueModifierContentId { get; set; }

        public AssociationValue AssociationValue { get; set; }
        public AssociationValueModifierContent AssociationValueModifierContent { get; set; }
    }
}
