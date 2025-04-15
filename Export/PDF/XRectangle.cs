using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.Export.PDF
{

    /// <summary>
    /// Container class for XRect
    /// </summary>
    internal class XRectangle : IDrawableBlock
    {
        public double Width => underlyingRect.Width;
        public double Height => underlyingRect.Height;
        
        public XPoint Position
        {
            get => underlyingRect.TopLeft;
            set => underlyingRect.Location = value;
            
        }
        
        public XPen? Stroke { get; set; } = null;
        public XBrush? Fill { get; set; } = null;

        public XRect underlyingRect;


        public XRectangle(double width, double height, XPoint position, XPen? stroke = null, XBrush? fill = null)
        {
            underlyingRect = new(new XSize(width, height));
            Position = position;
            Stroke = stroke;
            Fill = fill;
            
        }

        public void Draw(XGraphics graphics)
        {
            graphics.DrawRectangle(Stroke, Fill, underlyingRect);
        }
    }
}
