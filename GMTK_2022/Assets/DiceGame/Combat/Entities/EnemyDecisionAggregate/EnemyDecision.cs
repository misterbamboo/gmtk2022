
namespace Assets.DiceGame.Combat.Entities.CombatActionAggregate
{
    public class EnemyDecision
    {
        public EnemyDecisionType Decision { get; }
        public int SourceId { get; }
        public int TargetId { get; }

        public EnemyDecision(EnemyDecisionType action, int sourceId, int targetId)
        {
            Decision = action;
            SourceId = sourceId;
            TargetId = targetId;
        }
    }
}
