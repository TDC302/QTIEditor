using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.VariableProcessing
{
    
    public class MapResponse : IXmlSerializable
    {
        [XmlAttribute]
        public required UniqueIdentifier identifier;

        public XmlSchema? GetSchema() => null;
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            Helpers.SerializeFields(this, writer);
        }
    }
}
