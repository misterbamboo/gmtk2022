using Assets.DiceGame.Combat.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DiceGame.SharedKernel
{
    public static class GameEvents
    {
        public static Dictionary<Type, List<Action<IGameEvent>>> subscribers = new();

        public static void Subscribe<T>(Action<T> subscriberAction)
           where T : IGameEvent
        {
            Init<T>();
            SubscribeByType<T>((e) => subscriberAction((T)e));
        }

        private static void Init<T>() where T : IGameEvent
        {
            var type = typeof(T);
            if (!subscribers.ContainsKey(type))
            {
                subscribers[type] = new List<Action<IGameEvent>>();
            }
        }

        private static void SubscribeByType<T>(Action<IGameEvent> subscriberAction)
            where T : IGameEvent
        {
            var type = typeof(T);
            var list = subscribers[type];
            list.Add(subscriberAction);
        }

        public static void Raise<T>(T gameEvent)
            where T : IGameEvent
        {
            var type = typeof(T);
            Debug.Log($"Raise: {gameEvent} ({Time.realtimeSinceStartup})");

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
