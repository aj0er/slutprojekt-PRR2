using API.Drawing;
using System.Collections.Generic;
using System.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett ritverktyg som ritar som en pensel.
    /// </summary>
    public class BrushTool : IDrawTool
    {

        public ToolType Type => ToolType.Brush;

        public bool Instant => false;
        
        public bool NoInteraction => false;

        public void Draw(Graphics graphics, DrawAction action, DirectBitmap bitmap, List<DrawAction> allElements)
        {
            Pen pen = new Pen(action.Color, action.Size);
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round; // Gör penseln rund och inte så kantig
            
            graphics.DrawLines(pen, action.Points.ToArray());
        }

    }
}
