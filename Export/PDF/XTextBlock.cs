using PdfSharp.Drawing;

namespace QTIEditor.Export.PDF
{
    class XTextBlock : IDrawableBlock
    {

        public XBrush brush;

        public string text;

        public XFont font;

        public XPoint Position { get; set; }

        public double Width { get; set; }

        public double Height
        {
            get => SimulateDraw();
        }

        public XTextBlock(string text, XBrush brush, XFont font, XPoint pos, double width)
        {
            this.brush = brush;
            this.text = text;
            this.font = font;
            Position = pos;
            Width = width;
        }

        public XTextBlock(string text, XBrush brush, XFont font, double xPos, double yPos, double width)
        {
            this.brush = brush;
            this.text = text;
            this.font = font;
            Position = new(xPos, yPos);
            Width = width;
        }


        public void Draw(XGraphics gfx)
        {
            var lines = text.Split('\n');

            double heightOffset = 0d;
            foreach (var line in lines)
            {
                DrawLineWrap(line, gfx, ref heightOffset);

            }
        }

        double SimulateDraw()
        {
            var lines = text.Split('\n');

            double heightOffset = 0d;
            XGraphics virtualGfx = XGraphics.CreateMeasureContext(XSize.Empty, XGraphicsUnit.Point, XPageDirection.Downwards);
            foreach (var line in lines)
            {
                DrawLineWrap(line, virtualGfx, ref heightOffset, true);
            }

            return heightOffset;
        }

     
        void DrawLineWrap(string line, XGraphics gfx, ref double heightOffset, bool simulate = false)
        {
            var expectedSize = gfx.MeasureString(line, font);
            if (expectedSize.Width <= Width)
            {
                if (!simulate) gfx.DrawString(line, font, brush, Position + new XVector(0, heightOffset));
                heightOffset += font.GetHeight();
                return;
            }

            double approxCharWidth = expectedSize.Width / line.Length;
            int splitIndex = (int)(Width / approxCharWidth);

            // make the string smaller until we're under our limit
            while (gfx.MeasureString(line[..splitIndex], font).Width > Width)
            {
                splitIndex--;
            }

            // then reverse iterate until a word border is found
            for (int i = splitIndex - 1; i > 0; i--)
            {
                if (char.IsWhiteSpace(line[i]))
                {
                    if (!simulate) gfx.DrawString(line[0..i], font, brush, Position + new XVector(0, heightOffset));
                    heightOffset += font.GetHeight();
                    DrawLineWrap(line[(i+1)..], gfx, ref heightOffset);
                    return;
                }
            }

            // if no char break is found just split the line at the index
            if (!simulate) gfx.DrawString(line[0..splitIndex], font, brush, Position + new XVector(0, heightOffset));
            heightOffset += font.GetHeight();
            DrawLineWrap(line[(splitIndex + 1)..], gfx, ref heightOffset);

        }

    }
}
