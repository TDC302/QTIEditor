using QTIEditor.QTI.Base;
using QTIEditor.QTI.Interactions;
using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.Swift;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QTIEditor.QTI.Interfaces
{
    /// <summary>
    /// This is the abstract class that is used to enable the insertion of content including HTML block content and the QTI interactions.
    /// </summary>
    /// <remarks>
    /// <b>Implementors:</b>
    /// <list type="bullet">
    ///    <item><description>positionObjectStage</description></item>
    ///    <item><description>customInteraction</description></item>
    ///    <item><description>drawingInteraction</description></item>
    ///    <item><description>gapMatchInteraction - Not yet implemented</description></item>
    ///    <item><description>matchInteraction - Not yet implemented</description></item>
    ///    <item><description>graphicGapMatchInteraction - Not yet implemented</description></item>
    ///    <item><description>hotspotInteraction - Not yet implemented</description></item>
    ///    <item><description>graphicOrderInteraction - Not yet implemented</description></item>
    ///    <item><description>selectPointInteraction - Not yet implemented</description></item>
    ///    <item><description>graphicAssociateInteraction</description></item>
    ///    <item><description>sliderInteraction - Not yet implemented</description></item>
    ///    <item><description>choiceInteraction</description></item>
    ///    <item><description>mediaInteraction - Not yet implemented</description></item>
    ///    <item><description>hottextInteraction - Not yet implemented</description></item>
    ///    <item><description>orderInteraction - Not yet implemented</description></item>
    ///    <item><description>extendedTextInteraction - Not yet implemented</description></item>
    ///    <item><description>uploadInteraction - Not yet implemented</description></item>
    ///    <item><description>associateInteraction - Not yet implemented</description></item>
    ///    <item><description>feedbackBlock - Not yet implemented</description></item>
    ///    <item><description>templateBlock - Not yet implemented</description></item>
    ///    <item><description>infoControl - Not yet implemented</description></item>
    ///    <item><description>math - Not yet implemented</description></item>
    ///    <item><description>math - Not yet implemented</description></item>
    ///    <item><description>include - Not yet implemented</description></item>
    ///    <item><description><i>blockContentModel</i> - Not yet implemented</description></item>
    /// </list>
    /// </remarks>
    public interface IBlockGroup : IItemBodySelect 
    {

    }


    /// <summary>
    /// This is the content frame for the positionObjectInteraction(s).
    /// </summary>
    public class PositionObjectStage : IBlockGroup
    {
        ItemBody? IItemBodySelect.Parent { get; set; }

        /// <summary>
        /// This is the unique identifier that is used by other tags to reference this tag e.g. by the APIP accessibility annotations [APIP, 14].
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public UniqueIdentifier id = new(typeof(PositionObjectStage));


        /// <summary>
        /// The image to be used as a stage onto which individual positionObjectInteractions allow the candidate to place their objects.
        /// </summary>
        [XmlElement("object")]
        public required QTIObject qtiObject;


        [XmlElement]
        public required List<PositionObjectInteraction> positionObjectInteractions;


        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            id.WriteXmlAttr("id", writer);

            writer.WriteStartElement("object");
            qtiObject.WriteXml(writer);
            writer.WriteEndElement();

            foreach (PositionObjectInteraction interaction in positionObjectInteractions)
            {
                writer.WriteStartElement("postitionObjectInteraction");
                interaction.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }


    /// <summary>
    /// The custom interaction provides an opportunity for extensibility of this specification to include support for interactions not currently documented.
    /// The Portable Custom Interaction specification should be used in conjunction with this class [PCI, 14].
    /// </summary>
    public class CustomInteraction : BaseSequenceFull, IBlockGroup
    {
        public ItemBody? Parent { get; set; }


        public CustomInteraction() 
        {
            id = new(typeof(CustomInteraction));
        }

        /// <summary>
        /// This is an extension point to enable any set of characteristics to be entered to support the definition of the custom interaction(s).
        /// </summary>
        [XmlAttribute("extension")]
        public List<string>? extensionCharacteristics;


        /// <summary>
        /// This is an extension point to enable any set of child tags to be entered. The child tags must be new tags and not the reuse of other tags available in QTI ASI.
        /// Note that this class is used to support the definition of all of the types of custom interactions used in the content.
        /// </summary>
        [XmlElement("extension")]
        public List<IXmlSerializable>? extensions;


        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            extensionCharacteristics?.WriteXmlAttr("extension", writer);

            if (extensions != null)
            {
                foreach (var item in extensions)
                {
                    writer.WriteStartElement("extension");
                    item.WriteXml(writer);
                    writer.WriteEndElement();
                }
            }
        }


    }


    /// <summary>
    /// The drawing interaction allows the candidate to use a common set of drawing tools to modify a given graphical image (the canvas). It must be bound to a response variable
    /// with base-type file and single cardinality. The result is a file in the same format as the original image.
    /// </summary>
    public class DrawingInteraction : BasePromptInteraction, IBlockGroup
    {
        public ItemBody? Parent { get; set; }


        public DrawingInteraction() 
        {
            id = new(typeof(DrawingInteraction));
        }

        [XmlElement("object")]
        public required QTIObject qtiObject;

        

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteStartElement("object");
            qtiObject.WriteXml(writer);
            writer.WriteEndElement();
        }
    }



    public class P : BaseSequenceXBase, IBlockGroup
    {
        public ItemBody? Parent { get; set; }


        public required string value;


        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteRaw(value);
        }
    }
}
