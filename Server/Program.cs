using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            new Server().Start();
            Thread.Sleep(3000); // Ge användaren tid att läsa det sista meddelandet.
        }
    }
}