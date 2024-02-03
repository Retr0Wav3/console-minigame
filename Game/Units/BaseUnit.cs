using Game.Systems;
using Game.Enums;

namespace Game.Units
{
    public abstract class BaseUnit
    { 
        public string? Name { get; set; }
        public Health Health { get; protected set; }
        public float WeaponDamage { get; protected set; }
        public EUnitAction LastAction { get; protected set; }
        public Dictionary<ERecordType, List<float>> DamageHistory;
        public List<string> ActionsDescriptions;
        
        
        protected void InitDamageHistory(Dictionary<ERecordType, List<float>> damageHistory)
        {
            damageHistory[ERecordType.DamageToEnemy] = [];
            damageHistory[ERecordType.SelfInflictedDamage] = [];
            damageHistory[ERecordType.SelfHealing] = [];
        }
        
        public abstract void Attack(BaseUnit target, float damage, EAttackType attackType);
        
        public abstract void Heal(float healAmount, EUnitAction enemyLastUnitAction);
        
        public abstract EDamageType GetDamageType(EAttackType attackType);
        
        public virtual void AddDescriptions() { }
        
        
        public string GetActionDescription(int abilityNumber)
        {
            return ActionsDescriptions[abilityNumber];
        }
        
        
        public int GetActionsCount()
        {
            return ActionsDescriptions.Count;
        }
        
        
        public void TakeDamage(float damageAmount, EDamageType damageType)
        {
            Health.UpdateHealth(damageAmount, damageType);
        }
    }
}