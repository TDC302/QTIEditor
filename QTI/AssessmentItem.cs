using QTIEditor.QTI.Base;
using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using QTIEditor.QTI.VariableProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI
{

    /// <summary>
    /// An assessment item encompasses the information that is presented to a candidate and information about how to score the item. Scoring takes place when candidate
    /// responses are transformed into outcomes by response processing rules. It is sometimes desirable to have several different items that appear the same to the candidate
    /// but which are scored differently. In this specification, these are distinct items by definition and must therefore have distinct identifiers. To help facilitate the
    /// exchange of items that share significant parts of their presentation this specification supports the inclusion of separately managed item fragments (see Item and Test
    /// Fragments) in the itemBody.
    /// </summary>
    [XmlRoot("assessmentItem", Namespace = Constants.NAMESPACE)]
    public class AssessmentItem : IXmlSerializable, IManifestLinkable
    {
       
        /// <summary>
        /// The principle identifier of the item. This identifier must have a corresponding entry in the item's metadata. See QTI Metadata [QTI, 15b] for more information.
        /// </summary>
        [XmlIgnore]
        public UniqueIdentifier identifier = new(typeof(AssessmentItem));

        /// <summary>
        /// The title of an assessmentItem is intended to enable the item to be selected in situations where the full text of the itemBody is not available, for example
        /// when a candidate is browsing a set of items to determine the order in which to attempt them. Therefore, delivery engines may reveal the title to candidates at
        /// any time but are not required to do so.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public required string title;

        /// <summary>
        /// A human readable label that can be used to describe the Item.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public string? label;

        // The default language for the Item. Natural language identifiers as defined by [RFC 3066].
        // language Language

        /// <summary>
        /// The tool name characteristic allows the tool creating the item to identify itself. Other processing systems may use this information to interpret the content
        /// of application specific data, such as labels on the elements of the item's itemBody.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public const string toolName = Constants.TOOL_NAME;

        /// <summary>
        /// The tool version characteristic allows the tool creating the item to identify its version. This value must only be interpreted in the context of the toolName.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public const string toolVersion = Constants.VERSION;

        /// <summary>
        /// Denotes the adaptive nature of the Item. Items are classified as either Adaptive or Non-adaptive.
        /// </summary>
        [XmlAttribute]
        public bool adaptive = false;

        /// <summary>
        /// Denotes if the Item must be answered within some defined time limit.
        /// </summary>
        [XmlAttribute]
        public required bool timeDependent;


        // *** XML ELEMENTS ***

        /// <summary>
        /// This is the set of response variable declarations that are associated with the Item. Response variables are only relevant within the parent Item and so every
        /// declared variable must be referenced in the corresponding response processing.
        /// </summary>
        [XmlElement]
        private readonly HashSet<ResponseDeclaration> responseDeclarations = [];

        /// <summary>
        /// This is the set of outcome variable declarations that are associated with the Item. Outcome variables are only relevant within the parent Item and so every declared
        /// variable must be referenced in the corresponding outcomes processing.
        /// </summary>
        [XmlElement]
        public readonly HashSet<OutcomeDeclaration> outcomeDeclarations = [];

        // This is the set of template variable declarations that are associated with the Item. Template variables are only relevant within the parent Item and so every declared
        // variable must be referenced in the corresponding template processing.
        // List<TemplateDeclaration> templateDeclarations

        // The response processing rules that are used for this Item. These rules are embedded within the ItemBody.
        // TemplateProcessing templateProcessing

        // The set of identifier references to the stimulus content that should be associated with the Item. Each identifier must resolve to some AssessmentStimulus object that has
        // been associated with the Item.
        // List<AssessmentStimulusRef> assessmentStimulusRefs

        // The set of external style sheets that are associated with the Item. The order of definition is significant.
        // List<StyleSheet> styleSheets

        /// <summary>
        /// The item body for the Item. The itembody contains the text, graphics, media objects and interactions that describe the item's content and information about how it is
        /// structured.
        /// </summary>
        [XmlElement]
        public ItemBody? Body
        {
            get => _itemBody;
            set
            {
                _itemBody = value;
                if (_itemBody != null)
                {
                    _itemBody.parent = this;
                }
            }
        }

        [XmlIgnore]
        private ItemBody? _itemBody;


        /// <summary>
        /// The response processing rules that are used for this Item. These rules may either be embedded within the ItemBody or a reference to some external response processing
        /// template may be supplied. If both embedded rules and a template reference are supplied then the internal rules take precedence.
        /// </summary>
        [XmlElement]
        public ResponseProcessing? responseProcessing;

        // The Modal feedback that is to be shown to the candidate directly following response processing.
        // ModalFeedback modalFeedback

        // The accessibility information that is to be associated with the content in the ItemBody. The structure of this content is defined in [APIP, 14].
        // APIPAccessibility apipAccessibility

        public UniqueIdentifier id => identifier;

        public string type => "imsqti_item_xmlv2p2";

        public string href => identifier + ".xml";

        public List<string> files => [href];

        public List<IManifestLinkable>? dependencies => null;


        private void GetResponses()
        {
            responseDeclarations.Clear();
            if (_itemBody == null)
                return;

            foreach (IItemBodySelect item in _itemBody.items)
            {
                if (item is IRespondable respondable)
                {
                    responseDeclarations.Add(respondable.Response);
                }
            }

            if (responseDeclarations.Count > 0 && responseProcessing == null)
            {
                Console.WriteLine("WARNING: A response was declared but there was no response proccessing template");
            }
        }


        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }
        
        public void WriteXml(XmlWriter writer)
        {
            Helpers.WriteFileHeaders(writer);
            identifier.WriteXmlAttr("identifier", writer);
            title.WriteXmlAttr("title", writer);
            label?.WriteXmlAttr("label", writer);
            toolName.WriteXmlAttr("toolName", writer);
            toolVersion.WriteXmlAttr("toolVersion", writer);
            adaptive.WriteXmlAttr("adaptive", writer);
            timeDependent.WriteXmlAttr("timeDependent", writer);


            writer.WriteComment($"File automatically generated {DateTime.UtcNow} by {Constants.TOOL_NAME} v{Constants.VERSION}");


            GetResponses();
            foreach (ResponseDeclaration response in responseDeclarations)
            {
                writer.WriteStartElement("responseDeclaration");
                response.WriteXml(writer);
                writer.WriteEndElement();
            }
            foreach (OutcomeDeclaration outcome in outcomeDeclarations)
            {
                writer.WriteStartElement("outcomeDeclaration");
                outcome.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (_itemBody != null)
            {
                writer.WriteStartElement("itemBody");
                _itemBody.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (responseProcessing != null)
            {
                writer.WriteStartElement("responseProcessing");
                responseProcessing.WriteXml(writer);
                writer.WriteEndElement();
            }
            
        }


        public void WriteToFile(string fileName)
        {

            XmlSerializer ser = new(typeof(AssessmentItem));

            TextWriter writer = File.CreateText(fileName);
            
            ser.Serialize(writer, this);

            writer.Close();


        }





    }
}
