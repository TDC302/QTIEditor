using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI
{
    /// <summary>
    /// The item body contains the text, graphics, media objects and interactions that describe the item's content and information about how it is structured. The body is
    /// presented by combining it with stylesheet information, either explicitly or implicitly using the default style rules of the delivery or authoring system. The body must
    /// be presented to the candidate when the associated item session is in the interacting state. In this state, the candidate must be able to interact with each of the visible
    /// interactions and therefore set or update the values of the associated response variables. The body may be presented to the candidate when the item session is in the closed
    /// or review state. In these states, although the candidate's responses should be visible, the interactions must be disabled so as to prevent the candidate from setting or
    /// updating the values of the associated response variables. Finally, the body may be presented to the candidate in the solution state, in which case the correct values of the
    /// response variables must be visible and the associated interactions disabled. The content model employed by this specification uses many concepts taken directly from
    /// [XHTML, 10]. In effect, this part of the specification defines a profile of XHTML. Only some of the elements defined in XHTML are allowable in an assessmentItem and of those
    /// that are, some have additional constraints placed on their attributes. Only those elements from XHTML that are explicitly defined within this specification can be used. See
    /// XHTML Elements for details. Finally, this specification defines some new elements which are used to represent the interactions and to control the display of Integrated Feedback
    /// and content restricted to one or more of the defined content views.
    /// </summary>
    public class ItemBody : IXmlSerializable
    {
        [XmlIgnore]
        public AssessmentItem? parent;


        /// <summary>
        /// The 'id' of a body element must be unique within the item. This is used to enable reference links for other features e.g. APIP accessibility.
        /// </summary>
        [XmlAttribute]
        public UniqueIdentifier id = new(typeof(ItemBody));


        /// <summary>
        /// Classes can be assigned to individual body elements.Multiple class names can be given.These class names identify the element as being a member of the listed classes.
        /// Membership of a class can be used by authoring systems to distinguish between content objects that are not differentiated by this specification.Typically, this information
        /// is used to apply different formatting based on definitions in an associated stylesheet, but can also be used for user interface designs that go beyond.
        /// </summary>
        [XmlAttribute("class")]
        public List<string>? classes = null;

        // The main language of the element. This characteristic is optional and will usually be inherited from the enclosing element.
        // Language? language

        // The label characteristic provides authoring systems with a mechanism for labelling elements of the content model with application specific data. If an item uses labels
        // then values for the associated toolName and toolVersion attributes must also be provided.

        /// <summary>
        /// Specifies the directionality of the text within the itemBody as a whole.
        /// </summary>
        [XmlAttribute]
        public DIR? dir;



        /// <summary>
        /// This is an abstract attribute that enables the complex content for the Item be be constructed. This content consists of the rubric block(s), HTML block tags and QTI
        /// interactions.
        /// </summary>
        [XmlArray]
        public readonly ObservableCollection<IItemBodySelect> items = [];


        public ItemBody() 
        {
            items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems;
            if (newItems != null)
            {
                foreach (IItemBodySelect addedItem in newItems)
                {
                    addedItem.Parent = this;
                }
            }
        }

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            
            //id.WriteXmlAttr("id", writer);
            classes?.WriteXmlAttr("class", writer);
            dir?.WriteXmlAttr("dir", writer);


            foreach (IItemBodySelect item in items)
            {
                string itemName = item.GetType().Name;
                char lwr = itemName[0].ToString().ToLower()[0];
                
                string newName = lwr + itemName[1..];

                writer.WriteStartElement(newName);
                item.WriteXml(writer);
                writer.WriteEndElement();
                
            }




        }
    }
}
