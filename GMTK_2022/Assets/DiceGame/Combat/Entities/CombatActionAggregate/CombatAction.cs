using System.Collections.Generic;
using System.Linq;

namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public abstract class CombatAction
    {
        public int SourceId { get; private set; }

        public IEnumerable<int> TargetIds => targetIds;
        private List<int> targetIds;

        public abstract string Description { get; }

        public abstract string IconName { get; }

        public CombatAction(int sourceId, int targetId, params int[] targetIds)
        {
            SourceId = sourceId;
            this.targetIds = targetIds.ToList();
            this.targetIds.Add(targetId);
        }
    }
}