using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.VariableProcessing
{

    /// <summary>
    /// Outcome variables are declared by outcome declarations. 
    /// </summary>
    /// <remarks>
    /// Their value is set either from a default given in the declaration itself or by a responseRule during responseProcessing. Items that declare a numeric outcome variable
    /// representing the candidate's overall performance on the item should use the outcome name 'SCORE' for the variable. SCORE needs to be a float. Items that declare a
    /// maximum score (in multiple response choice interactions, for example) should do so by declaring the 'MAXSCORE' variable. MAXSCORE needs to be a float. Items or
    /// tests that want to make the fact that the candidate scored above a predefined treshold available as a variable should use the 'PASSED' variable. PASSED needs to be a
    /// boolean. At runtime, outcome variables are instantiated as part of an item session. Their values may be initialized with a default value and/or set during responseProcessing.
    /// If no default value is given in the declaration then the outcome variable is initialized to NULL unless the outcome is of a numeric type (integer or float) in which case it
    /// is initialized to 0. Declared outcomes with numeric types should indicate their range of possible values using normalMaximum and normalMinimum, especially if this range
    /// differs from [0,1].
    /// </remarks>
    public class OutcomeDeclaration : IXmlSerializable
    {

        /// <summary>
        /// The identifiers of the built-in session variables are reserved. They are completionStatus, numAttempts and duration. All item variables declared
        /// in an item share the same namespace. Different items have different namespaces.
        /// </summary>
        [XmlAttribute]
        public UniqueIdentifier identifier = new(typeof(OutcomeDeclaration));


        /// <summary>
        /// Each variable is either single valued or multi-valued. Multi-valued variables are referred to as containers and come in ordered, unordered and record types.
        /// </summary>
        /// <remarks>
        /// See the cardinality enumerated vocabulary for more information.
        /// </remarks>
        [XmlAttribute]
        public required Cardinality cardinality;


        /// <summary>
        /// The value space from which the variable's value can be drawn (or in the case of containers, from which the individual values are drawn) is identified with a
        /// baseType.
        /// </summary>
        /// <remarks>
        /// The baseType selects one of a small set of predefined types that are considered to have atomic values within the runtime data model. Variables with 
        /// record cardinality have no base-type. 
        /// </remarks>
        [XmlAttribute]
        public BaseType? baseType;


        /// <summary>
        /// The intended audience for an outcome variable can be set with the view attribute. 
        /// </summary>
        /// <remarks>
        /// If no view is specified the outcome is treated as relevant to all views. Complex items, such as adaptive items or complex templates, may declare outcomes
        /// that are of no interest to the candidate at all, but are merely used to hold intermediate values or other information useful during the item or test session.
        /// Such variables should be declared with a view of author (for item outcomes) or testConstructor (for test outcomes). Systems may exclude outcomes from result
        /// reports on the basis of their declared view if appropriate. Where more than one class of user should be able to view an outcome variable the view attribute should
        /// contain a comma delimited list.
        /// </remarks>
        [XmlAttribute]
        public View? view;


        /// <summary>
        /// A human interpretation of the variable's value.
        /// </summary>
        [XmlAttribute]
        public string? interpretation;


        /// <summary>
        /// An optional link to an extended interpretation of the outcome variable's value.
        /// </summary>
        [XmlAttribute]
        public Uri? longInterpretation;


        // The normalMaximum characteristic optionally defines the maximum magnitude of numeric outcome variables, it must be a positive value. If given, the outcome's
        // value can be divided by normalMaximum and then truncated (if necessary) to obtain a normalized score in the range [-1.0,1.0]. normalMaximum has no affect on
        // responseProcessing or the values that the outcome variable itself can take.
        // [XmlAttribute]
        // public NonNegativeDouble? normalMaximum;


        // The normalMinimum characteristic optionally defines the minimum value of numeric outcome variables, it may be negative.
        // [XmlAttribute]
        // public double? normalMinimum;


        // The masteryValue characteristic optionally defines a value for numeric outcome variables above which the aspect being measured is considered to have been mastered by
        // the candidate.
        // [XmlAttribute]
        // public double? masteryValue;


        // This identifies whether or not the value for this outcome is produced by human and machine scoring.
        //[XmlAttribute]
        //public ExternalScored? externalScored;


        // This is the identifier for an external variable that will be used to provide the external scoring value.
        // [XmlAttribute]
        // UniqueIdentifier? variableIdentifierRef; 

        /// <summary>
        /// The default outcome value to be used when no matching tabel entry is found. If omitted, the NULL value is used.
        /// </summary>
        [XmlElement]
        public ValueItem? defaultValue;


        // An abstract attribute to create a lookup table from a numeric source value to a single outcome value in the declared value set.
        // A lookup table works in the reverse sense to the similar mapping as it defines how a source numeric value is transformed into the outcome value,
        // whereas a (response) mapping defines how the response value is mapped onto a target numeric value.
        // [XmlElement]
        // LookupTable? lookupTable;

        public OutcomeDeclaration() { }


        public static OutcomeDeclaration TemplateScore()
        {
            return new()
            {
                cardinality = Cardinality.single,
                baseType = BaseType.@float,
                defaultValue = new() { values = ["0"] },
                identifier = new() { ID = "SCORE" },
            };
        }

        public static OutcomeDeclaration MaxScore(float val)
        {
            return new()
            {
                cardinality = Cardinality.single,
                baseType = BaseType.@float,
                defaultValue = new() { values = [val.ToString()] },
            };
        }



        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            identifier.WriteXmlAttr("identifier", writer);
            cardinality.WriteXmlAttr("cardinality", writer);
            baseType?.WriteXmlAttr("baseType", writer);
            view?.WriteXmlAttr("view", writer);
            interpretation?.WriteXmlAttr("interpretation", writer);
            longInterpretation?.WriteXmlAttr("longInterpretation", writer);

            if (defaultValue != null)
            {
                writer.WriteStartElement("defaultValue");
                defaultValue.WriteXml(writer);
                writer.WriteEndElement();
            }
        }



    }
}
