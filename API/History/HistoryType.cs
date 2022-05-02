namespace API.History
{
    /// <summary>
    /// Typer för objekt som ska placeras i historiken.
    /// </summary>
    public enum HistoryType
    {
        Connect,
        Chat,
        Disconnect,
        
        /// <summary>
        /// Då ett game state startar.
        /// </summary>
        StateStart,
        
        /// <summary>
        /// Då ett game state avslutas.
        /// </summary>
        StateEnd,
        
        /// <summary>
        /// Då ett meddelande skickas till alla spelare i servern (ej meddelande från spelare).
        /// </summary>
        BroadcastMessage,
    }
}
