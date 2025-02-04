using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media.Media3D;

namespace QTIEditor.QTI.SimpleTypes
{

    /// <summary>
    /// The navigation mode determines the general paths that the candidate may take throught the test.
    /// The data model for the "NavigationMode" enumerated class is shown in Figure 8.12 and the accompanying vocabulary definition in Table 8.12.
    /// </summary>
    public enum NavigationMode
    {
        /// <summary>
        /// A testPart in linear mode restricts the candidate to attempt each item in turn
        /// </summary>
        [XmlEnum("linear")]
        linear,

        /// <summary>
        /// A testPart in nonlinear mode allows the candidate to access an item in any order.
        /// </summary>
        [XmlEnum("nonlinear")]
        nonlinear
    }

    /// <summary>
    /// The submission mode determines when the candidate's responses are submitted for response processing.The data model for the "SubmissionMode"
    /// enumerated class is shown in Figure 8.19 and the accompanying vocabulary definition in Table 8.19.
    /// </summary>
    public enum SubmissionMode
    {

        /// <summary>
        /// A testPart in individual mode requires the candidate to submit their responses on an item-by-item basis.
        /// </summary>
        [XmlEnum("individual")]
        individual,

        /// <summary>
        /// In simultaneous mode the candidate's responses are all submitted together at the end of the testPart.
        /// </summary>
        [XmlEnum("simultaneous")]
        simultaneous

    }

    /// <summary>
    /// To define the direction of the text layout as part of the Bi-direction (bidi) element in HTML.
    /// </summary>
    public enum DIR
    {
        /// <summary>
        /// Allow the rendering system to determine the text layout direction.
        /// </summary>
        [XmlEnum("auto")]
        auto,


        /// <summary>
        /// A left-to-right text diection layout.
        /// </summary>
        [XmlEnum("ltr")]
        leftToRight,

        /// <summary>
        /// A right-to-left text diection layout.
        /// </summary>
        [XmlEnum("rtl")]
        rightToLeft

    }


    /// <summary>
    /// The permitted set of values for the ARIA roles that have been identified as applicable to QTI XML markup.
    /// </summary>
    public enum ARIARoleValue
    {
        /// <summary>
        /// Identifies the role as a section of a page that consists of a composition that forms an independent part of a document, page, or site.
        /// </summary>
        article,

        /// <summary>
        /// Identifies the role as an input that allows for user-triggered actions when clicked or pressed. See related link.
        /// </summary>
        button,

        /// <summary>
        /// Identifies the role as a checkable input that has three possible values: true, false, or mixed.
        /// </summary>
        checkbox,

        /// <summary>
        /// Identifies the role as a cell containing header information for a column.
        /// </summary>
        columnheader,

        /// <summary>
        /// Identifies the role as a supporting section of the document, designed to be complementary to the main content at a similar level in the DOM hierarchy, but remains
        /// meaningful when separated from the main content.
        /// </summary>
        complementary,

        /// <summary>
        /// Identifies the role as a large perceivable region that contains information about the parent document.
        /// </summary>
        contentinfo,

        /// <summary>
        /// Identifies the role as a definition of a term or concept.
        /// </summary>
        definition,

        /// <summary>
        /// Identifies the role as a list of references to members of a group, such as a static table of contents.
        /// </summary>
        directory,

        /// <summary>
        /// Identifies the role as a region containing related information that is declared as document content, as opposed to a web application.
        /// </summary>
        document,

        /// <summary>
        /// Identifies the role as a cell in a grid or treegrid.
        /// </summary>
        gridcell,

        /// <summary>
        /// Identifies the role as a set of user interface objects which are not intended to be included in a page summary or table of contents by assistive technologies.
        /// </summary>
        group,

        /// <summary>
        /// Identifies the role as a heading for a section of the page.
        /// </summary>
        heading,

        /// <summary>
        /// Identifies the role as a container for a collection of elements that form an image.
        /// </summary>
        img,

        /// <summary>
        /// Identifies the role as a an interactive reference to an internal or external resource that, when activated, causes the user agent to navigate to that resource.
        /// See related button.
        /// </summary>
        link,

        /// <summary>
        /// Identifies the role as a group of non-interactive list items.See related listbox.
        /// </summary>
        list,

        /// <summary>
        /// Identifies the role as a widget that allows the user to select one or more items from a list of choices.See related list.
        /// </summary>
        listbox,

        /// <summary>
        /// Identifies the role as a single item in a list or directory.
        /// </summary>
        listitem,

        /// <summary>
        /// Identifies the role as a type of live region where new information is added in meaningful order and old information may disappear.
        /// </summary>
        log,

