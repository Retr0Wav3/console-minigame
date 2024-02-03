using Game.Enums;
using Game.Systems;

namespace Game.Units
{
    public class Enemy : BaseUnit
    {
        private int _commandCount;
        private int _secondAbiltyModifier;
        
        public int CommandCount => _commandCount;
        public int SecondAbiltyModifier => _secondAbiltyModifier;

        public Enemy(float maxHealth, float weaponDamage, float healAmount)
        {
            Name = "Enemy";
            Health = new Health(maxHealth, healAmount);
            WeaponDamage = weaponDamage;
            DamageHistory = [];
            _commandCount = 3;
            _secondAbiltyModifier = 3;
            InitDamageHistory(DamageHistory);
        }
        
        public override void Attack(BaseUnit target, float damage, EAttackType attackType)
        {
            if (target.LastAction == EUnitAction.DefendWithShield)
            {
                DamageHistory[ERecordType.DamageToEnemy].Add(0f);
                return;
            }
            
            EDamageType damageType = GetDamageType(attackType);

            if (attackType == EAttackType.Weapon)
            {
                target.TakeDamage(damage, damageType);
                DamageHistory[ERecordType.DamageToEnemy].Add(damage);
            }
            else if (attackType == EAttackType.Ability)
            {
                TakeDamage(damage, EDamageType.Physical);
                target.TakeDamage(damage * SecondAbiltyModifier, damageType);
                
                DamageHistory[ERecordType.SelfInflictedDamage].Add(damage);
                DamageHistory[ERecordType.DamageToEnemy].Add(damage * SecondAbiltyModifier);
            }
            
            LastAction = EUnitAction.AttackWithWeapon;
        }

        public override void Heal(float healAmount, EUnitAction enemyLastAction)
        {
            if (enemyLastAction == EUnitAction.AttackWithWeapon)
            {
                TakeDamage(healAmount, EDamageType.Healing);
                DamageHistory[ERecordType.SelfInflictedDamage].Add(healAmount);
                LastAction = EUnitAction.Idle;
                return;
            }
           
            Health.UpdateHealth(healAmount, EDamageType.Healing);
            DamageHistory[ERecordType.SelfHealing].Add(healAmount);
            LastAction = EUnitAction.Heal;
        }
        
        public override EDamageType GetDamageType(EAttackType attackType)
        {
            switch (attackType)
            {
                default:
                    return EDamageType.Physical;
            }
        }
    }
}