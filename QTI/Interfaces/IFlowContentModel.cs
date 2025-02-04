using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.QTI.Interfaces
{
    public interface IFlowContentModel
    {
        string HTML { get; init; }
    }

    public class GenericHTML : IFlowContentModel
    {
        public required string HTML { get; init; }
    }
}
