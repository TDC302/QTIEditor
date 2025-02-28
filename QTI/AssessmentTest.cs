using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI
{

    /// <summary>
    /// An assessment test is a group of assessmentItems with an associated set of rules that determine which of the items the candidate sees, in what order,
    /// and in what way the candidate interacts with them. The rules describe the valid paths through the test, when responses are submitted for response processing
    /// and when (if at all) feedback is to be given. Assessment tests are composed of one or more test parts.
    /// </summary>
    [XmlRoot("assessmentTest", Namespace = Constants.NAMESPACE)]
    public class AssessmentTest : IXmlSerializable, IManifestLinkable
    {
        /// <summary>
        /// The principle identifier of the test. This identifier must have a corresponding entry in the test's metadata. See QTI Metadata and QTI Usage Data for
        /// more information [QTI, 15d], [QTI, 15e].
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public UniqueIdentifier identifier = new(typeof(AssessmentTest));

        /// <summary>
        /// The title of an assessmentTest is intended to enable the test to be selected outside of any test session. Therefore, delivery engines may reveal the title
        /// to candidates at any time, but are not required to do so.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public required string title;

        /// <summary>
        /// The tool name characteristic allows the tool creating the test to identify itself. Other processing systems may use this information to interpret the content
        /// of application specific data, such as labels on the elements of the test rubric.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public const string toolName = Constants.TOOL_NAME;

        /// <summary>
        /// The tool version characteristic allows the tool creating the test to identify its version. This value must only be interpreted in the context of the toolName.
        /// </summary>
        [XmlAttribute]
        public const string toolVersion = Constants.VERSION;

        //outcomeDeclaration

        //timeLimits

        //stylesheet

        /// <summary>
        /// Each test is divided into one or more parts which may in turn be divided into sections, sub-sections and so on. 
        /// </summary>
        /// <remarks>
        /// A testPart represents a major division of the test and is used to control the basic mode parameters that apply to all sections and sub-sections within that part.
        /// </remarks>
        [XmlElement]
        public required List<TestPart> testParts;


        //outcomeProcessing

        //testFeedback


        public UniqueIdentifier id => identifier;

        public string type => "imsqti_test_xmlv2p2";

        public string href => identifier.ToString() + ".xml";

        public List<string> files => [ href ];

        public List<IManifestLinkable>? dependencies 
        { 
            get 
            {
                List<IManifestLinkable> depends = [];
                foreach (TestPart part in testParts)
                {
                    foreach (AssessmentSection item in part.assessmentSections)
                    {
                        var secLinks = item.LinkedFiles;

                        if (secLinks != null)
                            depends.AddRange(secLinks);
                    }


                }

                if (depends.Count > 0)
                {
                    return depends;
                } else
                {
                    return null;
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
            Helpers.WriteFileHeaders(writer);
            identifier.WriteXmlAttr("identifier", writer);
            title.WriteXmlAttr("title", writer);
            toolName.WriteXmlAttr("toolName", writer);
            toolVersion.WriteXmlAttr("toolVersion", writer);

            foreach (TestPart part in testParts)
            {
                writer.WriteStartElement("testPart");   
                part.WriteXml(writer);
                writer.WriteEndElement();
            }
        }


        public void WriteToFile(string fileName)
        {

            XmlSerializer ser = new(typeof(AssessmentTest));
            
            TextWriter writer = File.CreateText(fileName);

            ser.Serialize(writer, this);

            writer.Close();


        }
       








    }
}
