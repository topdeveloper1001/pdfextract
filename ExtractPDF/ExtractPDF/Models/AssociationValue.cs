﻿using System;
using System.Collections.Generic;

namespace ExtractPDF.Models
{
    public partial class AssociationValue
    {
        public AssociationValue()
        {
            AssociationValueModifier = new HashSet<AssociationValueModifier>();
        }

        public int Id { get; set; }
        public int AssociationValueContentId { get; set; }
        public int AssociationId { get; set; }

        public Association Association { get; set; }
        public AssociationValueContent AssociationValueContent { get; set; }
        public ICollection<AssociationValueModifier> AssociationValueModifier { get; set; }
    }
}
