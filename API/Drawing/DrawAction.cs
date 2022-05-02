using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace API.Drawing
{
    /// <summary>
    /// En händelse då spelaren ritat något.
    /// </summary>
    public class DrawAction
    {

        /// <summary>
        /// Punkter på ritytan som händelsen markerar.
        /// </summary>
        public List<Point> Points { get; }
        /// <summary>
        /// Verktyget som ska rita händelsen.
        /// </summary>
        public ToolType Type { get; }
        /// <summary>
        /// Färg som händelsen ska ritas i.
        /// </summary>
        public Color Color { get; }
        /// <summary>
        /// Storlek som händelsen ska ritas med.
        /// </summary>
        public int Size { get; }
        
        /// <summary>
        /// Om händelsen ska utföras direkt utan att fortsättas ritas varje gång.
        /// </summary>
        public bool Instant { get; }
        
        /// <summary>
        /// Skapar en ny DrawAction.
        /// </summary>
        /// <param name="points">Punkter som spelaren valt.</param>
        /// <param name="type">Verktyget som spelaren ritat med.</param>
        /// <param name="color">Färgen som spelaren ritat med.</param>
        /// <param name="size">Storleken som spelaren ritat med.</param>
        /// <param name="instant">Om händelsen ska utföras direkt utan att fortsättas ritas varje gång.</param>
        [JsonConstructor]
        public DrawAction(List<Point> points, ToolType type, Color color, int size, bool instant)
        {
            Points = points;
            Type = type;
            Color = color;
            Size = size;
            Instant = instant;
        }

        /// <summary>
        /// Skapar en ny DrawAction för ett verktyg. Detta implicerar att verktyget är instant och inte kräver någon interaktion, t.ex. ClearTool.
        /// </summary>
        /// <param name="type">Verktygets typ</param>
        public DrawAction(ToolType type): this(null, type, Color.White, 0, true) { }

    }
}