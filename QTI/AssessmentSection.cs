using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.RefItems;
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
    /// An assessment section groups together individual item references and/or sub-sections. A section can be composed of any hierarchy/combination of items
    /// and sections. A section can only reference an item using an assessmentItemRef object but it may contain or reference other sections. The grouping of the sections/items
    /// depends upon the nature of the parent section i.e. each section can be used for different grouping criteria e.g. organizational, pedagogic, etc.
    /// </summary>
    public class AssessmentSection : ISectionPart
    {

        /// <summary>
        /// The unique identifier for the Section. The identifier of the section or item reference must be unique within the test and must not be the identifier of any
        /// testPart. The identifier is a string of characters that must start with a Letter or an underscore ('_') and contain only Letters, underscores, hyphens ('-'),
        /// period ('.', a.k.a. full-stop), Digits, CombiningChars and Extenders. Identifiers containing the period character are reserved for use in prefixing,
        /// as described in the definition of variable. The character classes Letter, Digit, CombiningChar and Extender are defined in the Extensible Markup Language (XML)
        /// 1.0 (Second Edition) [XML, 00]. Note particularly that identifiers may not contain the colon (':') character. Identifiers should have no more than 32
        /// characters for compatibility with version 1. They are always compared case-sensitively.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public UniqueIdentifier identifier = new(typeof(AssessmentSection));

        /// <summary>
        ///	If a child element is required it must appear (at least once) in the selection. It is in error if a section contains a selection rule that selects fewer child
        /// elements than the number of required elements it contains.
        /// </summary>
        [XmlAttribute]
        public bool? required;

        /// <summary>
        /// If a child element is fixed it must never be shuffled. When used in combination with a selection rule fixed elements do not have their position fixed until after
        /// selection has taken place. For example, selecting 3 elements from {A,B,C,D} without replacement might result in the selection {A,B,C}. If the section is subject to
        /// shuffling but B is fixed then permutations such as {A,C,B} are not allowed whereas permutations like {C,B,A} are.
        /// </summary>
        [XmlAttribute("fixed")]
        public bool? isFixed;


        /// <summary>
        /// The title of the section is intended to enable the section to be selected in situations where the contents of the section are not available, for example when a
        /// candidate is browsing a test. Therefore, delivery engines may reveal the title to candidates at any time during the test but are not required to do so.
        /// </summary>
        [XmlAttribute(DataType = "normalizedString")]
        public required string title;


        /// <summary>
        /// A visible section is one that is identifiable by the candidate. For example, delivery engines might provide a hierarchical view of the test to aid navigation. In such
        /// a view, a visible section would be a visible node in the hierarchy. Conversely, an invisible section is one that is not visible to the candidate - the child elements of
        /// an invisible section appear to the candidate as if they were part of the parent section (or testPart). The visibility of a section does not affect the visibility of
        /// its child elements. The visibility of each section is determined solely by the value of its own visible attribute.
        /// </summary>
        [XmlAttribute]
        public required bool visible = true;


        /// <summary>
        /// An invisible section with a parent that is subject to shuffling can specify whether or not its children, which will appear to the candidate as if they were part of the parent,
        /// are shuffled as a block or mixed up with the other children of the parent section.
        /// </summary>
        [XmlAttribute]
        public bool? keepTogether;


        // *** XML ELEMENTS ***


        // An optional set of conditions evaluated during the test, that determine if the item or section is to be skipped (in nonlinear mode, pre-conditions are ignored).
        // The order of the conditions is significant.
        // LogicSingle preCondition

        // An optional set of rules, evaluated during the test, for setting an alternative target as the next item or section (in nonlinear mode, branch rules are ignored).
        // BranchRule branchRule

        // Parameters used to control the allowable states of each item session (may be overridden at sub-section or item level).
        // ItemSessionControl itemSessionControl

        // Optionally controls the amount of time a candidate is allowed for this item or section.
        // TimeLimits timeLimits

        // The rules used to select which children of the section are to be used for each instance of the test. Each child section has its own selection and ordering
        // rules followed before those of its parent. A child section may shuffle the order of its own children while still requiring that they are kept together when shuffling
        // the parent section.
        // Selection selection

        // The rules used to determine the order in which the children of the section are to be arranged for each instance of the test. Each child section has its own
        // selection and ordering rules followed before those of its parent. A child section may shuffle the order of its own children while still requiring that they are
        // kept together when shuffling the parent section.
        // Ordering ordering

        // Section rubric is presented to the candidate with each item contained (directly or indirectly) by the section. As sections are nestable the rubric presented for each
        // item is the concatenation of the rubric blocks from the top-most section down to the item's immediately enclosing section.
        // RubricBlock rubricBlock

        /// <summary>
        /// This is an abstract attribute that enables the definition of the set of assessmentItemRefs, assessmentSectionRefs and assessmentSections to be included as
        /// children of the parent assessmentSection. Sections group together individual item references and/or sub-sections.
        /// </summary>
        [XmlArray]
        public List<ISectionPart>? sections;


        [XmlIgnore]
        public List<IManifestLinkable>? LinkedFiles
        {
            get
            {
                if (sections == null)
                    return null;

                List<IManifestLinkable> linkedFiles = new(sections.Count);

                foreach(ISectionPart sectionPart in sections)
                {
                    if (sectionPart is AssessmentSection section)
                    {
                        if (section.sections != null)
                        {
                            var secLinks = section.LinkedFiles;
                            if (secLinks != null)
                                linkedFiles.AddRange(secLinks);
                        }
                   
                    } else if (sectionPart is AssessmentItemRef assessmentItemRef)
                    {
                        linkedFiles.Add(assessmentItemRef.referencedItem);
                    }
                }


                if (linkedFiles.Count > 0)
                {
                    return linkedFiles;
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
            identifier.WriteXmlAttr("identifier", writer);
            required?.WriteXmlAttr("required", writer);
            isFixed?.WriteXmlAttr("fixed", writer);
            title.WriteXmlAttr("title", writer);
            visible.WriteXmlAttr("visible", writer);
            keepTogether?.WriteXmlAttr("keepTogether", writer);


            if (sections != null)
            {
                foreach (ISectionPart section in sections)
                {
                    string itemName = section.GetType().Name;
                    char lwr = itemName[0].ToString().ToLower()[0];

                    string newName = lwr + itemName[1..];

                    writer.WriteStartElement(newName);
                    section.WriteXml(writer);
                    writer.WriteEndElement();

                }
            }
        }
    }
}
