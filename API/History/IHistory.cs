namespace API.History
{
    /// <summary>
    /// Interface för ett objekt som ska placeras i historiken.
    /// </summary>
    public interface IHistory
    {
        
        /// <summary>
        /// Historikens typ.
        /// </summary>
        HistoryType HistoryType { get; }

        /// <summary>
        /// Genererar en sammanfattning av vad som hände i spelet.
        /// Om metoden returnerar null visas ingen sammanfattning för den här typen.
        /// </summary>
        /// <returns>Sammanfattning av vad som hände eller null.</returns>
        string CreateSummary();

    }
}
