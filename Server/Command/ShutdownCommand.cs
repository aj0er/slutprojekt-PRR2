namespace Server.Command
{
    /// <summary>
    /// Ett kommando som stänger ned servern.
    /// </summary>
    public class ShutdownCommand: ConsoleCommand
    {
        
        public ShutdownCommand() : base("shutdown") { }

        public override bool OnExecute(string[] arguments)
        {
            Server.WriteConsole("Shutting down server...");
            return false;
        }
        
    }
}