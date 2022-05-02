using System;

namespace API.Net
{
    /// <summary>
    /// Ett attribut som placeras på <see cref="Packet"/> klasser för att berätta vilket unikt Packet ID de har.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PacketAttribute : Attribute
    {
        
        /// <summary>
        /// ID för paketets typ.
        /// <see cref="PacketSerializer"/>
        /// </summary>
        public uint PacketId { get; }

        /// <summary>
        /// Skapar en ny PacketAttribute.
        /// </summary>
        /// <param name="packetId">Paketets unika ID.</param>
        public PacketAttribute(uint packetId)
        {
            PacketId = packetId;
        }
        
    }
}
