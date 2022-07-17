
namespace DiceGame.Combat.Entities.CombatActionAggregate
{
    public class EnemyDecisionObsolete
    {
        public EnemyDecisionType Decision { get; }
        public int SourceId { get; }
        public int TargetId { get; }

        public EnemyDecisionObsolete(EnemyDecisionType action, int sourceId, int targetId)
        {
            Decision = action;
            SourceId = sourceId;
            TargetId = targetId;
        }
    }
}
