
// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
using System.Collections.Generic;
namespace ExtractPDF.Helper
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    
    public partial class Page
    {

        private PageFlow[] flowField;

        private byte numField;

        private string crop_boxField;

        private string media_boxField;

        private byte rotateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Flow")]
        public PageFlow[] Flow
        {
            get
            {
                return this.flowField;
            }
            set
            {
                this.flowField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte num
        {
            get
            {
                return this.numField;
            }
            set
            {
                this.numField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string crop_box
        {
            get
            {
                return this.crop_boxField;
            }
            set
            {
                this.crop_boxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string media_box
        {
            get
            {
                return this.media_boxField;
            }
            set
            {
                this.media_boxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte rotate
        {
            get
            {
                return this.rotateField;
            }
            set
            {
                this.rotateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PageFlow
    {

        private PageFlowPara[] paraField;

        private byte idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Para")]
        public PageFlowPara[] Para
        {
            get
            {
                return this.paraField;
            }
            set
            {
                this.paraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PageFlowPara
    {

        private PageFlowParaLine[] lineField;

        private byte idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Line")]
        public PageFlowParaLine[] Line
        {
            get
            {
                return this.lineField;
            }
            set
            {
                this.lineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PageFlowParaLine
    {

        private PageFlowParaLineWord[] wordField;

        private string boxField;

        private string styleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Word")]
        public PageFlowParaLineWord[] Word
        {
            get
            {
                return this.wordField;
            }
            set
            {
                this.wordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string box
        {
            get
            {
                return this.boxField;
            }
            set
            {
                this.boxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string style
        {
            get
            {
                return this.styleField;
            }
            set
            {
                this.styleField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class PageFlowParaLineWord
    {

        private string boxField;

        private string styleField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string box
        {
            get
            {
                return this.boxField;
            }
            set
            {
                this.boxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string style
        {
            get
            {
                return this.styleField;
            }
            set
            {
                this.styleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    

}