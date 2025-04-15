using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTIEditor.Export.PDF
{

    /// <summary>
    /// Groups graphics objects together so that they can be reused or logically divided
    /// </summary>
    internal class XArrangementBlock : List<IDrawableBlock>, IDrawableBlock
    {
        public double Width
        {
            get
            {
                double edgeDist = 0;
                foreach (IDrawableBlock block in this)
                {
                    edgeDist = double.Max(edgeDist, block.Position.X + block.Width);
                }
                return edgeDist;
            }
        }
        public double Height
        {
            get {
                double edgeDist = 0;
                foreach (IDrawableBlock block in this)
                {
                    edgeDist = double.Max(edgeDist, block.Position.Y + block.Height);
                }
                return edgeDist;
            }
        }


        public XPoint Position { get; set; }


        public void Draw(XGraphics graphics)
        {
            foreach (IDrawableBlock item in this)
            {
                item.Position = new(Position.X + item.Position.X, Position.Y + item.Position.Y);
                item.Draw(graphics);
                item.Position = new(Position.X - item.Position.X, Position.Y - item.Position.Y);
            }
        }
    }
}
