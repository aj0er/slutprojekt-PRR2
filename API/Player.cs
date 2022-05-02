namespace API
{
    /// <summary>
    /// En spelare som spelar spelet.
    /// </summary>
    public class Player
    {

        /// <summary>
        /// Spelarens unika nätverksid.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Spelarens avatar.
        /// </summary>
        public Avatar Avatar { get; }
        /// <summary>
        /// Spelarens namn.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Spelarens nuvarande state.
        /// </summary>
        public PlayerState State { get; set; }
        /// <summary>
        /// Spelarens poäng.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Skapar en ny spelare.
        /// </summary>
        /// <param name="id">Spelarens unika nätverksid.</param>
        /// <param name="name">Spelarens namn.</param>
        /// <param name="avatar">Spelarens avatar.</param>
        /// <param name="score">Spelarens poäng.</param>
        public Player(int id, string name, Avatar avatar, int score)
        {
            Id = id;
            Name = name;
            Avatar = avatar;
            State = PlayerState.Default;
            Score = score;
        }

        /// <summary>
        /// Returnerar om spelaren spelar aktivt.
        /// </summary>
        /// <returns>Om spelaren spelar aktivt.</returns>
        public bool IsActive()
        {
            return State != PlayerState.Default;
        }

    }
}