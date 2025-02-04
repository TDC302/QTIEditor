using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.QTI.Interfaces
{
    public interface IPromptStaticGroup
    {
    }

    public class Ximport : IPromptStaticGroup
    {
        public required string value;
    }

    public class MathML3 : IPromptStaticGroup
    {
        public required string value;
    }

    public class MathML2 : IPromptStaticGroup 
    {
        public required string value;
    }



}
