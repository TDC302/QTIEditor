using QTIEditor.QTI.SimpleTypes;
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
    /// The BasePromptInteraction is the base class for the QTI interactions that support a Prompt. This also consists of a set of children characteristics.
    /// </summary>
    public abstract class BasePromptInteraction : BaseSequenceFull
    {
        /// <summary>
        /// This is an optional prompt that can be used to guide the learner. A prompt must not contain any nested interactions. A prompt is NOT the same as the actual question
        /// construct.
        /// </summary>
        [XmlElement]
        public Prompt? prompt;


        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            WriteElements(writer);
            
        }

        public override void WriteElements(XmlWriter writer)
        {
            if (prompt != null)
            {
                writer.WriteStartElement("prompt");
                prompt.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
    }
}
