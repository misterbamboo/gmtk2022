
namespace Assets.DiceGame.Combat.Entities.CombatActionAggregate
{
    public class CombatAction
    {
        private CombatActionType Action { get; }
        private int SourceId { get; }
        private int TargetId { get; }

        public CombatAction(CombatActionType action, int sourceId, int targetId)
        {
            Action = action;
            SourceId = sourceId;
            TargetId = targetId;
        }
    }
}
