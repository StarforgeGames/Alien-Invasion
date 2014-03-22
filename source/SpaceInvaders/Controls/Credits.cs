using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game.EventManagement;
using Game.EventManagement.Events;
using Game;

namespace SpaceInvaders.Controls
{
	public partial class Credits : UserControl
	{
		public bool IsRunning { get; private set; }

		private IEventManager eventManager;
		private float timeSinceLastMove;

		public Credits(IEventManager eventManager)
		{
			this.eventManager = eventManager;
			InitializeComponent();

			Stop();
		}

		public void Start()
		{
			IsRunning = true;
		}

		public void Stop()
		{
			IsRunning = false;
			creditsPanel.Location = new Point(creditsPanel.Location.X, 770);

			eventManager.Queue(GameStateChangedEvent.To(GameState.Menu));
		}

		public void OnUpdate(float deltaTime)
		{
			if (!IsRunning)
			{
				return;
			}

			timeSinceLastMove += deltaTime;
			if (timeSinceLastMove < 0.015f)
			{
				return;
			}
			timeSinceLastMove = 0.0f;

			creditsPanel.Location = new Point(creditsPanel.Location.X, creditsPanel.Location.Y - 1);

			if (creditsPanel.Location.Y < -creditsPanel.Height - 100)
			{
				Stop();
			}
		}
	}
}
