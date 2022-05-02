namespace API
{
    /// <summary>
    /// Ett skick som spelaren befinner sig i.
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// Det ursprungliga statet som spelaren har då den ansluter.
        /// </summary>
        Default,
        
        /// <summary>
        /// Då spelaren ska gissa.
        /// </summary>
        Guessing,
        
        /// <summary>
        /// Då spelaren har gissat.
        /// </summary>
        Guessed,
        
        /// <summary>
        /// Då spelaren är ritaren.
        /// </summary>
        Drawing   
    }
}