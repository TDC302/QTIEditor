using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.RefItems
{

    /// <summary>
    /// Items are incorporated into the test by reference and not by direct aggregation. 
    /// </summary>
    /// <remarks>
    /// Note that the identifier of the reference need not have any meaning outside the test. In particular it is not required to be unique in the context of any catalog,
    /// or be represented in the item's metadata. The syntax of this identifier is more restrictive than that of the identifier attribute of the assessmentItem itself.
    /// </remarks> 
    /// 
    public class AssessmentItemRef(AssessmentItem refItem) : ISectionPart
    {
        [XmlIgnore]
        public readonly AssessmentItem referencedItem = refItem;


        /// <summary>
        /// The identifier of the item reference must be unique within the test.
        /// </summary>
        [XmlAttribute]
        public readonly UniqueIdentifier identifier = new(typeof(AssessmentItemRef));


        /// <summary>
        /// If a child element is required it must appear (at least once) in the selection. 
        /// </summary>
        /// <remarks>
        /// It is in error if a section contains a selection rule that selects fewer child elements than the number of required elements it contains.
        /// </remarks>
        [XmlAttribute]
        public bool? required;


        /// <summary>
        /// If a child element is fixed it must never be shuffled. 
        /// </summary>
        /// <remarks>
        /// When used in combination with a selection rule fixed elements do not have their position fixed until after selection has taken place. For example,
        /// selecting 3 elements from {A,B,C,D} without replacement might result in the selection {A,B,C}. If the section is subject to shuffling but B is fixed
        /// then permutations such as {A,C,B} are not allowed whereas permutations like {C,B,A} are.
        /// </remarks>
        [XmlAttribute("fixed")]
        public bool? isFixed;


        /// <summary>
        /// The uri used to refer to the item's file (e.g., elsewhere in the same content package). There is no requirement that this be unique. 
        /// A test may refer to the same item multiple times within a test. Note however that each reference must have a unique identifier.
        /// </summary>
        [XmlAttribute]
        public readonly string href = refItem.identifier.ToString() + ".xml";


        /// <summary>
        /// Items can optionally be assigned to one or more categories. Categories are used to allow custom sets of item outcomes to be aggregated during outcomes processing.
        /// </summary>
        [XmlAttribute("category")]
        public List<UniqueIdentifier>? categories;

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            identifier.WriteXmlAttr("identifier", writer);
            required?.WriteXmlAttr("required", writer);
            isFixed?.WriteXmlAttr("fixed", writer);
            href.WriteXmlAttr("href", writer);
        }
    }
}
