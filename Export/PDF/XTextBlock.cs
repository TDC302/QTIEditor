using PdfSharp.Drawing;

namespace QTIEditor.Export.PDF
{
    class XTextBlock
    {

        public XBrush brush;

        public string text;

        public XFont font;

        public XPoint pos;

        public double width;


        public XTextBlock(string text, XBrush brush, XFont font, XPoint pos, double width)
        {
            this.brush = brush;
            this.text = text;
            this.font = font;
            this.pos = pos;
            this.width = width;
        }

        public XTextBlock(string text, XBrush brush, XFont font, double xPos, double yPos, double width)
        {
            this.brush = brush;
            this.text = text;
            this.font = font;
            pos = new(xPos, yPos);
            this.width = width;
        }


        public void Draw(XGraphics gfx)
        {
            var lines = text.Split('\n');

            double heightOffset = 0d;
            foreach (var line in lines)
            {
                WriteLineWrap(line, gfx, ref heightOffset);

                heightOffset += font.GetHeight();
            }
        }

        void WriteLineWrap(string line, XGraphics gfx, ref double heightOffset)
        {
            var expectedSize = gfx.MeasureString(line, font);
            if (expectedSize.Width <= width)
            {
                gfx.DrawString(line, font, brush, pos + new XVector(0, heightOffset));
                heightOffset += font.GetHeight();
                return;
            }

            double approxCharWidth = expectedSize.Width / line.Length;
            int splitIndex = (int)(width / approxCharWidth);

            // make the string smaller until we're under our limit
            while (gfx.MeasureString(line[..splitIndex], font).Width > width)
            {
                splitIndex--;
            }

            // then reverse iterate until a word border is found
            for (int i = splitIndex - 1; i > 0; i--)
            {
                if (char.IsWhiteSpace(line[i]))
                {
                    gfx.DrawString(line[0..i], font, brush, pos + new XVector(0, heightOffset));
                    heightOffset += font.GetHeight();
                    WriteLineWrap(line[(i+1)..], gfx, ref heightOffset);
                    return;
                }
            }


            
            
        }

    }
}
