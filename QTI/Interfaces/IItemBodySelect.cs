using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QTIEditor.QTI.Interfaces
{
    /// <summary>
    /// Within the assessmentItem, this enables the creation of the various ItemBody content structures based upon the 'rubricBlock' and the HTML block content.
    /// </summary>
    /// <remarks>
    /// <b>Implementors:</b><br/>
    /// RubricBlock - Not implemented <br/>
    /// IBlockGroup
    /// 
    /// </remarks>
    public interface IItemBodySelect : IXmlSerializable
    {   
        public ItemBody? Parent { get; internal set; }
    }
}
