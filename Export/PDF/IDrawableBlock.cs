using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.Export.PDF
{
    internal interface IDrawableBlock
    {

        /// <summary>
        /// The width of the object in points
        /// </summary>
        public double Width { get; }
        

        /// <summary>
        /// The height of the object in points
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// The position of the top left corner of the object relative to its parent
        /// </summary>
        public XPoint Position { get; set; }

        /// <summary>
        /// Draw the object to graphics
        /// </summary>
        /// <param name="graphics">The canvas to draw to</param>
        public void Draw(XGraphics graphics);


    }
}
