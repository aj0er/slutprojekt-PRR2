namespace API.Net.Packets
{
    /// <summary>
    /// Paket som skickas till alla spelare när en spelare har gissat rätt, får information om hur mycket poäng som spelaren får.
    /// </summary>
    [PacketAttribute(11)]
    public class PlayerScorePacket : Packet
    {

        /// <summary>
        /// Spelarens id.
        /// </summary>
        public int PlayerId { get; }
        /// <summary>
        /// Spelarens nuvarande antal poäng.
        /// </summary>
        public int Score { get; }

        /// <summary>
        /// Skapar ett nytt PlayerScorePaket.
        /// </summary>
        /// <param name="playerId">Spelarens id.</param>
        /// <param name="score">Spelarens nuvarande poäng.</param>
        public PlayerScorePacket(int playerId, int score)
        {
            PlayerId = playerId;
            Score = score;
        }

    }
}
