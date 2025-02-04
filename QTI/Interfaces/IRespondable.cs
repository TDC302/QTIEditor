using QTIEditor.QTI.VariableProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.QTI.Interfaces
{
    /// <summary>
    /// This interface defines all items that have a response variable.
    /// </summary>
    public interface IRespondable
    {
        /// <summary>
        /// The response variable for this item.
        /// </summary>
        public ResponseDeclaration Response { get; set; }
    }
}
