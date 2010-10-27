using System;
using System.Windows.Forms;
using Game;
using Game.EventManagement;
using Game.EventManagement.Events;

namespace SpaceInvaders.Menus
{
    public partial class GameMainMenu : UserControl
    {
        private IEventManager eventManager { get; set; }

        public GameMainMenu(IEventManager eventManager)
        {
            InitializeComponent();

            this.eventManager = eventManager;
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            eventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED, 
                GameState.Loading));
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            eventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED, 
                GameState.Quit));
        }
    }
}
