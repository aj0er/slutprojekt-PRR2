using Newtonsoft.Json;
using System.Collections.Generic;

namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas till alla spelare för att få reda på när en spelares state ändrats.
    /// </summary>
    [PacketAttribute(10)]
    public class PlayerStatePacket : Packet
    {

        /// <summary>
        /// Mappning med spelarid -> deras nuvarande state.
        /// </summary>
        public Dictionary<int, PlayerState> States { get; }

        /// <summary>
        /// Skapar ett nytt PlayerStatePacket.
        /// </summary>
        /// <param name="playerId">Spelarens id.</param>
        /// <param name="state">Spelarens nya state.</param>
        public PlayerStatePacket(int playerId, PlayerState state) : this(new Dictionary<int, PlayerState> { { playerId, state } }) {}

        /// <summary>
        /// Skapar ett nytt PlayerStatePacket.
        /// </summary>
        /// <param name="states">Mappning med spelarid -> deras nuvarande state.</param>
        [JsonConstructor]
        public PlayerStatePacket(Dictionary<int, PlayerState> states)
        {
            States = states;
        }

    }
}
