using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.VariableProcessing
{

    /// <summary>
    /// Response variables are declared by response declarations and bound to interactions in the itemBody. Each response variable declared may be bound to one and only one
    /// interaction. At runtime, response variables are instantiated as part of an item session. Their values are always initialized to NULL (no value) regardless of whether
    /// or not a default value is given in the declaration. A response variable with a NULL value indicates that the candidate has not offered a response, either because they
    /// have not attempted the item at all or because they have attempted it and chosen not to provide a response. If a default value has been provided for a response variable
    /// then the variable is set to this value at the start of the first attempt. If the candidate never attempts the item, in other words, the item session passes straight from
    /// the initial state to the closed state without going through the interacting state, then the response variable remains NULL and the default value is never used.
    /// </summary>
    public class ResponseDeclaration : IXmlSerializable
    {


        /// <summary>
        /// The identifiers of the built-in session variables are reserved. They are completionStatus, numAttempts and duration. All item variables declared in an item share
        /// the same namespace. Different items have different namespaces.
        /// </summary>
        [XmlIgnore]
        public UniqueIdentifier identifier = new(typeof(ResponseDeclaration));


        /// <summary>
        /// Each variable is either single valued or multi-valued. Multi-valued variables are referred to as containers and come in ordered, unordered and record types.
        /// See cardinality for more information.
        /// </summary>
        [XmlAttribute]
        public required Cardinality cardinality;


        /// <summary>
        /// The value space from which the variable's value can be drawn (or in the case of containers, from which the individual values are drawn) is identified with a baseType.
        /// The baseType selects one of a small set of predefined types that are considered to have atomic values within the runtime data model. Variables with record cardinality
        /// have no base-type.
        /// </summary>

        [XmlAttribute]
        public BaseType? baseType;


        /// <summary>
        /// A response declaration may assign an optional correctResponse. This value may indicate the only possible value of the response variable to be considered correct or
        /// merely just a correct value. For responses that are being measured against a more complex scale than correct/incorrect this value should be set to the (or an) optimal
        /// value. Finally, for responses for which no such optimal value is defined the correctResponse must be omitted. If a delivery system supports the display of a solution
        /// then it should display the correct values of responses (where defined) to the candidate. When correct values are displayed they must be clearly distinguished from the
        /// candidate's own responses (which may be hidden completely if necessary).
        /// </summary>
        [XmlElement]
        public ValueItem? correctResponse;


        /// <summary>
        /// An optional default value for the variable.The point at which a variable is set to its default value varies depending on the type of item variable.
        /// </summary>

        [XmlElement]
        public ValueItem? defaultValue;


        /// <summary>
        /// The mapping provides a mapping from the set of base values to a set of numeric values for the purposes of response processing. See the MapResponse class for
        /// information on how to use the mapping.
        /// </summary>
   
        [XmlElement]
        public Mapping? mapping;


        // The areaMapping, which may only be present in declarations of variables with baseType point, provides an alternative form of mapping which tests against areas
        // of the coordinate space instead of mapping single values i.e. single points.
        // [XmlElement]
        // AreaMapping? areaMapping

        public static ResponseDeclaration TemplateCorrectResponse(UniqueIdentifier res) => TemplateCorrectResponse([res]);

        public static ResponseDeclaration TemplateCorrectResponse(IEnumerable<UniqueIdentifier> correctChoiceIdentifiers)
        {
            List<string> resps = [];
            foreach (var identifier in correctChoiceIdentifiers)
            {
                resps.Add(identifier.ToString());
            }
            if (resps.Count == 0)
            {
                throw new InvalidOperationException("Attempted to construct a response with no values!");
            }

            return new()
            {
                cardinality = resps.Count > 1 ? Cardinality.multiple : Cardinality.single,
                baseType = BaseType.identifier,
                correctResponse = new() { values = resps },
                identifier = new() { ID = "RESPONSE" }
            };
        }

        
        public static ResponseDeclaration TemplateDirectedPairResponse(IEnumerable<(UniqueIdentifier, UniqueIdentifier)> correctChoiceIdentifierPairs)
        {
            List<string> pairs = [];
            foreach (var pair in correctChoiceIdentifierPairs) 
            {
                pairs.Add(pair.Item1.ToString() + " " + pair.Item2.ToString());
            }

            return new()
            {
                cardinality = pairs.Count > 1 ? Cardinality.multiple : Cardinality.single,
                baseType = BaseType.directedPair,
                correctResponse = new() { values = pairs },
                identifier = new() { ID = "RESPONSE" }
            };
        }

        /// <summary>
        /// Applies a mapping to this Respnse Declaration such that when all correct values are matched the mapping will equal total points.
        /// </summary>
        /// <remarks>
        /// For example, a Response Declaration that declares three correct answers would make the value of each correct answer equal to one-third the total points.
        /// </remarks>
        /// <param name="totalPoints">The number of total points that are expected.</param>
        public void ApplyMappingEven(double totalPoints)
        {
            if (correctResponse is null)
                throw new NullReferenceException("Cannot apply mapping to a Response Declaration that does not declare any correct responses");

            double pointsPerResponse = totalPoints / correctResponse.values.Count;

            List<MapEntry> entries = [];
            foreach (string correctResponse in correctResponse.values)
            {
                entries.Add(new() 
                { 
                    mapKey = correctResponse,
                    mappedValue = pointsPerResponse
                });
            }

            mapping = new()
            {
                defaultValue = 0,
                mapEntries = entries
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


            if (correctResponse != null)
            {
                writer.WriteStartElement("correctResponse");
                correctResponse.WriteXml(writer);
                writer.WriteEndElement();
            }


            if (defaultValue != null)
            {
                writer.WriteStartElement("defaultValue");
                defaultValue.WriteXml(writer);
                writer.WriteEndElement();
            }


            if (mapping != null)
            {
                writer.WriteStartElement("mapping");
                mapping.WriteXml(writer);
                writer.WriteEndElement();
            }


        }
    }
}
