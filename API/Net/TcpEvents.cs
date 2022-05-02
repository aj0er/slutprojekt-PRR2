using System;

namespace API.Net
{
    /// <summary>
    /// Events för händelser som sker i TCP-anslutningar.
    /// </summary>
    public class TcpEvents
    {

        /// <summary>
        /// Event för då en klient ansluter till TCP-servern.
        /// </summary>
        public event EventHandler<TcpConnectionEventArgs> Connect;
        /// <summary>
        /// Event för då en klient skickat ett meddelande till TCP-servern.
        /// </summary>
        public event EventHandler<TcpMessageEventArgs> Message;
        /// <summary>
        /// Event för då en klient avslutar anslutningen till en TCP-server.
        /// </summary>
        public event EventHandler<TcpDisconnectionEventArgs> Disconnect;

        /// <summary>
        /// Invokar event handlers beroende på typen av event args.
        /// </summary>
        /// <param name="eventArgs">Argumentet som ska mappas till en event handlar.</param>
        public void DispatchEvent(EventArgs eventArgs)
        {
            if (eventArgs == null)
                return;

            switch (eventArgs)
            {
                case TcpMessageEventArgs args:
                    {
                        Message?.Invoke(this, args);
                        break;
                    }
                case TcpDisconnectionEventArgs args:
                    {
                        Disconnect?.Invoke(this, args);
                        break;
                    }
                case TcpConnectionEventArgs args:
                    {
                        Connect?.Invoke(this, args);
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Argument för event då en klient ansluter till en server.
    /// </summary>
    public class TcpConnectionEventArgs : EventArgs
    {
        
        /// <summary>
        /// Anslutningen som anslöt.
        /// </summary>
        public TcpConnection Connection { get; }

        /// <summary>
        /// Skapar en ny TcpConnectionEventArgs. 
        /// </summary>
        /// <param name="connection">Anslutningen som anslöt.</param>
        public TcpConnectionEventArgs(TcpConnection connection)
        {
            Connection = connection;
        }
        
    }

    /// <summary>
    /// Argument för event då en klient avslutar anslutningen till en server.
    /// </summary>
    public class TcpDisconnectionEventArgs : EventArgs
    {

        /// <summary>
        /// Anslutningen som avslutades.
        /// </summary>
        public TcpConnection Connection { get; }

        /// <summary>
        /// Skapar en ny TcpDisconnectionEventArgs.
        /// </summary>
        /// <param name="connection">Anslutningen som avslutades.</param>
        public TcpDisconnectionEventArgs(TcpConnection connection)
        {
            Connection = connection;
        }

    }

    /// <summary>
    /// Argument för event då en server skickar ett meddelande till en klient.
    /// </summary>
    public class TcpMessageEventArgs : EventArgs
    {

        /// <summary>
        /// Anslutningen som skickade meddelandet.
        /// </summary>
        public TcpConnection Connection { get; }
        /// <summary>
        /// Meddelandet som skickades.
        /// </summary>
        public TcpMessage Result { get; }

        /// <summary>
        /// Skapar en ny TcpMessageEventArgs
        /// </summary>
        /// <param name="connection">Anslutningen som skickade meddelandet.</param>
        /// <param name="result">Meddelandet som skickades.</param>
        public TcpMessageEventArgs(TcpConnection connection, TcpMessage result)
        {
            Connection = connection;
            Result = result;
        }

    }
}
