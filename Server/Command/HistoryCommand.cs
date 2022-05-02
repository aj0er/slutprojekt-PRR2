using System;
using System.Collections.Generic;
using System.Linq;
using API.History;
using Server.History;
using Server.Store;

namespace Server.Command
{
    /// <summary>
    /// Ett kommando som visar historiken för ett spel.
    /// </summary>
    public class HistoryCommand : ConsoleCommand
    {

        private readonly JsonStore<Guid, GameHistory> _historyStore;
        
        /// <summary>
        /// Skapar ett nytt HistoryCommand.
        /// </summary>
        /// <param name="historyStore">Store att hämta historiken ifrån.</param>
        public HistoryCommand(JsonStore<Guid, GameHistory> historyStore) : base("history")
        {
            _historyStore = historyStore;
        }

        public override bool OnExecute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Server.WriteConsole("Available subcommands: list, all, type, remove");
                return true;
            }

            switch (arguments[0])
            {
                case "list":
                {
                    ICollection<GameHistory> histories = _historyStore.All;
                    Console.WriteLine($"> Listing all games stored in history ({histories.Count}):");
                    foreach (GameHistory history in histories)
                    {
                        // Konvertera från unix-tidsstämpel till nuvarande tidzonen
                        string timeStarted = DateTimeOffset.FromUnixTimeMilliseconds(history.TimeStarted).ToLocalTime().ToString("g");
                        string timeEnded = DateTimeOffset.FromUnixTimeMilliseconds(history.TimeEnded).ToLocalTime().ToString("g");
                        
                        Console.WriteLine($"> ID: {history.Id} | {timeStarted} -> {timeEnded} [{history.History.Count} events]");
                    }

                    break;
                }
                case "all":
                {
                    if (arguments.Length != 2)
                    {
                        Server.WriteConsole("Syntax: history all <game_id>");
                        return true;
                    }

                    if (!Guid.TryParse(arguments[1], out Guid gameId))
                    {
                        Server.WriteConsole("Invalid game id format!");
                        return true;
                    }
                    
                    ShowHistory(gameId, null);
                    break;
                }
                case "type":
                {
                    if (arguments.Length != 3)
                    {
                        string typeList = String.Join(", ", Enum.GetNames(typeof(HistoryType)));
                        Server.WriteConsole($"Available history types: {typeList}");
                        Server.WriteConsole("Syntax: history type <game_id> <history_type>");
                        return true;
                    }

                    if (!Guid.TryParse(arguments[1], out Guid gameId))
                    {
                        Server.WriteConsole("Invalid game id format!");
                        return true;
                    }

                    if (!Enum.TryParse(arguments[2], out HistoryType historyType))
                    {
                        Server.WriteConsole("Invalid history type!");
                        return true;
                    }
                    
                    ShowHistory(gameId, historyType);
                    break;
                }
                case "remove":
                {
                    if (arguments.Length != 2)
                    {
                        Server.WriteConsole("Syntax: history remove <game_id>");
                        return true;
                    }
                    
                    if (!Guid.TryParse(arguments[1], out Guid gameId))
                    {
                        Server.WriteConsole("Invalid game id format!");
                        return true;
                    }
                    
                    GameHistory gameHistory = _historyStore.GetById(gameId);
                    if (gameHistory == null)
                    {
                        Server.WriteConsole("The specified game doesn't exist!");
                        return true;
                    }
                    
                    _historyStore.Delete(gameHistory);
                    Server.WriteConsole($"Successfully removed history for game with id \"{gameHistory.Id.ToString()}\".");
                    break;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Skapar en sammanfattning över historiken och applicerar valt filter om det finns.
        /// </summary>
        /// <param name="guid">Id för spelet att visa historiken för.</param>
        /// <param name="filter">Optional filter att visa enskilda typer.</param>
        private void ShowHistory(Guid guid, HistoryType? filter)
        {
            GameHistory gameHistory = _historyStore.GetById(guid);
            if (gameHistory == null)
            {
                Server.WriteConsole("The specified game doesn't exist!");
                return;
            }
            
            string filterName = filter != null ? Enum.GetName(typeof(HistoryType), filter) : "None";
            Server.WriteConsole($"Showing history for game \"{guid.ToString()}\", filter={filterName}:");
            
            IEnumerable<HistoryPoint> historyPoints = gameHistory
                .History
                .Where(p => filter == null || p.Data != null && p.Data.HistoryType == filter); // Applicera filter om det inte är null
            
            foreach (HistoryPoint point in historyPoints)
            {
                IHistory data = point.Data;
                if(data == null)
                    continue; // Hoppa över om vi inte kunde deserialisera denna punkten
                
                string timestamp = DateTimeOffset.FromUnixTimeMilliseconds(point.Timestamp).ToLocalTime().ToString("g");
                string info = data.CreateSummary();
                
                if(info != null)
                    Console.WriteLine($"[{timestamp}] {info}");
            }
        }
        
    }
}