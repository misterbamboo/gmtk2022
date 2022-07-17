using DiceGame.SharedKernel;
using DiceGame.Combat.Entities.CharacterAggregate;

namespace DiceGame.Combat.Events
{
    public class CombatActionSentEvent : IGameEvent
    {
        public CombatAction CombatAction { get; }

        public CombatActionSentEvent(CombatAction combatAction)
        {
            CombatAction = combatAction;
        }

        public override string ToString()
        {
            var targets = string.Join(',', CombatAction.TargetIds);
            return $"{GetType().Name}: CombatAction={CombatAction.GetType().Name}, SourceId={CombatAction.SourceId}, TargetIds={targets}";
        }
    }
}
