using QTIEditor.QTI.Base;
using QTIEditor.QTI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QTIEditor.QTI.SimpleTypes
{
    /// <summary>
    /// This enables an author to define the prompt for the question. The way in which the prompt is displayed depends upon the rendering system. The prompt should not be
    /// used to contain the actual root of the question.
    /// </summary>
    public class Prompt : BaseSequence
    {

        public Prompt()
        {
            id = new(typeof(Prompt));
        }


        public required string prompt;



        public List<IPromptStaticGroup>? promptStaticGroup;


        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteString(prompt);

            if (promptStaticGroup != null)
            {
                throw new NotImplementedException();
            }
        }

    }
}
