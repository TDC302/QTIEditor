using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Base
{
    /// <summary>
    /// The BaseSequenceFull class provides the base characteristics for some of the QTI interactions that support the full set of base characteristics.
    /// </summary>
    public abstract class BaseSequence : IXmlSerializable
    {
        
        
        protected UniqueIdentifier id;


        /// <summary>
        /// The unique identifier assigned to the HTML tag. This must be unique otherwise features such as the APIP accessibilty text cannot be supplied as an alternative.
        /// </summary>
        public UniqueIdentifier ID { 
            get => id; 
            protected set => id = value;
        }




        // Every HTML tag may have a class characteristic specified.If specified, it must have a value that is a set of space-separated tokens representing the various classes to which
        // the element belongs.
        // StringList? class

        // This characteristic specifies the primary language for the element's contents and for any of the element's characteristics that contain text. Its value must be a
        // valid [RFC 3066] language tag, or the empty string. Setting the characteristic to the empty string indicates that the primary language is unknown.
        // Language? language


        /// <summary>
        /// The label characteristic provides authoring systems with a mechanism for labelling elements of the content model with application specific data. If an item uses labels
        /// then values for the associated toolName and toolVersion characteristics must also be provided.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public string? label;


        /// <summary>
        /// Specifies the element's text directionality.
        /// </summary>
        [XmlAttribute]
        public DIR? dir;


        /// <summary>
        /// This is the ARIA role. Roles are defined and described by their characteristics. Characteristics define the structural function of a role, such as what a role is,
        /// concepts behind it, and what instances the role can or must contain.
        /// </summary>
        [XmlAttribute]
        public ARIARoleValue? role;


        /// <summary>
        /// This is a part of the ARIA annotation. This identifies the element (or elements) whose contents or presence are controlled by the current element.
        /// </summary>
        [XmlAttribute("aria-controls")]
        public List<UniqueIdentifier>? aria_controls;


        /// <summary>
        /// This is a part of the ARIA annotation. Identifies the element (or elements) that describes the object.
        /// </summary>
        [XmlAttribute("aria-describedby")]
        public List<UniqueIdentifier>? aria_describedby;


        /// <summary>
        /// This is a part of the ARIA annotation. Identifies the next element (or elements) in an alternate reading order of content which, at the user's discretion, allows assistive
        /// technology to override the general default of reading in document source order.
        /// </summary>
        [XmlAttribute("aria-flowto")]
        public List<UniqueIdentifier>? aria_flowto;


        /// <summary>
        /// This is a part of the ARIA annotation. Defines a string value that labels the current element. See related aria-labelledby. The purpose of aria-label is the same as that
        /// of aria-labelledby. It provides the user with a recognizable name of the object.
        /// </summary>
        [XmlAttribute("aria-label")]
        public string? aria_label;


        /// <summary>
        /// This is a part of the ARIA annotation. Identifies the tag (or tags) that labels the current element. See related aria-label and aria-describedby. The purpose of
        /// aria-labelledby is the same as that of aria-label. It provides the user with a recognizable name of the object. The most common accessibility API mapping for a
        /// label is the accessible name property. If the label text is visible on screen, authors SHOULD use aria-labelledby and SHOULD NOT use aria-label. Use aria-label only
        /// if the interface is such that it is not possible to have a visible label on the screen. As required by the text alternative computation, user agents give precedence to
        /// aria-labelledby over aria-label when computing the accessible name property. The aria-labelledby attribute is similar to aria-describedby in that both reference other
        /// elements to calculate a text alternative, but a label should be concise, where a description is intended to provide more verbose information.
        /// </summary>
        [XmlAttribute("aria-labelledby")]
        public List<UniqueIdentifier>? aria_labelledby;


        /// <summary>
        /// This is a part of the ARIA annotation.Defines the hierarchical level of an element within a structure.This can be applied inside trees to tree items, to
        /// headings inside a document, to nested grids, nested tablists and to other structural items that may appear inside a container or participate in an ownership hierarchy.
        /// The value for aria-level is an integer greater than or equal to 1.
        /// </summary>
        [XmlAttribute("aria-level")]
        public ARIALevelInteger? aria_level;


        /// <summary>
        /// This is a part of the ARIA annotation. Indicates that an element will be updated, and describes the types of updates the user agents, assistive technologies,
        /// and user can expect from the live region. The values of this attribute are expressed in degrees of importance. When regions are specified as polite, assistive
        /// technologies will notify users of updates but generally do not interrupt the current task, and updates take low priority. When regions are specified as assertive,
        /// assistive technologies will immediately notify the user, and could potentially clear the speech queue of previous updates.
        /// </summary>
        [XmlAttribute("aria-live")]
        public ARIALiveValue? aria_live;


        /// <summary>
        /// This is a part of the ARIA annotation. Indicates whether the element and orientation is horizontal or vertical.
        /// </summary>
        [XmlAttribute("aria-orientation")]
        public ARIAOrienationValue? aria_orientation;


        /// <summary>
        /// This is a part of the ARIA annotation. See related aria-controls. The value of the aria-owns attribute is a space-separated list of IDREFS that reference one or more
        /// elements in the document by ID.
        /// </summary>
        [XmlAttribute("aria-owns")]
        public List<UniqueIdentifier>? aria_owns;

        public XmlSchema? GetSchema() => null;

        public virtual void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            //writer.WriteAttributeString("id", id.ToString());

        }

        // This is the HTML5 extension characteristic. A custom data characteristic is a characteristic in no namespace whose name starts with the string
        // 'data-', has at least one character after the hyphen, is XML-compatible, and contains no uppercase ASCII letters.
        // DataHTML5Extension? dataExtension
        public virtual void WriteAttributes(XmlWriter writer)
        {

            label?.WriteXmlAttr("label", writer);
            dir?.WriteXmlAttr("dir", writer);
            role?.WriteXmlAttr("role", writer);

            if (aria_controls != null)
            {
                writer.WriteAttributeString("aria-controls", aria_controls.ToAttrString());
            }


            if (aria_describedby != null)
            {
                writer.WriteAttributeString("aria-describedby", aria_describedby.ToAttrString());
            }

            if (aria_flowto != null)
            {
                writer.WriteAttributeString("aria-flowto", aria_flowto.ToAttrString());
            }

            aria_label?.WriteXmlAttr("aria-label", writer);

            if (aria_labelledby != null)
            {
                writer.WriteAttributeString("aria-labelledby", aria_labelledby.ToAttrString());
            }

            aria_level?.WriteXmlAttr("aria-level", writer);
            aria_live?.WriteXmlAttr("aria-live", writer);
            aria_orientation?.WriteXmlAttr("aria-orientation", writer);

            if (aria_owns != null)
            {
                writer.WriteAttributeString("aria-owns", aria_owns.ToAttrString());
            }
        }

        public virtual void WriteElements(XmlWriter writer)
        {

        }
    }
}
