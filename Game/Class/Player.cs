using Game.Enums;

namespace Game.Class;

public class Player : BasePlayer
{
    public PlayerActions LastAction { get; set; }
    public ShieldState Shield { get; set; }
    public override float CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (value > MaxHealth)
                _currentHealth = MaxHealth;
            else
                _currentHealth = value;
        }
    }
    public float FireballDamage { get; private set; }
    private float _currentHealth;
    
    public Player()
    {
        MaxHealth = 1000f;
        _currentHealth = MaxHealth;
        Shield = ShieldState.Unactive;
        LastAction = default;
        WeaponDamage = 50f;
        FireballDamage = 200f;
        HealAmount = 100f;
    }

    public override void SetLastAction(int action)
    {
        LastAction = (PlayerActions)action;
    }
}