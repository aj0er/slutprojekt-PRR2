using System.Collections.Generic;
using System.Drawing;
using API.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett verktyg som modifierar en DrawAction för att anropa ett annat verktyg och/eller modifiera egenskaperna på rithändelsen.
    /// </summary>
    public abstract class SubTool: IDrawTool
    {

        /// <summary>
        /// Modifierar rithändelsen.
        /// Här kan t.ex. ToolType ändras för att dirigera händelsen till ett annat verktyg.
        /// </summary>
        /// <param name="original">Den originella händelsen.</param>
        /// <returns>Den modifierade rithändelsen.</returns>
        public abstract DrawAction ModifyAction(DrawAction original);

        public void Draw(Graphics graphics, DrawAction action, DirectBitmap bitmap, List<DrawAction> allElements) { }

        public abstract ToolType Type { get; }
        public abstract bool Instant { get; }
        public abstract bool NoInteraction { get; }
        
    }
}