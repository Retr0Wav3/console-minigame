using Game.Enums;

namespace Game.Class;

public class Enemy : BasePlayer
{
    public EnemyActions LastAction { get; set; }
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
    public int CommandCount { get; private set; }
    public int SecondAbiltyModifier { get; private set; }
    private float _currentHealth;

    public Enemy()
    {
        Name = "Enemy";
        MaxHealth = 2000f;
        _currentHealth = MaxHealth;
        LastAction = default;
        WeaponDamage = 55f;
        HealAmount = 100f;
        CommandCount = 3;
        SecondAbiltyModifier = 3;
    }

    public override void SetLastAction(int action)
    {
        LastAction = (EnemyActions)action;
    }
}