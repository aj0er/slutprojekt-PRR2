namespace Server.Store
{
    /// <summary>
    /// En entitet som ska sparas i en <see cref="JsonStore{T, TE}"/>
    /// </summary>
    /// <typeparam name="T">Typen för objektets ID.</typeparam>
    public interface IStoreEntity<out T>
    {

        /// <summary>
        /// ID på entiteten.
        /// </summary>
        public T Id { get; }

    }
}
