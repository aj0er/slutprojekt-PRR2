using API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Client.Avatars
{
    /// <summary>
    /// Ansvarar för att ladda, generera och cacha avatarer.
    /// </summary>
    public class AvatarManager
    {

        /* Konstanter för generering av avatarer. */
        private const int HeadXMargin = 10;
        private const int HeadYMargin = 20;
        
        private const int EyesHeadMargin = 15;
        private const int EyesHeight = 20;
        
        private const int HatHeight = 40;
        private const int HatYOffset = 30;
        
        private const int MouthDepth = 10;
        private const int MouthXOffset = 15;
        private const int MouthHeadOffset = -20;

        private readonly Dictionary<int, Image> _avatarCache;
        private readonly List<CachedAvatarPart> _partCache;

        private Image _defaultAvatar;

        /// <summary>
        /// Skapar en ny AvatarManager och laddar in bilderna för avatardelarna.
        /// </summary>
        public AvatarManager()
        {
            _avatarCache = new Dictionary<int, Image>();
            _partCache = new List<CachedAvatarPart>();
            
            LoadParts();
        }

        /// <summary>
        /// Genererar en bild från en avatar.
        /// </summary>
        /// <param name="avatar">Avatar att generera bild för.</param>
        /// <returns>Genererad bild.</returns>
        public Image GenerateAvatar(Avatar avatar)
        {
            Bitmap map = new Bitmap(100, 100);
            Graphics graphics = Graphics.FromImage(map);

            Image hatImg = GetAvatarPart(AvatarPartType.Hat, avatar.Hat);
            if (hatImg == null)
                return GetDefaultAvatar();

            Image eyesImg = GetAvatarPart(AvatarPartType.Eyes, avatar.Eyes);
            if (eyesImg == null)
                return GetDefaultAvatar();

            Brush skinColorBrush = new SolidBrush(avatar.Color);
            
            // Width: Huvudet får HeadXMargin vänster och höger. Height: Huvudet får HeadYMargin vänster och höger
            Rectangle head = new Rectangle(new Point(HeadXMargin, HeadYMargin), new Size(map.Width - (HeadXMargin * 2), map.Height / 2 + HeadYMargin)); 

            graphics.FillEllipse(skinColorBrush, head); // Ritar huvudet
            graphics.DrawImage(eyesImg, head.X, head.Y + EyesHeadMargin, head.Width, EyesHeight); // Ritar ögonen

            int mouthYStart = head.Y + head.Height + MouthHeadOffset; // Börja munnen MouthHeadOffset från huvudets botten
            graphics.DrawCurve(new Pen(Color.Blue, 4), new Point[] { new Point(head.X + MouthXOffset, mouthYStart),
                new Point(head.X + head.Width / 2, mouthYStart + MouthDepth),
                new Point(head.X + head.Width - MouthXOffset, mouthYStart) }); // Ritar munnen

            graphics.DrawImage(hatImg, head.X + head.Width / 4, head.Y - HatYOffset, head.Width / 2, HatHeight); // Ritar hatten

            return map;
        }

        /// <summary>
        /// Hämtar en avatar-del från cachen, eller null om ingen fanns.
        /// </summary>
        /// <param name="type">Delens typ.</param>
        /// <param name="id">Delens ID.</param>
        /// <returns>Cachad bild eller null om den inte fanns.</returns>
        private Image GetAvatarPart(AvatarPartType type, int id)
        {
            CachedAvatarPart part = _partCache.Find(p => p.Type == type && p.Id == id);
            if (part == null)
                return null;

            return part.Image;
        }

        /// <summary>
        /// Hämtar den cachade avataren eller skapar en ny avatar för spelaren.
        /// </summary>
        /// <param name="player">Spelaren som avataren ska tillhöra.</param>
        /// <returns>Bilden för den genererade avataren.</returns>
        public Image GetOrCreateAvatar(Player player)
        {
            if (_avatarCache.TryGetValue(player.Id, out Image image))
                return image; // Den fanns i cachen.

            Image generated = GenerateAvatar(player.Avatar);
            _avatarCache[player.Id] = generated;
            return generated;
        }

        /// <summary>
        /// Skapar en dictionary med gränserna för avatarer, dvs. hur många som finns av varje typ.
        /// </summary>
        /// <returns>Nyskapad dictionary med avatargränser.</returns>
        public Dictionary<AvatarPartType, int> GetPartBoundaries()
        {
            Dictionary<AvatarPartType, int> parts = new Dictionary<AvatarPartType, int>();
            foreach (CachedAvatarPart part in _partCache)
            {
                parts.TryGetValue(part.Type, out int count); // Om typen redan finns i dictionaryn, hämta nuvarande antalet

                count++;
                parts[part.Type] = count;
            }

            return parts;
        }

        /// <summary>
        /// Laddar avatar-bilder från filsystemet.
        /// </summary>
        private void LoadParts()
        {
            foreach (AvatarPartType part in Enum.GetValues(typeof(AvatarPartType))) // Ladda alla konstanter i AvatarPartType som bilder från /resources/avatars/
            {
                string friendlyName = Enum.GetName(typeof(AvatarPartType), part);
                string path = $"resources/avatars/{friendlyName}/";

                foreach (string fileName in Directory.GetFiles(path))
                {
                    string name = fileName.Substring(path.Length).Split(".png".ToCharArray())[0]; // fileName är absolut relativ till path, ta fram endast namnet på filen (utan filändelse).
                    if (int.TryParse(name, out int id))
                    {

                        Image image;
                        try
                        {
                            image = Image.FromFile($"{fileName}");
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine(ex.Message);
                            continue; // Skippa.
                        }

                        _partCache.Add(new CachedAvatarPart(part, image, id));
                    }
                }
            }
        }

        /// <summary>
        /// Genererar en ny eller returnerar den cachade default avataren.
        /// </summary>
        /// <returns>Returnerar bilden för default-avataren.</returns>
        private Image GetDefaultAvatar()
        {
            if (_defaultAvatar == null)
            {
                _defaultAvatar = GenerateAvatar(new Avatar(0, 0, Color.Red)); // Cacha avataren som genereras.
            }

            return _defaultAvatar;
        }

    }
}