using QTIEditor.QTI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace QTIEditor.QTI.SimpleTypes
{
    public class SimpleAssociableChoice : BaseSequence
    {
        public SimpleAssociableChoice() 
        {
            id = new(typeof(SimpleAssociableChoice));
        }


        /// <summary>
        /// The identifier of the choice. This identifier must not be used by any other choice or item variable.
        /// </summary>
        /// <remarks>
        /// For this class this is a duplicate of BaseSequence's id property.
        /// The reason for this is because this class requires an identifier where basesequence does not.
        /// This implementation of IMSQTIv2 *always* generates identifiers for objects even if they are not referenced. <br/>
        /// This means that: <br/>
        /// - Items will *always* have an identifier, generated on init and so it should be impossible for references to become lost or invalid. <br/>
        /// - Some items will end up having multiple identifiers, which *should* but are not guaranteed to be the same.
        /// </remarks>
        [XmlAttribute]
        public UniqueIdentifier identifier { get => id; }


        /// <summary>
        /// If fixed is 'true' for a choice then the position of this choice within the interaction must not be changed by the delivery engine even if the immediately
        /// enclosing interaction supports the shuffling of choices. If no value is specified then the choice is free to be shuffled. In Item Templates, the visibility of
        /// choices can be controlled by setting the value(s) of an associated template variable during template processing. For information about item templates see Item
        /// Templates (Section 2).
        /// </summary>
        [XmlAttribute("fixed")]
        public bool? isFixed;


        /// <summary>
        /// The identifier of a template variable that must have a base-type of identifier and be either single of multiple cardinality. When the associated interaction
        /// is part of an Item Template the value of the identified template variable is used to control the visibility of the choice. When a choice is hidden it is not
        /// selectable and its content is not visible to the candidate unless otherwise stated.
        /// </summary>
        [XmlAttribute]
        public UniqueIdentifier? templateIdentifier;

        /// <summary>
        /// The showHide characteristic determines how the visibility of the choice is controlled. If set to 'show' then the choice is hidden by default and shown only if the
        /// associated template variable matches, or contains, the identifier of the choice. If set to 'hide' then the choice is shown by default and hidden if the associated
        /// template variable matches, or contains, the choice's identifier.
        /// </summary>
        [XmlAttribute]
        public ShowHide? showHide;

        /// <summary>
        /// This is used to identify the set of entities that can be matched with this choice. This is used to enable the author to constrain the set of objects that can be
        /// matched with the target choice. This is a list of the identifiers for the objects that can be matched with this choice.
        /// </summary>
        [XmlAttribute]
        public List<UniqueIdentifier>? matchGroup;


        /// <summary>
        /// The maximum number of choices this choice may be associated with. If matchMax is 0 then there is no restriction.
        /// </summary>
        [XmlAttribute]
        public required uint matchMax;


        /// <summary>
        /// The minimum number of choices this choice must be associated with to form a valid response. If matchMin is 0 then the candidate is not required to associate
        /// this choice with any others at all. matchMin must be less than or equal to the limit imposed by matchMax.
        /// </summary>
        [XmlAttribute]
        public uint? matchMin;



        [XmlText]
        public required string text;


        // This is an abstract child that is used to enable the inclusion of HTML flow content that includes support for feedback and template-base content.
        // [XmlElement]
        // List<IFlowStaticGroup>? flowStaticGroup


        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            id.WriteXmlAttr("identifier", writer);
            isFixed?.WriteXmlAttr("fixed", writer);
            templateIdentifier?.WriteXmlAttr("templateIdentifier", writer);
            showHide?.WriteXmlAttr("showHide", writer);
            matchGroup?.WriteXmlAttr("matchGroup", writer);
            matchMax.WriteXmlAttr("matchMax", writer);
            matchMin?.WriteXmlAttr("matchMin", writer);

            writer.WriteString(text);
        }



    }
}
