using System.Drawing;

namespace API
{
    /// <summary>
    /// En avatar som en spelare har.
    /// </summary>
    public class Avatar
    {

        /// <summary>
        /// ID för hatten i avataren.
        /// </summary>
        public int Hat { get; set; }
        /// <summary>
        /// ID för ögonen i avataren.
        /// </summary>
        public int Eyes { get; set; }
        /// <summary>
        /// Avatarens hudfärg.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// Skapar en ny Avatar.
        /// </summary>
        /// <param name="hat">ID för hatten.</param>
        /// <param name="eyes">ID för ögonen.</param>
        /// <param name="color">Hudfärgen.</param>
        public Avatar(int hat, int eyes, Color color)
        {
            Hat = hat;
            Eyes = eyes;
            Color = color;
        }

        public override string ToString()
        {
            return $"[Hat={Hat}, Eyes={Eyes}, Color={Color}]";
        }
        
    }
}
