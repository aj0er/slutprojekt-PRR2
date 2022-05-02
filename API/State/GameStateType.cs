namespace API.State
{
    /// <summary>
    /// Enumeration över de states som spelet kan befinna sig i.
    /// </summary>
    public enum GameStateType
    {
        /// <summary>
        /// Spelets lobbystate, väntar på att spelare ska ansluta.
        /// </summary>
        Lobby,

        /// <summary>
        /// Spelets main state där spelare ritar.
        /// </summary>
        Draw,

        /// <summary>
        /// Spelets post main state då DrawState avslutas.
        /// </summary>
        PostDraw,

        /// <summary>
        /// Spelets post round state då spelet väntar ett antal sekunder innan en ny runda påbörjas.
        /// </summary>
        PostRound,

        /// <summary>
        /// Spelets post state då alla rundor är spelade och topplistan ska visas.
        /// </summary>
        PostGame
    }
}
