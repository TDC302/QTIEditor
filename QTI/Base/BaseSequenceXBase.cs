using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Base
{
    /// <summary>
    /// The BaseSequenceXBase class provides the base characteristics (as per the BaseSequence plus 'base') for some of the HTML tags and QTI interactions.
    /// </summary>
    public abstract class BaseSequenceXBase : BaseSequence
    {
        
        // An optional URI used to change the base for resolving relative URI for the scope of this object. Particular care needs to be taken when resolving relative URI included as
        // part of an Item Fragment. See Item and Test Fragments (Section 2) for more information.
        // Base? base

    }
}
