using Game.Enums;

namespace Game.Systems
{
    public class Health
    {
        public float MaxHealth => _maxHealth; 
        public float CurrentHealth => _currentHealth;
        public float HealAmount => _healAmount;
        
        private float _maxHealth;
        private float _currentHealth;
        private float _healAmount;
        
        public Health(float maxHealth, float healAmount)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _healAmount = healAmount;
        }

        public void UpdateHealth(float amount, EDamageType damageType)
        {
            if (damageType == EDamageType.Physical || damageType == EDamageType.Elemental)
                _currentHealth -= amount;
            else if (damageType == EDamageType.Healing)
                _currentHealth += amount;
            
            _currentHealth = Math.Clamp(_currentHealth, 0f, _maxHealth);
        }
    
    
    }
}