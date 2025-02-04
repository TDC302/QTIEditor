using QTIEditor.QTI.Base;
using QTIEditor.QTI.Interfaces;
using QTIEditor.QTI.SimpleTypes;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.QTI.Interactions
{
    public class OrderInteraction : BasePromptInteraction, IBlockGroup
    {
        public ItemBody? Parent { get; set; }

        public OrderInteraction()
        {
            throw new NotImplementedException();
            id = new UniqueIdentifier(typeof(OrderInteraction));
        }


        
    }
}
