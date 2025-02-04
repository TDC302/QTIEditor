using QTIEditor.QTI.Base;
using QTIEditor.QTI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Interactions
{

    /// <summary>
    /// An Associate Interaction is a blockInteraction that presents candidates with a number of choices and allows them to create associations between them.
    /// The associateInteraction must be bound to a response variable with base-type pair and either single or multiple cardinality.
    /// </summary>
    public class AssociateInteraction : BasePromptInteraction, IBlockGroup, IShuffleable
    {
        public bool Shuffle { get => shuffle ?? false; set => shuffle = value; }

        public ItemBody? Parent { get; set; }

        public AssociateInteraction() 
        {
            id = new(typeof(AssociateInteraction));
            throw new NotImplementedException();
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





    }
}
