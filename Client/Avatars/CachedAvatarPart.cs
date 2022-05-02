using System.Drawing;

namespace Client.Avatars
{
    /// <summary>
    /// En del av en avatar som är cachad genom att endast ladda in bilden i RAM-minnet en gång.
    /// </summary>
    public class CachedAvatarPart
    {

        /// <summary>
        /// Typen på delen.
        /// </summary>
        public AvatarPartType Type { get; }
        /// <summary>
        /// Bilden med delen.
        /// </summary>
        public Image Image { get; }
        /// <summary>
        /// Delens unika ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Skapar en ny CachedAvatarPart.
        /// </summary>
        /// <param name="type">Typen på delen.</param>
        /// <param name="image">Bilden med delen.</param>
        /// <param name="id">Delens unika ID.</param>
        public CachedAvatarPart(AvatarPartType type, Image image, int id)
        {
            Type = type;
            Image = image;
            Id = id;
        }

    }
}
