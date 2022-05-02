using System.Collections.Generic;
using System.IO;

namespace Server.Command
{
    /// <summary>
    /// Ett kommando för att modifiera spelets ordlista.
    /// </summary>
    public class WordCommand : ConsoleCommand
    {
        
        private readonly HashSet<string> _words;

        /// <summary>
        /// Skapar ett nytt WordCommand.
        /// </summary>
        /// <param name="words">Spelets ordlista.</param>
        public WordCommand(HashSet<string> words) : base("words")
        {
            _words = words;
        }
        
        public override bool OnExecute(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                Server.WriteConsole("Available subcommands: add, remove");
                return true;
            }

            switch (arguments[0])
            {
                case "add":
                {
                    if (arguments.Length != 2)
                    {
                        Server.WriteConsole("Syntax: words add <word>");
                        return true;
                    }

                    string word = arguments[1];
                    if (_words.Contains(word))
                    {
                        Server.WriteConsole("This word has already been added!");
                        return true;
                    }
                    
                    _words.Add(word);
                    SaveWordList();

                    Server.WriteConsole($"Successfully added word \"{word}\".");
                    break;
                }

                case "remove":
                {
                    if (arguments.Length != 2)
                    {
                        Server.WriteConsole("Syntax: words remove <word>");
                        return true;
                    }

                    string word = arguments[1];
                    if (!_words.Contains(word))
                    {
                        Server.WriteConsole("The wordlist does not contain this word!");
                        return true;
                    }

                    _words.Remove(word);
                    SaveWordList();

                    Server.WriteConsole($"Successfully removed word \"{word}\".");
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Sparar ordlistan till en fil.
        /// </summary>
        private void SaveWordList()
        {
            try
            {
                File.WriteAllLines(Server.WordListFile, _words);
            }
            catch (IOException ex)
            {
                Server.WriteConsole($"Unable to update word-list file: {ex.Message}");
            }
        }
        
    }
}