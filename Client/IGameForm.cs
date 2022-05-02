using API;
using System.Collections.Generic;
using System.Drawing;
using API.Drawing;

namespace Client
{
    /// <summary>
    /// Interface för ett formulär som ska kunna hålla i spelet.
    /// </summary>
    public interface IGameForm
    {
        
        /// <summary>
        /// Ritar en händelse på ritytan.
        /// </summary>
        /// <param name="action">Händelsen att rita.</param>
        /// <param name="elementId">Händelsens element id för att antingen uppdatera eller skapa ett nytt element.</param>
        void Draw(DrawAction action, int elementId);

        /// <summary>
        /// Lägger till ett nytt chattmeddelande i chatten.
        /// </summary>
        /// <param name="sender">Spelaren som skickat meddelandet, eller null om det är servern.</param>
        /// <param name="message">Meddelandet som ska visas.</param>
        /// <param name="bold">Om meddelandet ska vara i fetstil.</param>
        /// <param name="color">Meddelandets färg.</param>
        void AddChatMessage(Player sender, string message, bool bold, Color color);

        /// <summary>
        /// Uppdaterar spelarlistan.
        /// </summary>
        /// <param name="players">Listan över de nuvarande spelarna.</param>
        void UpdatePlayerList(List<Player> players);

        /// <summary>
        /// Visar overlayen med topplistan i slutet av spelet.
        /// </summary>
        void ShowPostGameScreen();

        /// <summary>
        /// Uppdaterar spelets skick i formuläret.
        /// </summary>
        /// <param name="round">Den nuvarande rundan.</param>
        /// <param name="timeRemaining">Tiden kvar för det nuvarande statet.</param>
        /// <param name="timer">Om timern ska visas.</param>
        void UpdateGameFormState(int round, int timeRemaining, bool timer);

        /// <summary>
        /// Uppdaterar ordet i formuläret.
        /// </summary>
        /// <param name="word">Ordet som ska visas.</param>
        void UpdateWord(string word);

        /// <summary>
        /// Visar topplistan i overlayen.
        /// </summary>
        void ShowLeaderboard();

        /// <summary>
        /// Rensar ritytan.
        /// </summary>
        void ClearCanvas();

        /// <summary>
        /// Uppdaterar om overlayen ska visas eller inte.
        /// </summary>
        /// <param name="visible">Om overlayen ska visas.</param>
        void SetOverlayVisible(bool visible);

        /// <summary>
        /// Hanterar formuläret då klienten lämnat spelet.
        /// </summary>
        /// <param name="reason">Anledning från server eller default meddelande.</param>
        /// <param name="kick">Om spelaren lämnade p.g.a att hen blev kickad.   </param>
        void Disconnect(string reason, bool kick);

        /// <summary>
        /// Hanterar formuläret då klienten anslutit till spelet.
        /// </summary>
        void OnConnect();

        /// <summary>
        /// Visar overlay med ordet som spelaren ska rita då hen är ritaren.
        /// </summary>
        /// <param name="word">Ordet som ska visas.</param>
        void ShowDrawerNotice(string word);
        
        /// <summary>
        /// Uppdaterar ritverktygens synlighet.
        /// </summary>
        /// <param name="visible">Om ritverktyen ska vara synliga.</param>
        void SetDrawToolsVisible(bool visible);
        
    }
}