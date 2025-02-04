using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI
{

    [XmlInclude(typeof(string))]
    public partial class Length
    {
        [XmlIgnore]
        private const string pattern = "[0-9]+%?";

        [GeneratedRegex(pattern, RegexOptions.Compiled)]
        private static partial Regex Searcher();

        public readonly string value;

        public Length(string value)
        {
            if (Searcher().IsMatch(value))
            {
                this.value = value;
            }
            else 
            {
                throw new ArgumentException($"Length value must be a number with an optional % sign, not: {value}");
            }
        }

        public override string ToString()
        {
            return value;
        }

        public void WriteXmlAttr(string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, value);
        }

    }

    [XmlInclude(typeof(int))]
    public class ARIALevelInteger
    {
        [XmlIgnore] 
        private const int minInclusive = 1;

        public readonly int value;

        public ARIALevelInteger(int value)
        {
            if (value >= minInclusive)
            {
                this.value = value;
            }
            else
            {
                throw new ArgumentException($"{typeof(ARIALevelInteger)} minimum value is {minInclusive}, but recieved {value}");
            }
        }


        public void WriteXmlAttr(string attrName, XmlWriter writer)
        {
            writer.WriteAttributeString(attrName, XmlConvert.ToString(value));
        }
    }

    /// <summary>
    /// A class that can represent a single value of any baseType in variable declarations and result reports. The base-type is defined by the baseType attribute
    /// of the declaration except in the case of variables with record cardinality.
    /// </summary>
    public class Value : IXmlSerializable
    {
        [XmlText]
        public required string value;

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(value);
        }
    }

}
