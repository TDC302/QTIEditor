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
    /// This is the ordered set of choices for the match set.
    /// </summary>
    public class SimpleMatchSet : List<SimpleAssociableChoice>, IXmlSerializable
    {
        /// <summary>
        /// The unique identifier to allow links to other structures e.g. alternative APIP accessibility annotations.
        /// </summary>
        [XmlAttribute]
        public UniqueIdentifier? id;

        public SimpleMatchSet() 
        {
        }

        

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            id?.WriteXmlAttr("id", writer);

            foreach (var choice in this)
            {
                writer.WriteStartElement("simpleAssociableChoice");
                choice.WriteXml(writer);
                writer.WriteEndElement();
            }
            
        }
    }
}
