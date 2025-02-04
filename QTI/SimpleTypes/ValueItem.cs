using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.SimpleTypes
{



    /// <summary>
    /// This class replaces DefaultValue and CorrectResponse
    /// </summary>
    /// <remarks>
    /// Defines the default value of the associated response, outcome and template variable.
    /// This class is used to define, as part of the response declaration, the values(s) for the correct response.
    /// </remarks>
    public class ValueItem : IXmlSerializable
    {
        /// <summary>
        /// A human readable interpretation of the contained value.
        /// </summary>
        [XmlAttribute]
        public string? interpretation;

        /// <summary>
        /// The order of the values is signficant only when the response is of ordered cardinality.
        /// </summary>
        [XmlElement]
        public required List<string> values;

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            interpretation?.WriteXmlAttr("interpretation", writer);

            foreach (string value in values)
            {
                writer.WriteStartElement("value");
                writer.WriteString(value);
                writer.WriteEndElement();
            }
        }
    }
}
