using System;
using DiceGame.SharedKernel;

namespace DiceGame.Combat.Events
{
    public class StatusEffectExpiredEvent : IGameEvent
    {
        public int CharacterId { get; }
        public Type StatusEffectType { get; }

        public StatusEffectExpiredEvent(int characterId, Type statusEffectType)
        {
            CharacterId = characterId;
            StatusEffectType = statusEffectType;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: CharacterId={CharacterId}, StatusEffectType={StatusEffectType}";
        }
    }
}