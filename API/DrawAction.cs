using API.Drawing;
using System.Drawing;

namespace API
{
    public class DrawAction
    {

        public Point[] Points { get; }
        public ToolType Type { get; }
        public Color Color { get; }
        public int Size { get; }
        
        public DrawAction(Point[] points, ToolType type, Color color, int size)
        {
            Points = points;
            Type = type;
            Color = color;
            Size = size;
        }

    }
}
