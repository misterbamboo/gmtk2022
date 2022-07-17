using System.Collections.Generic;
using System.Linq;
using DiceGame.Combat.Entities.EnemyAggregate;
using DiceGame.Combat.Events;
using DiceGame.SharedKernel;
using UnityEngine;

namespace DiceGame.Combat.Entities.CharacterAggregate
{
    public abstract class Character
    {
        protected int id;
        protected ICharacterStats stats;
        protected List<StatusEffect> activeStatusEffects = new List<StatusEffect>();
        protected int currentHealth;
        protected int currentArmor;

        public Character(int id, ICharacterStats characterStats)
        {
            this.id = id;
            stats = characterStats;
            currentHealth = characterStats.MaxLife;
        }

        public int Id => id;
        public int CurrentHealth => currentHealth;
        public float MaxLife => stats.MaxLife;
        public bool IsDead => currentHealth <= 0;

        private void TakeDamage(int amount)
        {
            currentHealth = Mathf.Clamp(0, stats.MaxLife, currentHealth - amount);
            GameEvents.Raise(new CharacterTookDamageEvent(id, amount));

            if (IsDead)
            {
                GameEvents.Raise(new CharacterKilledEvent(id));
            }
        }

        private void TakeHeal(int amount)
        {
            currentHealth = Mathf.Clamp(0, stats.MaxLife, currentHealth + amount);
            GameEvents.Raise(new CharacterGotHealedEvent(id, amount));
        }

        private void TakeShield(int amount)
        {
            currentArmor += amount;
            GameEvents.Raise(new CharacterGotShieldedEvent(id, amount));
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
            var action = new HealAction(heal, id, id);

            // GameEvents.SendHealAction(action);
        }

        protected void TakeShieldSelfAction(Shield shield = null)
        {
            shield = shield ?? new Shield(stats.Defence);
            shield = OnShieldingPipeline(shield);
            var action = new ShieldAction(shield, id, id);

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

        public void ReceiveAction(CombatAction combatAction)
        {
            switch (combatAction)
            {
                case AttackAction attackAction:
                    ReceiveAttack(attackAction.Attack);
                    break;
                case HealAction healAction:
                    ReceiveHealAction(healAction.Heal);
                    break;
                case ShieldAction shieldAction:
                    ReceiveShieldAction(shieldAction.Shield);
                    break;
                default:
                    break;
            }
        }

        protected virtual void ReceiveAttack(Attack attack)
        {
            attack = OnReceiveAttackPipeline(attack);
            TakeStatusEffects(attack.StatusEffects);
            TakeDamage(attack.Amount);
        }

        protected virtual void ReceiveHealAction(Heal heal)
        {
            heal = OnReceiveHealingPipeline(heal);
            TakeHeal(heal.Amount);
        }

        protected virtual void ReceiveShieldAction(Shield shield)
        {
            shield = OnReceiveShieldPipeline(shield);
            TakeShield(shield.Amount);
        }

        #endregion
    }
}