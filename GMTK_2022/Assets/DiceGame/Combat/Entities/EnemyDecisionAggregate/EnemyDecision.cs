
namespace Assets.DiceGame.Combat.Entities.CombatActionAggregate
{
    public class EnemyDecision
    {
        private EnemyDecisionType Decision { get; }
        private int SourceId { get; }
        private int TargetId { get; }

        public EnemyDecision(EnemyDecisionType action, int sourceId, int targetId)
        {
            Decision = action;
            SourceId = sourceId;
            TargetId = targetId;
        }
    }
}
