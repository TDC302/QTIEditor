using QTIEditor.QTI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QTIEditor.QTI
{
    /// <summary>
    /// This is the representation for the HTML 'object' tag.
    /// </summary>
    public class QTIObject : BaseSequenceXBase
    {
        public QTIObject()
        {
            id = new(typeof(QTIObject));
        }


        /// <summary>
        /// The data characteristic provides a URI for locating the data associated with the object.
        /// </summary>
        [XmlAttribute]
        public required string data;


        // The Mime-type for the object.
        // public required MimeType type


        /// <summary>
        /// The width of the canvas for the object.
        /// </summary>
        [XmlAttribute]
        public Length? width;


        /// <summary>
        /// The height of the canvas for the object.
        /// </summary>
        [XmlAttribute]
        public Length? height;


        // An abstract attribute that enables the relevant complex content/parameters for the object to be defined.
        // [XmlElement]
        // ObjectFlowGroup? objectFlowGroup

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            width?.WriteXmlAttr("width", writer);
            height?.WriteXmlAttr("height", writer);
            writer.WriteString(data);
        }

    }
}
