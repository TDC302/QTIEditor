using QTIEditor.QTI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor
{
    interface IInteractionControl
    {

        /// <summary>
        /// A string representing how this item should be called when presented to users
        /// </summary>
        public static string FriendlyName { get; }

        public AssessmentItem ToQTIAssessmentItem();  

       
    }

}
