using System.Collections.Generic;
using System.Drawing;
using API.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Interface för ett verktyg som ritar något på målarduken.
    /// </summary>
    public interface IDrawTool
    {

        /// <summary>
        /// Ritar något på målarduken.
        /// </summary>
        /// <param name="graphics">Graphics-instansen att rita med.</param>
        /// <param name="action">Händelsen som ska ritas.</param>
        /// <param name="bitmap">Bitmap som ska ritas på.</param>
        /// <param name="allElements">Lista över alla element som finns på målarduken för att ev. modifiera dessa.</param>
        void Draw(Graphics graphics, DrawAction action, DirectBitmap bitmap, List<DrawAction> allElements);

        /// <summary>
        /// Ritverktygets typ.
        /// </summary>
        ToolType Type { get; }

        /// <summary>
        /// Om ritverktyget ska utföras direkt utan att kunna modifieras senare, t.ex BucketTool.
        /// </summary>
        bool Instant { get; }
        
        /// <summary>
        /// Om ritverktyget utförs utan att markera några punkter överhuvudtaget, t.ex. ClearTool.
        /// </summary>
        bool NoInteraction { get; }

    }
}
