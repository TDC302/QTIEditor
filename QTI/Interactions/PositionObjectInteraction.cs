using QTIEditor.QTI.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Interactions
{
    public class PositionObjectInteraction : BaseSequenceRIdent
    {
        PositionObjectInteraction()
        {
            id = new(typeof(PositionObjectInteraction));
        }

        /// <summary>
        /// The centrePoint attribute defines the point on the image being positioned that is to be treated as the centre as an offset from the top-left corner of the
        /// image in horizontal, vertical order. By default this is the centre of the image's bounding rectangle. The stage on which the image is to be positioned may be
        /// shared amongst several position object interactions and is therefore defined in a class of its own: positionObjectStage.
        /// </summary>
        [XmlAttribute]
        public int? centerPoint;

        /// <summary>
        /// The minimum number of positions that the image must be placed to form a valid response to the interaction. If specified, minChoices must be 1 or greater but must
        /// not exceed the limit imposed by maxChoices.
        /// </summary>
        [XmlAttribute]
        public uint? minChoices
        {
            get => _minChoices;
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("If specified, minChoices must be 1 or more.");
                }
                else if (value > maxChoices && maxChoices != 0)
                {
                    throw new ArgumentException($"minChoices cannot be more than maxChoices, which is {maxChoices}");
                }
                else
                {
                    _minChoices = value;
                }
            }
        }

        [XmlIgnore]
        private uint? _minChoices;


        /// <summary>
        /// The maximum number of positions (on the stage) that the image can be placed. If matchChoices is 0 there is no limit. If maxChoices is greater than 1 (or 0)
        /// then the interaction must be bound to a response with multiple cardinality.
        /// </summary>
        [XmlAttribute]
        public uint? maxChoices
        {
            get => _maxChoices;
            set
            {
                if (value < minChoices)
                {
                    throw new ArgumentException($"maxChoices cannot be less than minChoices, which is {minChoices}");
                }
                else
                {
                    _maxChoices = value;
                }
            }
        }

        [XmlIgnore]
        private uint? _maxChoices = 1;


        /// <summary>
        /// The image, required, to be positioned on the stage by the candidate.
        /// </summary>
        [XmlElement("object")]
        public required QTIObject qtiObject;

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            throw new NotImplementedException();
        }

    }
}
