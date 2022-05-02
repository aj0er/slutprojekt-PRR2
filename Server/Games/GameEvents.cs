using System;
using API.Net;
using API.Net.Packets;

namespace Server.Games
{
    public class GameEvents
    {

        /// <summary>
        /// Event handler för då en spelare ritar något.
        /// </summary>
        public event EventHandler<DrawPacket> Draw;

        /// <summary>
        /// Event handler för då en spelare skrivit något i chatten.
        /// </summary>
        public event EventHandler<ChatPacket> Chat;

        /// <summary>
        /// Event handler för då en spelare ansluter till servern.
        /// </summary>
        public event EventHandler<ConnectPacket> Connect;

        /// <summary>
        /// Event handler för då en spelare lämnar servern.
        /// </summary>
        public event EventHandler<DisconnectPacket> Disconnect;

        /// <summary>
        /// Dispatchar ett event om en matchande event handler finns för det specifierade paketet.
        /// </summary>
        /// <param name="packet">Paketet som kommer in.</param>
        public void DispatchEvent(Packet packet)
        {
            switch (packet)
            {
                case DrawPacket p:
                {
                    Draw?.Invoke(this, p);
                    break;
                }

                case ChatPacket p:
                {
                    Chat?.Invoke(this, p);
                    break;
                }

                case ConnectPacket p:
                {
                    Connect?.Invoke(this, p);
                    break;
                }

                case DisconnectPacket p:
                {
                    Disconnect?.Invoke(this, p);
                    break;
                }
            }
        }

    }
}