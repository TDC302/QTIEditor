using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Interfaces
{

    /// <summary>
    /// This is an abstract class that enables the inclusion of the external referenced content or child assessmentSections to be used in an 'assessmentSection'.
    /// 
    /// <para><b>Inheritors:</b> <br/>
    /// Include - not implemented <br/>
    /// AssessmentSection <br/>
    /// AssessmentSectionRef - not implemented<br/>
    /// AssessmentItemRef <br/>
    /// </para>
    /// </summary>

    public interface ISectionPart : IXmlSerializable
    {

    }

    
}
