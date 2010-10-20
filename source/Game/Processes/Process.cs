namespace Game.Processes
{
    public abstract class Process
    {
        public bool IsActive { get; set; }
        public bool IsPaused { get; private set; }
        public bool IsDead { get; private set; }

        public bool IsInitialized { get; private set; }
        public bool IsAttached { get; set; }

        public Process Next { get; set; }

        public Process()
        {
            IsActive = true;
        }

        public abstract void OnInitialize();

        public virtual void Kill()
        {
            IsDead = true;
        }

        public virtual void Update(float deltaTime)
        {
            if (!IsInitialized) {
                OnInitialize();
                IsInitialized = true;
            }
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
        }
    }

}
