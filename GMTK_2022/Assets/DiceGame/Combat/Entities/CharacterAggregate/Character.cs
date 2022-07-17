using System.Collections.Generic;
using System.Linq;
using Assets.DiceGame.Combat.Entities.EnemyAggregate;

namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public abstract class Character
    {
        private int id;
        private CharacterStats stats;
        protected List<StatusEffect> activeStatusEffects;
        protected int currentHealth;
        protected int currentArmor;

        private void TakeDamage(int amount)
        {
            currentHealth -= amount;
            // GameEvents.TakeDamage(id, amount);
        }

        private void TakeHeal(int amount)
        {
            currentHealth += amount;
            // GameEvents.Heal(id, amount);
        }

        private void TakeShield(int amount)
        {
            currentArmor += amount;

            // GameEvents.Shield(id, amount);
        }

        private void TakeStatusEffects(IEnumerable<StatusEffect> statusEffects)
        {
            foreach (var statusEffect in statusEffects)
            {
                if (activeStatusEffects.Any(se => se.GetType() == statusEffect.GetType()))
                {
                    var se = activeStatusEffects.First(se => se.GetType() == statusEffect.GetType());
                    se.Refresh(statusEffect.duration);
                }
                else
                {
                    statusEffect.Apply(this);
                    activeStatusEffects.Add(statusEffect);
                }

                // GameEvents.ReceiveStatusStatusEffect(id, statusEffect);
            }
        }

        #region TakeActions
        protected void TakeAttackAction(int targetId, Attack attack = null)
        {
            attack = attack ?? new Attack(stats.Attack);
            attack = OnAttackingPipeline(attack);
            var action = new AttackAction(attack, id, targetId);

            // GameEvents.SendAttackAction(action);
        }

        protected void TakeHealSelfAction(Heal heal = null)
        {
            heal = heal ?? new Heal(stats.Heal);
            heal = OnHealingPipeline(heal);
            var action = new HealSelfAction(heal, id);

            // GameEvents.SendHealAction(action);
        }

        protected void TakeShieldSelfAction(Shield shield = null)
        {
            shield = shield ?? new Shield(stats.Defence);
            shield = OnShieldingPipeline(shield);
            var action = new ShieldSelfAction(shield, id);

            // GameEvents.SendShieldAction(action);
        }
        #endregion

        #region PipeLines
        protected Attack OnAttackingPipeline(Attack attack)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                attack = statusEffect.OnAttacking(attack);
            }

            return attack;
        }

        protected Attack OnReceiveAttackPipeline(Attack attack)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                attack = statusEffect.OnReceiveAttack(attack);
            }

            return attack;
        }

        protected Heal OnHealingPipeline(Heal heal)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                heal = statusEffect.OnHealing(heal);
            }

            return heal;
        }

        protected Heal OnReceiveHealingPipeline(Heal heal)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                heal = statusEffect.OnHealing(heal);
            }

            return heal;
        }

        protected Shield OnShieldingPipeline(Shield shield)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                shield = statusEffect.OnShielding(shield);
            }

            return shield;
        }

        protected Shield OnReceiveShieldPipeline(Shield shield)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                shield = statusEffect.OnReceiveShield(shield);
            }

            return shield;
        }

        #endregion

        #region ReceiveActions
        public void ReceiveAttack(Attack attack)
        {
            attack = OnReceiveAttackPipeline(attack);
            TakeStatusEffects(attack.StatusEffects);
            TakeDamage(attack.Amount);
        }

        public void ReceiveHealAction(Heal heal)
        {
            heal = OnReceiveHealingPipeline(heal);
            TakeHeal(heal.Amount);
        }

        public void ReceiveShieldAction(Shield shield)
        {
            shield = OnReceiveShieldPipeline(shield);
            TakeShield(shield.Amount);
        }

        #endregion
    }
}