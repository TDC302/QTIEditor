using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.QTI.Interfaces
{
    /// <summary>
    /// The interface for all objects that have a shuffle option.
    /// </summary>
    internal interface IShuffleable
    {
        /// <summary>
        /// Whether or not to shuffle the item.
        /// </summary>
        public bool Shuffle { get; set; }
    }
}
