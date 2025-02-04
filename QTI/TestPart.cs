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

namespace QTIEditor.QTI
{
    /// <summary>
    /// A test is composed of one or more test parts. A testPart represents a major division of the test and is used to control the basic mode parameters that apply to
    /// all sections and sub-sections within that part.
    /// </summary>
    public class TestPart : IXmlSerializable
    {


        /// <summary>
        /// The identifier of the test part must be unique within the test and must not be the identifier of any assessmentSection or assessmentItemRef.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public UniqueIdentifier identifier = new(typeof(TestPart));


        /// <summary>
        ///	The navigation mode determines the general paths that the candidate may take. A testPart in linear mode restricts the candidate to attempt
        ///	each item in turn. </summary>
        ///	<remarks>
        ///	Once the candidate moves on they are not permitted to return. A testPart in nonlinear mode removes this restriction - 
        /// the candidate is free to navigate to any item in the test at any time. Test delivery systems are free to implement their own user interface elements
        /// to facilitate navigation provided they honour the navigation mode currently in effect. A test delivery system may implement nonlinear mode simply by providing
        /// a method to step forward or backwards through the test part.
        /// </remarks>
        [XmlAttribute]
        public required NavigationMode navigationMode;


        /// <summary>
        /// The submission mode determines when the candidate's responses are submitted for response processing. A testPart in individual mode requires the candidate
        /// to submit their responses on an item-by-item basis.
        /// </summary>
        /// <remarks>
        /// In simultaneous mode the candidate's responses are all submitted together at the end of the testPart.
        /// The choice of submission mode determines the states through which each item's session can pass during the test. In simultaneous mode, response processing cannot
        /// take place until the testPart is complete so each item session passes between the interacting and suspended states only. By definition the candidate can take one and
        /// only one attempt at each item and feedback cannot be seen during the test. For this reason, adaptive items are not compatible with the simultaneous mode. Whether or
        /// not the candidate can return to review their responses and/or any item-level feedback after the test, is outside the scope of this specification. Simultaneous mode is
        /// typical of paper-based tests. In individual mode response processing may take place during the test and the item session may pass through any of the states described in
        /// Items, subject to the itemSessionControl settings in force. Care should be taken when designing user interfaces for systems that support nonlinear navigation mode in
        /// combination with individual submission. With this combination candidates may change their responses for an item and then leave it in the suspended state by navigating to
        /// a different item in the same part of the test. Test delivery systems need to make it clear to candidates that there are unsubmitted responses (akin to unsaved changes in
        /// a traditional document editing system) at the end of the test part. A test delivery system may force candidates to submit or discard such responses before moving to a
        /// different item in individual mode if this is more appropriate.
        /// </remarks>
        [XmlAttribute]
        public required SubmissionMode submissionMode;


        // *** XML ELEMENTS ***

        // An optional set of conditions evaluated during the test, that determine if this part is to be skipped.
        // LogicSingle preCondition

        // An optional set of rules, evaluated during the test, for setting an alternative target as the next part of the test.
        // BranchRule branchRule

        // Parameters used to control the allowable states of each item session in this part. These values may be overridden at section and item level.
        // ItemSessionControl itemSessionControl

        // Optionally controls the amount of time a candidate is allowed for this part of the test.
        // TimeLimits timeLimits

        /// <summary>
        /// This is an abstract class that allows the set of child items and sections to be identified. 
        /// The items contained in each testPart are arranged into sections and sub-sections.
        /// </summary>
        [XmlArray]
        public required List<AssessmentSection> assessmentSections;



        // Test-level feedback specific to this part of the test.
        // TestFeedback testFeedback


        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            identifier.WriteXmlAttr("identifier", writer);
            navigationMode.WriteXmlAttr("navigationMode", writer);
            submissionMode.WriteXmlAttr("submissionMode", writer);

            foreach (AssessmentSection section in assessmentSections)
            {
                writer.WriteStartElement("assessmentSection");
                section.WriteXml(writer);
                writer.WriteEndElement();
            }
        }



    }
}
