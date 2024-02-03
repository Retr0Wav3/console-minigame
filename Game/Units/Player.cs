using Game.Enums;
using Game.Systems;

namespace Game.Units
{
    public class Player : BaseUnit
    {
        private float _abilityDamage;
        private int _shieldCount;
        public float AbilityDamage => _abilityDamage;
        public int ShieldCount => _shieldCount;
    
        public Player(float maxHealth, float weaponDamage, float abilitylDamage, float healAmount)
        {
            Health = new Health(maxHealth, healAmount);
            DamageHistory = [];
            ActionsDescriptions = [];
            WeaponDamage = weaponDamage;
            _shieldCount = 3;
            _abilityDamage = abilitylDamage;
            InitDamageHistory(DamageHistory);
            AddDescriptions();
        }

        public override void Attack(BaseUnit target, float damage, EAttackType attackType)
        {
            EDamageType damageType = GetDamageType(attackType);
            target.TakeDamage(damage, damageType);
            DamageHistory[ERecordType.DamageToEnemy].Add(damage);
            
            if (attackType == EAttackType.Weapon)
                LastAction = EUnitAction.AttackWithWeapon;
            else
                LastAction = EUnitAction.AttackWithAbility;
        }

        public override void Heal(float healAmount, EUnitAction enemyLastAction)
        {
            if (enemyLastAction == EUnitAction.AttackWithWeapon)
            {
                DamageHistory[ERecordType.SelfHealing].Add(0f);
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
                case EAttackType.Weapon:
                    return EDamageType.Physical;
                case EAttackType.Ability:
                    return EDamageType.Elemental;
                
                default:
                    return EDamageType.Physical;
            }
        }
        
        public override void AddDescriptions()
        {
            ActionsDescriptions.Add($"Ударить оружием (урон: {WeaponDamage})");
            ActionsDescriptions.Add($"Блокировать атаку щитом (следующая атака противника не наносит урон)");
            ActionsDescriptions.Add($"Огненный шар (урон: {AbilityDamage})");
            ActionsDescriptions.Add($"Исцелить: {Health.HealAmount} hp (Если противник в прошлом раунде атаковал оружием - исцеление не сработает)");
        }
        
        public void BlockAttack()
        {
            if (_shieldCount > 0)
            {
                _shieldCount--;
                LastAction = EUnitAction.DefendWithShield;
                return;
            }
            
            LastAction = EUnitAction.Idle;
        }
    }
}