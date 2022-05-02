using System.Drawing;
using API.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett ritverktyg som ritar en rektangel.
    /// </summary>
    class RectangleTool : SelectionDrawTool
    {
        
        protected override void DrawSelection(Graphics graphics, DrawAction action, Rectangle rectangle)
        {
            graphics.DrawRectangle(new Pen(action.Color, action.Size), rectangle);
        }

        public override ToolType Type => ToolType.Rectangle;

    }
}