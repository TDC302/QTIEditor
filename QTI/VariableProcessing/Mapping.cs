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
    /// A special class used to create a mapping from a source set of any baseType (except file and duration) to a single float. 
    /// </summary>
    /// <remarks>
    /// Note that mappings from values of base type float should be avoided due to the difficulty of matching floating point values, see the match operator for more details.
    /// When mapping containers the result is the sum of the mapped values from the target set. See the MapResponse class for details.
    /// </remarks>
    public class Mapping : IXmlSerializable
    {
        /// <summary>
        /// The lower bound for the result of mapping a container. If unspecified there is no lower-bound.
        /// </summary>
        [XmlAttribute]
        public double? lowerBound;


        /// <summary>
        /// The lower bound for the result of mapping a container. If unspecified there is no upper-bound.
        /// </summary>
        [XmlAttribute] 
        public double? upperBound;


        /// <summary>
        /// The default value from the target set to be used when no explicit mapping for a source value is given.
        /// </summary>
        [XmlAttribute]
        public double? defaultValue;


        /// <summary>
        /// The map is defined by a set of mapEntries, each of which maps a single value from the source set onto a single float.   
        /// </summary>
        [XmlArray]
        public required List<MapEntry> mapEntries;

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
