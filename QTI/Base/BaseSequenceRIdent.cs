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
    /// The BaseSequenceRIdent class provides the base characteristics (as per the BaseSequence plus 'rident') for some of the QTI interactions.
    /// </summary>
    public abstract class BaseSequenceRIdent : BaseSequence, IRespondable
    {
        // <summary>
        // The response variable associated with the interaction.
        // </summary>
        [XmlIgnore]
        public required ResponseDeclaration Response { get; set; }



        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteAttributeString("responseIdentifier", Response.identifier.ToString());
        }
    }
}
