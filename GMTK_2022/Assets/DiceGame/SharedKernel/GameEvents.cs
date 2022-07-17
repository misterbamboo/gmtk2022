using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DiceGame.SharedKernel
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents Instance { get; private set; }

        public static void Subscribe<T>(Action<T> subscriberAction)
           where T : IGameEvent
        {
            Instance.Init<T>();
            Instance.SubscribeByType<T>((e) => subscriberAction((T)e));
        }

        public static void Raise<T>(T gameEvent)
            where T : IGameEvent
        {
            Instance.pendingGameEvents.Enqueue(gameEvent);
        }

        public Dictionary<Type, List<Action<IGameEvent>>> subscribers = new();

        public Queue<IGameEvent> pendingGameEvents = new Queue<IGameEvent>();

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if(pendingGameEvents.Count > 0)
            {
                var nextGameEvent = pendingGameEvents.Dequeue();
                RaisePendingEvent(nextGameEvent);
            }
        }

        private void Init<T>() where T : IGameEvent
        {
            var type = typeof(T);
            if (!subscribers.ContainsKey(type))
            {
                subscribers[type] = new List<Action<IGameEvent>>();
            }
        }

        private void SubscribeByType<T>(Action<IGameEvent> subscriberAction)
            where T : IGameEvent
        {
            var type = typeof(T);
            var list = subscribers[type];
            list.Add(subscriberAction);
        }

        private void RaisePendingEvent(IGameEvent gameEvent)
        {
            Debug.Log($"Raise: {gameEvent} ({Time.realtimeSinceStartup})");

            var type = gameEvent.GetType();
            if (subscribers.ContainsKey(type))
            {
                var list = subscribers[type];
                foreach (var subscriber in list)
                {
                    subscriber.Invoke(gameEvent);
                }
            }
        }
    }
}
