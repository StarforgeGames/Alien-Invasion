using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Processes;

namespace Game
{

    public class ProcessManager
    {
        private List<Process> processes;

        public ProcessManager()
        {
            processes = new List<Process>();
        }

        public void Attach(Process process)
        {
            processes.Add(process);
            process.IsAttached = true;
        }

        public void Detach(Process process)
        {
            processes.Remove(process);
            process.IsAttached = false;
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Process p in processes) {
                if (p.IsDead) {
                    if (p.Next != null) {
                        Attach(p.Next);
                        p.Next = null;
                    }

                    Detach(p);
                }
                else if(p.IsActive && !p.IsPaused) {
                    p.Update(deltaTime);
                }
            }
        }
    }

}
