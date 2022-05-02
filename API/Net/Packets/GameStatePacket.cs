using API.State;

namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas till spelare för att de ska få reda på i vilket skick spelet befinner sig i.
    /// Detta kan skickas när rundor startar, men även när spelaren ansluter mitt i spelet.
    /// </summary>
    [PacketAttribute(9)]
    public class GameStatePacket : Packet
    {

        /// <summary>
        /// Spelets nuvarande state.
        /// </summary>
        public GameStateType Type {get;}
        /// <summary>
        /// Hur mycket tid som finns kvar av ritandet.
        /// </summary>
        public int TimeLeft { get; }
        /// <summary>
        /// Spelets nuvarande runda.
        /// </summary>
        public int Round { get; }

        /// <summary>
        /// Skapar ett nytt GameStatePacket.
        /// </summary>
        /// <param name="type">Vilket state som spelet befinner sig i.</param>
        /// <param name="timeLeft">Hur mycket tid som finns kvar.</param>
        /// <param name="round">Den nuvarande rundan.</param>
        public GameStatePacket(GameStateType type, int timeLeft, int round)
        {
            Type = type;
            TimeLeft = timeLeft;
            Round = round;
        }

    }
}
