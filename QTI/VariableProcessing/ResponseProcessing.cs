using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace QTIEditor.QTI.VariableProcessing
{
    /// <summary>
    /// Response processing is the process by which the Delivery Engine assigns outcomes based on the candidate's responses. 
    /// </summary>
    /// <remarks>
    /// The outcomes may be used to provide feedback to the candidate. Feedback is either provided immediately following the end of the candidate's attempt or it is provided at some
    /// later time, perhaps as part of a summary report on the item session. The end of an attempt, and therefore response processing, must only take place in direct response to a
    /// user action or in response to some expected event, such as the end of a test. An item session that enters the suspended state may have values for the response variables that
    /// have yet to be submitted for response processing.
    /// </remarks>
    public class ResponseProcessing : IXmlSerializable
    {
        /// <summary>
        /// If a template identifier is given it may be used to locate an externally defined responseProcessing template. The rules obtained from the external template may be used
        /// instead of the rules defined within the item itself, though if both are given the internal rules are still preferred.
        /// </summary>
        [XmlAttribute]
        public Uri? template;


        /// <summary>
        /// In practice, the template attribute may well contain a URN or the URI of a template stored on a remote web server, such as the standard response processing templates 
        /// defined by this specification.
        /// </summary>
        /// <remarks>
        /// When processing an assessmentItem tools working offline will not be able to obtain the template from a URN or remote URI. The templateLocation attribute provides an
        /// alternative URI, typically a relative URI to be resolved relative to the location of the assessmentItem itself, that can be used to obtain a copy of the
        /// response processing template. If a delivery system is able to determine the correct behaviour from the template identifier alone the templateLocation should be
        /// ignored. For example, a delivery system may have built-in procedures for handling the standard templates defined above.
        /// </remarks>
        [XmlAttribute]
        public Uri? templateLocation;


        public static ResponseProcessing TemplateMatchCorrect()
        {
            return new()
            {
                template = new("http://www.imsglobal.org/question/qti_v2p2/rptemplates/match_correct")
            };
        }

        public static ResponseProcessing TemplateMapResponse()
        {
            return new() {
                template = new("http://www.imsglobal.org/question/qti_v2p2/rptemplates/map_response")
            };
        }

        public XmlSchema? GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            template?.WriteXmlAttr("template", writer);
            templateLocation?.WriteXmlAttr("templateLocation", writer);
        }


        // This is an abstract attribute that provides the selection of the constructs that can be used to compose a response rule group. This allows arbitraily complex
        // response processing rules to be constructed for the processing of the responses to the presented item interactions.
        // [XmlElement]
        // public ResponseRuleGroup responseRuleGroup


    }
}
