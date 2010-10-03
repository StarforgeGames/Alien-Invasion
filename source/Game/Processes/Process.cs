using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Processes
{
    public abstract class Process
    {
        protected bool isDead = false;
        public bool IsDead { get { return isDead; } }

        public bool IsActive { get; set; }

        public bool IsAttached { get; set; }

        protected bool isPaused = false;
        public bool IsPaused { get { return isPaused; } }

        protected bool isInitialized = false;
        public bool IsInitialized { get { return isInitialized; } }

        public Process Next { get; set; }


        public abstract void OnInitialize();

        public virtual void Kill()
        {
            isDead = true;
        }

        public virtual void Update(float deltaTime)
        {
            if (!isInitialized) {
                OnInitialize();
                isInitialized = true;
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
        }
    }

}
