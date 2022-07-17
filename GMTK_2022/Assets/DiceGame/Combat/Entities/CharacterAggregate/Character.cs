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
        public int CurrentArmor => currentArmor;
        public float MaxLife => stats.MaxLife;
        public bool IsDead => currentHealth <= 0;
        public IEnumerable<StatusEffect> ActiveStatusEffects => activeStatusEffects;
        public ICharacterStats Stats => stats;

        public void TakeDamage(int amount)
        {
            OnReceiveDamagePipeline(amount);
            if (currentArmor > 0)
            {
                var damageLeft = amount - currentArmor;
                if (damageLeft <= 0)
                {
                    currentArmor -= amount;
                }
                else
                {
                    currentArmor = 0;
                    currentHealth = Mathf.Clamp(currentHealth - damageLeft, 0, stats.MaxLife);
                }
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth - amount, 0, stats.MaxLife);
            }
            GameEvents.Raise(new CharacterTookDamageEvent(id, amount));

            if (IsDead)
            {
                GameEvents.Raise(new CharacterKilledEvent(id));
            }
        }

        public void TakeHeal(int amount)
        {
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, stats.MaxLife);
            GameEvents.Raise(new CharacterGotHealedEvent(id, amount));
        }

        public void TakeShield(int amount)
        {
            currentArmor += amount;
            GameEvents.Raise(new CharacterGotShieldedEvent(id, amount));
        }

        public void TakeStatusEffects(IEnumerable<StatusEffect> statusEffects)
        {
            foreach (var statusEffect in statusEffects)
            {
                if (activeStatusEffects.Any(se => se.GetType() == statusEffect.GetType()))
                {
                    var se = activeStatusEffects.First(se => se.GetType() == statusEffect.GetType());
                    se.Refresh(statusEffect.Duration);
                }
                else
                {
                    statusEffect.Apply(this);
                    activeStatusEffects.Add(statusEffect);
                }

                GameEvents.Raise<CharacterReceivedStatusEffectEvent>(new CharacterReceivedStatusEffectEvent(id, statusEffect.GetType()));
            }
        }

        #region TakeActions
        protected void TakeAttackAction(int targetId, Attack attack = null)
        {
            attack = attack ?? new Attack(stats.Attack);
            attack = OnAttackingPipeline(attack);
            var action = new AttackAction(attack, id, targetId);

            GameEvents.Raise<CombatActionSentEvent>(new CombatActionSentEvent(action));
        }

        protected void TakeHealAction(int targetId = -1, Heal heal = null)
        {
            targetId = targetId == -1 ? id : targetId;
            heal = heal ?? new Heal(stats.Heal);
            heal = OnHealingPipeline(heal);
            var action = new HealAction(heal, id, targetId);

            GameEvents.Raise<CombatActionSentEvent>(new CombatActionSentEvent(action));
        }

        protected void TakeShieldAction(int targetId = -1, Shield shield = null)
        {
            targetId = targetId == -1 ? id : targetId;
            shield = shield ?? new Shield(stats.Defence);
            shield = OnShieldingPipeline(shield);
            var action = new ShieldAction(shield, id, targetId);

            GameEvents.Raise<CombatActionSentEvent>(new CombatActionSentEvent(action));
        }
        #endregion

        #region PipeLines

        protected Attack OnAttackingPipeline(Attack attack)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                attack = statusEffect.OnAttacking(attack);
            }
            RemoveExpiredStatusEffects();

            return attack;
        }

        protected Attack OnReceiveAttackPipeline(Attack attack)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                attack = statusEffect.OnReceiveAttack(attack);
            }
            RemoveExpiredStatusEffects();

            return attack;
        }

        protected Heal OnHealingPipeline(Heal heal)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                heal = statusEffect.OnHealing(heal);
            }
            RemoveExpiredStatusEffects();

            return heal;
        }

        protected Heal OnReceiveHealingPipeline(Heal heal)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                heal = statusEffect.OnHealing(heal);
            }
            RemoveExpiredStatusEffects();

            return heal;
        }

        protected Shield OnShieldingPipeline(Shield shield)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                shield = statusEffect.OnShielding(shield);
            }
            RemoveExpiredStatusEffects();

            return shield;
        }

        protected Shield OnReceiveShieldPipeline(Shield shield)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                shield = statusEffect.OnReceiveShield(shield);
            }
            RemoveExpiredStatusEffects();

            return shield;
        }

        protected void OnReceiveDamagePipeline(int amount)
        {
            foreach (var statusEffect in activeStatusEffects)
            {
                statusEffect.OnReceiveDamage(amount);
            }
            RemoveExpiredStatusEffects();
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

            RemoveExpiredStatusEffects();
        }

        protected virtual void ReceiveHealAction(Heal heal)
        {
            heal = OnReceiveHealingPipeline(heal);
            RemoveExpiredStatusEffects();
            TakeHeal(heal.Amount);
        }

        protected virtual void ReceiveShieldAction(Shield shield)
        {
            shield = OnReceiveShieldPipeline(shield);
            RemoveExpiredStatusEffects();
            TakeShield(shield.Amount);
        }

        protected void RemoveExpiredStatusEffects()
        {
            foreach (var item in activeStatusEffects.Where(se => se.IsExpired))
            {
                GameEvents.Raise<StatusEffectExpiredEvent>(new StatusEffectExpiredEvent(id, item.GetType()));
            }

            activeStatusEffects.RemoveAll(se => se.IsExpired);
        }

        #endregion
    }
}