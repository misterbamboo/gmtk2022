
using DiceGame.Combat.Entities.EnemyAggregate;
using DiceGame.Combat.Entities.CharacterAggregate;

namespace DiceGame.Combat.Entities
{
    public class Player : Character
    {
        public const int PlayerId = 0;

        public Player(ICharacterStats stats)
            : base(PlayerId, stats)
        {
        }

        public void TakeAction(Dice diceSelected, int targetId)
        {
            for (int i = 0; i < diceSelected.Value; i++)
            {
                TakeAttackAction(targetId);
            }
        }
    }
}