        /// <summary>
        /// Identifies the role as content that represents a mathematical expression.
        /// </summary>
        math,

        /// <summary>
        /// Identifies the role as a section whose content is parenthetic or ancillary to the main content of the resource.
        /// </summary>
        note,

        /// <summary>
        /// Identifies the role as a selectable item in a select list.
        /// </summary>
        option,

        /// <summary>
        /// Identifies the role as an element whose implicit native role semantics will not be mapped to the accessibility API.
        /// </summary>
        presentation,

        /// <summary>
        /// Identifies the role as a checkable input in a group of radio roles, only one of which can be checked at a time.
        /// </summary>
        radio,

        /// <summary>
        /// Identifies the role as a group of radio buttons.
        /// </summary>
        radiogroup,

        /// <summary>
        /// Identifies the role as a large perceivable section of a web page or document, that is important enough to be included in a page summary or table of contents,
        /// for example, an area of the page containing live sporting event statistics.
        /// </summary>
        region,

        /// <summary>
        /// Identifies the role as a row of cells in a grid.
        /// </summary>
        row,

        /// <summary>
        /// Identifies the role as a group containing one or more row elements in a grid.
        /// </summary>
        rowgroup,

        /// <summary>
        /// Identifies the role as a cell containing header information for a row in a grid.
        /// </summary>
        rowheader,

        /// <summary>
        /// Identifies the role as a divider that separates and distinguishes sections of content or groups of menuitems.
        /// </summary>
        separator,

        /// <summary>
        /// Identifies the role as a user input where the user selects a value from within a given range.
        /// </summary>
        slider,

        /// <summary>
        /// Identifies the role as a form of range that expects the user to select from among discrete choices.
        /// </summary>
        spinbutton,

        /// <summary>
        /// Identifies the role as a container whose content is advisory information for the user but is not important enough to justify an alert,
        /// often but not necessarily presented as a status bar.
        /// </summary>
        status,

        /// <summary>
        /// Identifies the role as a grouping label providing a mechanism for selecting the tab content that is to be rendered to the user.
        /// </summary>
        tab,

        /// <summary>
        /// Identifies the role as a list of tab elements, which are references to tabpanel elements.
        /// </summary>
        tablist,

        /// <summary>
        /// Identifies the role as a container for the resources associated with a tab, where each tab is contained in a tablist.
        /// </summary>
        tabpanel,

        /// <summary>
        /// Identifies the role as an Input that allows free-form text as its value.
        /// </summary>
        textbox,

        /// <summary>
        ///  Identifies the role as a type of live region containing a numerical counter which indicates an amount of elapsed time from a start point, or the time remaining until
        ///  an end point.
        /// </summary>
        timer,

        /// <summary>
        /// Identifies the role as a collection of commonly used function buttons or controls represented in compact visual form.
        /// </summary>
        toolbar
    }

    /// <summary>
    /// The permitted set of values for the aria-level ARIA annotations.
    /// </summary>
    public enum ARIALiveValue
    {
        /// <summary>
        /// Denotes that the ARIA live feature is off (the default setting).
        /// </summary>
        off,

        /// <summary>
        /// Denotes that the content is assertive in nature.
        /// </summary>
        assertive,

        /// <summary>
        /// Denotes that the content is polite in nature.
        /// </summary>
        polite

    }

    /// <summary>
    /// The set of permitted values that indicate the orientation of the associated element.
    /// </summary>
    public enum ARIAOrienationValue
    {
        /// <summary>
        /// Denotes that the orientation of the associated element is horizontal.
        /// </summary>
        horizontal,

        /// <summary>
        /// Denotes that the orientation of the associated element is vertical.
        /// </summary>
        vertical,

    }

    /// <summary>
    /// The set of values for whether or not the associated object should be displayed or hidden.
    /// </summary>
    public enum ShowHide
    {
        /// <summary>
        /// Denotes that the object is originally displayed and hidden depending on the associated trigger condition.
        /// </summary>
        show,

        /// <summary>
        /// Denotes that the object is originally hidden and displayed depending on the associated trigger condition.
        /// </summary>
        hide
    }


    /// <summary>
    /// Contains the permitted set of cardinality values. The cardinality is used in the context of the associated variable.
    /// </summary>
    public enum Cardinality
    {
        /// <summary>
        /// A cardinality of single i.e. a single value.
        /// </summary>
        single,

        /// <summary>
        /// A multi-valued expression (or variable) is called a container. A container contains a list of values, this list may be empty in which case it is treated as NULL.
        /// All the values in a multiple or ordered container are drawn from the same value set, however, containers may contain multiple occurrences of the same value. In other
        /// words, [A,B,B,C] is an acceptable value for a container.
        /// </summary>
        multiple,

