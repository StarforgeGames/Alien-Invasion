using System;
using System.Runtime.InteropServices;

namespace Utility
{
    /// <summary>
    /// Class that keeps track of in-game time, taking pausing and stopping the game into account.
    /// </summary>
    public class GameClock
    {
        /// <summary>
        /// Returns the current game time since start.
        /// </summary>
        public float Time {
            get 
            {
                if (isStopped) 
                {
                    return (float)((stopTime - baseTime) * secondsPerCount);
                }
                else 
                {
                    return (float)((currentTime - pausedTime - baseTime) * secondsPerCount);
                }
            }
        }

        /// <summary>
        /// Returns the time difference since the last tick.
        /// </summary>
        public float DeltaTime { get; set; }

        private float timeScale;
        public float TimeScale { 
            get { 
                return timeScale; 
            }
            set
            {
                timeScale = value;
                if (value < 0.0f) 
                {
                    timeScale = 0.0f;
                }
            }
        }

        private double secondsPerCount;

        private Int64 baseTime;
        private Int64 pausedTime;
        private Int64 stopTime;
        private Int64 previousTime;
        private Int64 currentTime;

        private bool isStopped;
        private bool doSingleStep;

        private readonly float singleStep = 1.0f / 30.0f;
        private readonly float debugBreakpointThreshold = 1.0f / 10.0f;

        public GameClock()
            : this(1.0f)
        { }

        public GameClock(float timeScale)
        {
            this.DeltaTime = -1.0f;
            this.TimeScale = timeScale;

            long countsPerSec;
            QueryPerformanceFrequency(out countsPerSec);
            secondsPerCount = 1.0 / (double)countsPerSec;
        }

        /// <summary>
        /// Resets the GameTimer to default values.
        /// </summary>
        public void Reset()
        {
            long currTime;
            QueryPerformanceCounter(out currTime);

            baseTime = currTime;
            previousTime = currTime;
            stopTime = 0;
            isStopped = false;
        }

        /// <summary>
        /// Starts the GameTimer.
        /// </summary>
        public void Start()
        {
            long startTime;
            QueryPerformanceCounter(out startTime);

            if (isStopped) 
            {
                pausedTime += (startTime - stopTime);

                previousTime = startTime;
                stopTime = 0;
                isStopped = false;
            }
        }

        /// <summary>
        /// Stops the GameTimer.
        /// </summary>
        public void Stop()
        {
            if (!isStopped) 
            {
                long currTime;
                QueryPerformanceCounter(out currTime);

                stopTime = currTime;
                isStopped = true;
            }
        }

        /// <summary>
        /// Calculates the time values for the current tick, if GameTimer is not stopped.
        /// </summary>
        public void Tick()
        {
            if (isStopped || timeScale <= 0.0f) 
            {
                if (doSingleStep) 
                {
                    DeltaTime = singleStep;
                    doSingleStep = false;
                    return;
                }

                DeltaTime = 0.0f;
                return;
            }

            long currTime;
            QueryPerformanceCounter(out currTime);
            currentTime = currTime;
            
            float currentDelta;
            currentDelta = (float)((currentTime - previousTime) * secondsPerCount) * timeScale;
            previousTime = currentTime;

            // Force non-negative.  The DXSDK's CDXUTTimer mentions that if the processor goes into a power save mode 
            // or we get shuffled to another processor, then DeltaTime can be negative.
            if (currentDelta < 0.0f) 
            {
                DeltaTime = 0.0f;
            }
            else if(currentDelta > debugBreakpointThreshold) 
            {
                DeltaTime = singleStep;
            }
            else
            {
                DeltaTime = currentDelta;
            }
        }

        public void SingleStep()
        {
            doSingleStep = true;
        }

        /// <summary>
        /// Use Windows API functions as they have a higher resolution which is preferable for Direct3D Applications.
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/en-us/library/aa964692%28VS.80%29.aspx"/>
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

    }
}
