using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.VariableProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Base
{
    /// <summary>
    /// The BaseSequenceFull class provides the base characteristics for some of the QTI interactions that support the full set of base characteristics.
    /// </summary>
    public abstract class BaseSequenceFull : BaseSequence, IRespondable
    {
        // <summary>
        // The response variable associated with the interaction.
        // </summary>
        //[XmlAttribute]
        //public required UniqueIdentifier responseIdentifier;

        [XmlIgnore]
        public ResponseDeclaration Response { get; set; }


        // An optional URI used to change the base for resolving relative URI for the scope of this object. Particular care needs to be taken when resolving relative URI included as
        // part of an Item Fragment. See Item and Test Fragments (Section 2) for more information.
        // Base? base

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            WriteAttributes(writer);
            
        }

        public override void WriteAttributes(XmlWriter writer)
        {
            base.WriteAttributes(writer);
            writer.WriteAttributeString("responseIdentifier", Response.identifier.ToString());
        }

    }
}
