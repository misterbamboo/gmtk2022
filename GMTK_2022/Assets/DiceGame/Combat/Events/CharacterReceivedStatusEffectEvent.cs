using System;
using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class CharacterReceivedStatusEffectEvent : IGameEvent
    {
        public int CharacterId { get; }
        public Type StatusEffectType { get; }

        public CharacterReceivedStatusEffectEvent(int CharacterId, Type StatusEffectType)
        {
        }
    }
}