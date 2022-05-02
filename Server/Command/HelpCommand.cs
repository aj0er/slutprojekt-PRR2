using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Command
{
    /// <summary>
    /// Ett kommando som visar vilka kommandon som är giltiga.
    /// </summary>
    public class HelpCommand : ConsoleCommand
    {
        
        private readonly List<ConsoleCommand> _commands;

        /// <summary>
        /// Skapar ett nytt HelpCommand.
        /// </summary>
        /// <param name="commands">Listan med kommandon som finns i servern.</param>
        public HelpCommand(List<ConsoleCommand> commands) : base("help")
        {
            _commands = commands;
        }

        public override bool OnExecute(string[] arguments)
        {
            string commandNames = String.Join(", ", _commands.Select(c => c.Name));
            Server.WriteConsole($"Available commands: {commandNames}", ConsoleColor.Yellow);

            return true;
        }
        
    }
}