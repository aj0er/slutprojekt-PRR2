using System.Collections.Generic;
using API;
using Server.Games;

namespace Server.State.Games
{
    /// <summary>
    /// State för en runda, en kollektion med flertal <see cref="DrawState" />, en för varje spelare.
    /// </summary>
    class RoundState : GameStateCollection
    {

        private readonly int _round;

        /// <summary>
        /// Skapar en ny RoundState.
        /// </summary>
        /// <param name="game">Spelet som detta state tillhör.</param>
        /// <param name="round">Den nuvarande rundan.</param>
        public RoundState(Game game, int round): base(game)
        {
            _round = round;
        }

        protected override void OnCollectionStart()
        {
            int playerCount = Game.Players.Count;
            string[] words = new string[playerCount];
            List<string> wordList = Game.Words;
            
            for(int i = 0; i < words.Length; i++) // Välj ut spelarantal unika ord för denna runda
            {
                string word = wordList.Count > 0 ? wordList[Game.GameRandom.Next(wordList.Count)] : Game.DefaultWord; // Använd DefaultWord om vi fått slut på ord (borde inte hända)
                wordList.Remove(word); // Ordet ska inte förekomma i detta spelet igen.

                words[i] = word;
            }

            for(int i = 0; i < playerCount; i++)
            {
                Player player = Game.Players[i];
                States.Add(new DrawState(Game, _round, player, words[i])); // Varje spelare ska ha en runda då de ritar
            }
        }

    }
}