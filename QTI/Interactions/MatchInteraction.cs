using QTIEditor.QTI.Base;
using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Interactions
{
    /// <summary>
    /// A match interaction is a blockInteraction that presents candidates with two sets of choices and allows them to create associates between pairs of choices in
    /// the two sets, but not between pairs of choices in the same set.
    /// </summary>
    /// <remarks>
    /// Further restrictions can still be placed on the allowable associations using the matchMax characteristic of the choices. The matchInteraction must be bound to
    /// a response variable with base-type directedPair and either single or multiple cardinality.
    /// </remarks>
    public class MatchInteraction : BasePromptInteraction, IBlockGroup, IShuffleable
    {
        public ItemBody? Parent { get; set; }

        public bool Shuffle { get => shuffle ?? false; set => shuffle = value; }

        public MatchInteraction()
        {
            id = new(typeof(MatchInteraction));
        }



        /// <summary>
        /// If the shuffle characteristic is 'true' then the delivery engine must randomize the order in which the choices are initially presented,
        /// subject to the value of the fixed attribute of each choice.
        /// </summary>
        [XmlAttribute]
        public bool? shuffle;


        /// <summary>
        /// The maximum number of associations that the candidate is allowed to make. If maxAssociations is 0 then there is no restriction. 
        /// </summary>
        /// <remarks>
        /// If maxAssociations is greater than 1 (or 0) then the interaction must be bound to a response with multiple cardinality.
        /// </remarks>
        [XmlAttribute]
        public uint? maxAssociations;


        /// <summary>
        /// The minimum number of associations that the candidate is required to make to form a valid response. 
        /// </summary>
        /// <remarks>
        /// If minAssociations is 0 then the candidate is not required to make any associations. minAssociations must be less than or equal to the limit imposed
        /// by maxAssociations.
        /// </remarks>
        [XmlAttribute]
        public uint? minAssociations;


        /// <summary>
        /// The two sets of choices, the first set defines the source choices and the second set the targets.
        /// </summary>
        public required (SimpleMatchSet, SimpleMatchSet) simpleMatchSet;


        public bool IsValid()
        {
            //if (simpleMatchSet.Item1.Count != simpleMatchSet.Item2.Count)
            //{
            //    return false;                
            //}

            return true;
        }


        public override void WriteXml(XmlWriter writer)
        {
            if (!IsValid())
                throw new InvalidOperationException("Match interaction contained invalid data.");

            base.WriteXml(writer);
            shuffle?.WriteXmlAttr("shuffle", writer);
            maxAssociations?.WriteXmlAttr("maxAssociations", writer);
            minAssociations?.WriteXmlAttr("minAssociations", writer);

            writer.WriteStartElement("simpleMatchSet");
            simpleMatchSet.Item1.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement("simpleMatchSet");
            simpleMatchSet.Item2.WriteXml(writer);
            writer.WriteEndElement();


        }

    }
}
