using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.EventManagement.Events;
using Game.Entities;

namespace Game.EventManagement
{

    public class SwappingEventManager : IEventManager
    {
        private readonly int numOfQueues = 2;

        private int activeQueueId = 0;
        public List<Event> ActiveQueue {
            get { return queues[activeQueueId]; }
        }

        private Dictionary<Type, List<IEventListener>> listenerMap;
        private List<Event>[] queues;

        private GameLogic game;

        public SwappingEventManager(GameLogic game)
        {
            this.game = game;

            listenerMap = new Dictionary<Type, List<IEventListener>>();

            queues = new List<Event>[numOfQueues];
            for (int i = 0; i < numOfQueues; i++) {
                queues[i] = new List<Event>();
            }
        }

        public bool AddListener(IEventListener listener, Type eventType)
        {
            if (!listenerMap.ContainsKey(eventType)) {
                listenerMap.Add(eventType, new List<IEventListener>());
            }
            
            foreach (IEventListener l in listenerMap[eventType]) {
                if (l == listener) {
                    return false;
                }
            }

            listenerMap[eventType].Add(listener);
            return true;
        }

        public bool RemoveListener(IEventListener listener, Type eventType)
        {
            if (!listenerMap.ContainsKey(eventType)) {
                return false;
            }

            return listenerMap[eventType].Remove(listener);
        }

        public void Trigger(Event evt)
        {
            if (!listenerMap.ContainsKey(evt.GetType())) {
                return;
            }

            sendEventToGeneralListeners(evt);

            if (evt.RecipientID != 0) {
                Entity entity = game.Entities[evt.RecipientID];
                entity.SendEvent(evt);
                return;
            }

            foreach (IEventListener listener in listenerMap[evt.GetType()]) {
                listener.OnEvent(evt);
            }
        }

        private void sendEventToGeneralListeners(Event evt)
        {
            if (!listenerMap.ContainsKey(typeof(Event))) {
                return;
            }

            foreach (IEventListener listener in listenerMap[typeof(Event)]) {
                listener.OnEvent(evt);
            }

        }

        public bool QueueEvent(Event evt)
        {
            if (!listenerMap.ContainsKey(evt.GetType())) {
                return false;
            }

            ActiveQueue.Add(evt);
            return true;
        }

        public bool AbortEvent(Event evt)
        {
            return ActiveQueue.Remove(evt);
        }

        public bool Tick()
        {
            List<Event> queueToProcess = ActiveQueue;
            SwapActiveQueue();
            ActiveQueue.Clear();

            foreach (Event evt in queueToProcess) {
                Trigger(evt);
            }

            return true;
        }

        private void SwapActiveQueue()
        {
            activeQueueId = ++activeQueueId % numOfQueues;
        }
    }

}
