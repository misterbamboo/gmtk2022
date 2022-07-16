﻿using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.Combat.Events
{
    public class EnemyUnselectedEvent : IGameEvent
    {
        public int Id { get; }

        public EnemyUnselectedEvent(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: EnemyId={Id}";
        }
    }
}
