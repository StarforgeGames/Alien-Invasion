using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game;
using Game.EventManagement.Events;

namespace SpaceInvaders.Menus
{
    public partial class GameMainMenu : UserControl
    {
        public GameLogic Game { get; set; }

        public GameMainMenu(GameLogic game)
        {
            InitializeComponent();

            this.Game = game;
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Game.ChangeState(GameState.Loading);
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Game.ChangeState(GameState.Quit);
        }
    }
}
