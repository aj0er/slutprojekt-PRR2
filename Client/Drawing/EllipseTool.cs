using API.Drawing;
using System.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett ritverktyg som ritar en ellips.
    /// </summary>
    class EllipseTool : SelectionDrawTool 
    {

        public override ToolType Type => ToolType.Ellipse;

        protected override void DrawSelection(Graphics graphics, DrawAction action, Rectangle selection)
        {
            graphics.DrawEllipse(new Pen(action.Color, action.Size), selection);
        }

    }
}
