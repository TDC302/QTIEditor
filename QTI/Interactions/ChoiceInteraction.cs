using QTIEditor.QTI.Base;
using QTIEditor.QTI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Xml;
using QTIEditor.QTI.SimpleTypes;
using QTIEditor.QTI.VariableProcessing;

namespace QTIEditor.QTI.Interactions
{


    public class ChoiceInteraction : BasePromptInteraction, IBlockGroup, IShuffleable
    {
        public ItemBody? Parent { get; set; }

        public bool Shuffle { get => shuffle ?? false; set => shuffle = value; }

        public ChoiceInteraction()
        {
            id = new(typeof(ChoiceInteraction));
        }



        /// <summary>
        /// If the shuffle characteristic is 'true' then the delivery engine must randomize the order in which the choices are initially presented,
        /// subject to the value of the fixed attribute of each choice.
        /// </summary>
        [XmlAttribute]
        public bool? shuffle;


        /// <summary>
        /// The maximum number of choices that the candidate is allowed to select. If maxChoices is 0 then there is no restriction. If maxChoices is greater than 1
        /// (or 0) then the interaction must be bound to a response with multiple cardinality.
        /// </summary>
        [XmlAttribute]
        public uint? maxChoices;


        /// <summary>
        /// The minimum number of choices that the candidate is required to select to form a valid response.If minChoices is 0 then the candidate is not required to
        /// select any choices.minChoices must be less than or equal to the limit imposed by maxChoices.
        /// </summary>
        [XmlAttribute]
        public uint? minChoices;


        /// <summary>
        /// The orientation characteristic provides a hint to rendering systems that the choices have an inherent vertical or horizontal interpretation.
        /// </summary>
        [XmlAttribute]
        public Orientation? orientation;


        /// <summary>
        /// This is an abstract child that is used to enable the inclusion of HTML flow content that includes support for feedback and template-base content.
        /// </summary>
        [XmlArray]
        public required List<SimpleChoice> choices;

        
        public int CorrectChoiceIndex
        {
            set
            {
                var correctChoice = choices[value];
                Response = ResponseDeclaration.TemplateCorrectResponse(correctChoice.identifier);
                minChoices = null;
                maxChoices = 1;
                minChoices = 1;
            }

        }

        public bool IsValid()
        {
            if (maxChoices != 1 && Response.cardinality != Cardinality.multiple)
            {
                return false;
            }
            else if (minChoices > maxChoices)
            {
                return false;
            }

            return true;
        }


        public override void WriteXml(XmlWriter writer)
        {
            if (!IsValid())
            {
                throw new InvalidOperationException("Choice contained invalid data.");
            }
            
            base.WriteAttributes(writer);
            shuffle?.WriteXmlAttr("shuffle", writer);
            maxChoices?.WriteXmlAttr("maxChoices", writer);
            minChoices?.WriteXmlAttr("minChoices", writer);
            orientation?.WriteXmlAttr("orientation", writer);

            base.WriteElements(writer);

            foreach (SimpleChoice choice in choices)
            {
                writer.WriteStartElement("simpleChoice");
                choice.WriteXml(writer);
                writer.WriteEndElement();
            }

        }
    }



}
