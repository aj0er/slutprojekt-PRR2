using API.Drawing;
using System.Collections.Generic;
using System.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett ritverktyg som tömmer ritytan.
    /// </summary>
    class ClearTool : IDrawTool
    {

        public ToolType Type => ToolType.Clear;
        public bool Instant => false;
        public bool NoInteraction => true;

        public void Draw(Graphics graphics, DrawAction action, DirectBitmap bitmap, List<DrawAction> allElements)
        {
            allElements.Clear();
            graphics.Clear(Color.White);
        }

    }
}