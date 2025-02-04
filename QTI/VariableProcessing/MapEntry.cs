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

    /// <summary>
    /// This is a part of the mapping functionality. The map is defined by a set of mapEntries, each of which maps a single value from the source set onto a single float.
    /// </summary>
    public class MapEntry : IXmlSerializable
    {
        /// <summary>
        /// The source value for the mapEntry.
        /// </summary>
        [XmlAttribute]
        public required string mapKey;


        /// <summary>
        /// The mapped value for the MapEntry.
        /// </summary>
        [XmlAttribute]
        public required double mappedValue;

        /// <summary>
        /// Used to control whether or not a mapEntry string is matched case sensitively. The default value is false.
        /// </summary>
        [XmlAttribute]
        public bool? caseSensitive;


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
