namespace Client
{
    /// <summary>
    /// Interface för ett formulär som håller i anslutningssidan.
    /// </summary>
    public interface IStartForm
    {
        
        /// <summary>
        /// Hanterar processen då klienten anslutit till servern utan problem.
        /// </summary>
        void HandleConnect();

        /// <summary>
        /// Hanterar processen då klienten inte lyckats ansluta till servern. 
        /// </summary>
        void HandleFailedConnect();
        
    }
}
