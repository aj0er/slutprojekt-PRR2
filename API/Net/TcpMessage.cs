namespace API.Net
{
    /// <summary>
    /// Ett meddelande som skickas på en TCP-ström.
    /// </summary>
    public class TcpMessage {

        /// <summary>
        /// Den råa datan som lästs.
        /// </summary>
        public byte[] Data { get; }
        
        /// <summary>
        /// Längden på buffer som krävs för att läsa datan.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Skapar ett nytt TcpMessage.
        /// </summary>
        /// <param name="data">Den råa datan som lästs.</param>
        /// <param name="length">Längden på buffern.</param>
        public TcpMessage(byte[] data, int length)
        {
            Data = data;
            Length = length;
        }

        /// <summary>
        /// Kollar om meddelandet är giltigt, dvs. har data att läsa.
        /// </summary>
        /// <returns>Om meddelandet är giltigt.</returns>
        public bool IsInvalid()
        {
            return Length < 1;
        }

    }
}
