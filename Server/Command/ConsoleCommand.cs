namespace Server.Command
{
    /// <summary>
    /// Abstrakt kommando som skickas i konsolen och ska hanteras.
    /// </summary>
    public abstract class ConsoleCommand
    {

        /// <summary>
        /// Namn på kommandot.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Skapar ett nytt ConsoleCommand.
        /// </summary>
        /// <param name="name">Namnet på kommandot, som är det som skrivs i konsolen för att exekvera kommandot.</param>
        protected ConsoleCommand(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Abstrakt metod som anropas när kommandot skrivs i konsolen.
        /// </summary>
        /// <param name="arguments">Argumenten som användaren skrev in.</param>
        /// <returns>Om servern ska fortsätta köras.</returns>
        public abstract bool OnExecute(string[] arguments);

    }
}