        /// <summary>
        /// A container with cardinality multiple and value [A,B,C] is equivalent to a similar one with value [C,B,A] whereas these two values would be considered distinct
        /// for containers with cardinality ordered.
        /// </summary>
        ordered,

        /// <summary>
        /// The record container type is a special container that contains a set of independent values each identified by its own identifier and having its own base-type.
        /// This specification does not make use of the record type directly however it is provided to enable customInteractions to manipulate more complex responses and
        /// customOperators to return more complex values, in addition to the use for detailed information about numeric responses described in the stringInteraction abstract class.
        /// </summary>
        record
    }

    /// <summary>
    /// A base-type is simply a description of a set of atomic values (atomic to this specification). Note that several of the baseTypes used to define the runtime data model
    /// have identical definitions to those of the basic data types used to define the values for attributes in the specification itself. The use of an enumeration to define the
    /// set of baseTypes used in the runtime model, as opposed to the use of classes with similar names, is designed to help distinguish between these two distinct levels of
    /// modelling.
    /// </summary>
    public enum BaseType
    {
        /// <summary>
        /// The set of boolean values is the same as the set of values defined by the boolean primitveType.
        /// </summary>
        boolean,

        /// <summary>
        /// A directedPair value represents a pair of identifiers corresponding to a directed association between two objects. The two identifiers correspond to the
        /// source and destination objects.
        /// </summary>
        directedPair,

        /// <summary>
        /// A duration value specifies a distance (in time) between two time points. In other words, a time period as defined by [ISO 8601], but represented as a single float that
        /// records time in seconds. Durations may have a fractional part. Durations are represented using the xsd:double datatype rather than xsd:dateTime for convenience and backward
        /// compatibility.
        /// </summary>
        duration,

        /// <summary>
        ///  A file value is any sequence of octets (bytes) qualified by a content-type and an optional filename given to the file (for example, by the candidate when uploading
        ///  it as part of an interaction). The content type of the file is one of the MIME types defined by[RFC 2045].
        /// </summary>
        file,

        /// <summary>
        /// The set of float values is the same as the set of values defined by the float primitiveType.
        /// </summary>
        @float,

        /// <summary>
        /// The set of identifier values is the same as the set of values defined by the identifier class.
        /// </summary>
        identifier,

        /// <summary>
        /// The set of integer values is the same as the set of values defined by the integer primitiveType.
        /// </summary>
        integer,

        /// <summary>
        /// A pair value represents a pair of identifiers corresponding to an association between two objects.The association is undirected so (A, B) and (B, A) are equivalent.
        /// </summary>
        pair,

        /// <summary>
        /// A point value represents an integer tuple corresponding to a graphic point.The two integers correspond to the horizontal (x-axis) and vertical(y-axis) positions
        /// respectively.The up/down and left/right senses of the axes are context dependent.
        /// </summary>
        point,

        /// <summary>
        /// The set of string values is the same as the set of values defined by the string primitiveType
        /// </summary>
        @string,

        /// <summary>
        /// A URI value is a Uniform Resource Identifier as defined by [URI, 98].
        /// </summary>
        uri,

    }


    /// <summary>
    /// The intended audience for the associated object can be set with the view attribute.
    /// </summary>
    /// <remarks>
    /// If no view is specified the outcome is treated as relevant to all views. Objects may have content that is of no interest to the candidate at all, but are merely used
    /// to hold intermediate values or other information useful during the item or test session. Where more than one class of user should be able to view the content the view
    /// attribute should contain a comma delimited list.
    /// </remarks>
    public enum View
    {
        /// <summary>
        /// Viewable by an 'author'.
        /// </summary>
        author,

        /// <summary>
        /// Viewable by a 'candidate'.
        /// </summary>
        canidate,

        /// <summary>
        /// Viewable by a 'proctor' (sometimes referred to as an 'invigilator').
        /// </summary>
        proctor,

        /// <summary>
        /// Viewable by a 'scorer'.
        /// </summary>
        scorer,

        /// <summary>
        /// Viewable by a 'testConstructor'.
        /// </summary>
        testConstructor,

        /// <summary>
        /// Viewable by a 'tutor'.
        /// </summary>
        tutor


    }


    /// <summary>
    /// Identifies the set of modes for the exernal scoring of the Item.
    /// </summary>
    public enum ExternalScored
    {
        /// <summary>
        /// An external scoring system will be used to score the Item.
        /// </summary>
        externalMachine,

        /// <summary>
        /// A human must score the Item.
        /// </summary>
        human
    }

}
