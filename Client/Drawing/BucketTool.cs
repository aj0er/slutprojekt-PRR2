using API.Drawing;
using System.Collections.Generic;
using System.Drawing;

namespace Client.Drawing
{
    /// <summary>
    /// Ett ritverktyg som fyller det klickade området med samma färg med den valda färgen.
    /// </summary>
    class BucketTool : IDrawTool
    {

        public ToolType Type => ToolType.Bucket;
        public bool Instant => true;
        public bool NoInteraction => false;

        public void Draw(Graphics graphics, DrawAction action, DirectBitmap bitmap, List<DrawAction> allElements)
        {
            Stack<Point> pixels = new Stack<Point>(); // Vi använder en stack eftersom en rekursiv metod leder till Stackoverflow p.g.a alla tusentals rutor som måste kollas.
            Point clicked = action.Points[0];

            Color targetColor = bitmap.GetPixel(clicked.X, clicked.Y);
            Color color = action.Color;

            if(targetColor.ToArgb() == color.ToArgb()) // Om den valda färgen är samma som den vi klickade på behöver vi inte fylla här.
                return;

            pixels.Push(clicked);

            while (pixels.Count > 0)
            {
                Point point = pixels.Pop();
                int x = point.X;
                int y = point.Y;
                if (x < bitmap.Width && x > 0 && y < bitmap.Height && y > 0) // Stanna innanför ramen av ritytan.
                {
                    Color currentColor = bitmap.GetPixel(x, y);
                    if (currentColor.ToArgb() == targetColor.ToArgb())
                    {
                        for (int i = allElements.Count - 1; i >= 0; i--) 
                        {
                            // Ta bort element som är ivägen för fyllningen och som skulle skriva över färgen om de ritades igen.
                            DrawAction element = allElements[i];
                            if (element.Color.ToArgb() == targetColor.ToArgb())
                            {
                                allElements.RemoveAt(i);
                            }
                        }

                        bitmap.SetPixel(x, y, color);

                        // Skapa ett kors runt punkten för att kolla de närliggande pixlarna.
                        pixels.Push(new Point(x - 1, y));
                        pixels.Push(new Point(x + 1, y));
                        pixels.Push(new Point(x, y - 1));
                        pixels.Push(new Point(x, y + 1));
                    }
                }
            }

        }

    }
}
