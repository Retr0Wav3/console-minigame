namespace Game.Class;

public abstract class BasePlayer
{
    public abstract float CurrentHealth { get; set; }
    public abstract void SetLastAction(int action);
    public string? Name { get; set; }
    public float MaxHealth { get; protected set; }
    public float WeaponDamage { get; protected set; }
    public float HealAmount { get; protected set; }
}