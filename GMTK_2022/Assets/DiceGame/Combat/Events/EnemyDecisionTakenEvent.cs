using Assets.DiceGame.Combat.Entities.CombatActionAggregate;
using Assets.DiceGame.SharedKernel;

namespace Assets.DiceGame.Combat.Events
{
    public class EnemyDecisionTakenEvent : IGameEvent
    {
        public EnemyDecision EnemyDecision { get; }

        public EnemyDecisionTakenEvent(EnemyDecision enemyDecision)
        {
            EnemyDecision = enemyDecision;
        }

        public override string ToString()
        {
            return $"{GetType().Name}: Decision={EnemyDecision.Decision}, FromId={EnemyDecision.SourceId}, ToId={EnemyDecision.TargetId}";
        }
    }
}
