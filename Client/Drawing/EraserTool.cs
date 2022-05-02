using System.Drawing;
using API.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett ritverktyg som använder <see cref="BrushTool" /> för att rita över med ritytans bakgrundsfärg.
    /// </summary>
    class EraserTool : SubTool
    {
        
        private readonly Color _backgroundColor;

        /// <summary>
        /// Skapar en ny EraserTool.
        /// </summary>
        /// <param name="backgroundColor">Bakgrundsfärgen på ritytan, att rita över med.</param>
        public EraserTool(Color backgroundColor)
        {
            _backgroundColor = backgroundColor;
        }

        public override DrawAction ModifyAction(DrawAction original)
        {
            return new DrawAction(original.Points, ToolType.Brush, _backgroundColor, original.Size, original.Instant);
        }

        public override ToolType Type => ToolType.Eraser;
        public override bool Instant => false;
        public override bool NoInteraction => false;
        
    }
}