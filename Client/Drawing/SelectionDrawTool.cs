using API.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett ritverktyg som ritar en vald markering. 
    /// En första punkt väljs och sedan kan användaren ändra storleken på markeringen.
    /// </summary>
    public abstract class SelectionDrawTool : IDrawTool
    {

        public void Draw(Graphics graphics, DrawAction action, DirectBitmap bitmap, List<DrawAction> allElements)
        {
            if(action.Points.Count < 2)
                return;
            
            Point first   = action.Points[0];
            Point current = action.Points[1]; // Var spelaren drar musen just nu.

            int width  = Math.Abs(current.X - first.X); // Den absoluta bredden på markeringen.
            int height = Math.Abs(current.Y - first.Y); // Den absoluta höjden på markeringen.

            Rectangle selection;
            if (current.X < first.X) // Om musen är till vänster om den ursprungliga markeringen.
            {
                if (current.Y > first.Y) 
                {
                    // Om musen är under den ursprungliga markeringen.
                    selection = new Rectangle(current.X, first.Y, width, height);
                }
                else
                {
                    // Om musen är över den ursprungliga markeringen.
                    selection = new Rectangle(current.X, current.Y, width, height);
                }
            }
            else // Om musen är till höger om den ursprungliga markeringen.
            {
                if (current.Y < first.Y)
                {
                    // Om musen är över den ursprungliga markeringen.
                    selection = new Rectangle(first.X, current.Y, width, height);
                }
                else
                {
                    // Om musen är under den ursprungliga markeringen.
                    selection = new Rectangle(first.X, first.Y, width, height);
                }
            }
            
            DrawSelection(graphics, action, selection);
        }

        /// <summary>
        /// Ritar den valda markeringen.
        /// </summary>
        /// <param name="graphics">Graphics-instansen att rita med.</param>
        /// <param name="action">Händelsen att rita.</param>
        /// <param name="selection">Markeringen att rita.</param>
        protected abstract void DrawSelection(Graphics graphics, DrawAction action, Rectangle selection);

        
        public abstract ToolType Type { get; }

        public bool Instant => false;
        
        public bool NoInteraction => false;
        
    }
}
