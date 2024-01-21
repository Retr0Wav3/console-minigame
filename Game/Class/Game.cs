using Game.Enums;

namespace Game.Class;

public class Game
{
    public string? PlayerInput { get; set; }
    public Logger Logger { get; set; }
    private Dictionary<AttackType, List<float>> _damagesToPlayer = [];
    private Dictionary<AttackType, List<float>> _damagesToEnemy = [];
    
    public Game()
    {
        Logger = new Logger();
        
        _damagesToPlayer[AttackType.Damage] = [];
        _damagesToPlayer[AttackType.Self] = [];
        _damagesToPlayer[AttackType.Heal] = [];
        
        _damagesToEnemy [AttackType.Damage] = [];
        _damagesToEnemy [AttackType.Self] = [];
        _damagesToEnemy [AttackType.Heal] = [];
    }
    
    
    public void GameLoop(Player player, Enemy enemy)
    {
        Logger.WelcomeMessage(player);
        
        while (player.CurrentHealth > 0 && enemy.CurrentHealth > 0)
        {
            Logger.ShowHealthStatus(player,enemy);
            PlayerTurn(player,enemy);
            EnemyTurn(player, enemy);
        }
    }
    
   
    private void PlayerTurn(Player player, Enemy enemy)
    {
        Logger.ShowAvaliableActions(player);
        PlayerInput = Console.ReadLine();

        switch (PlayerInput)
        {
            case "1":
                Attack(player, enemy, (int)PlayerActions.AttackedWithWeapon, player.WeaponDamage);
                break;
            
            case "2":
                player.Shield = ShieldState.Active;
                player.LastAction = PlayerActions.UsedShield;
                break;
            
            case "3":
                Attack(player, enemy, (int)PlayerActions.AttackedWithAbility, player.FireballDamage);
                break;
            
            case "4":
                if (enemy.LastAction != EnemyActions.AttackedWithWeapon && enemy.LastAction != EnemyActions.WasBlocked)
                {
                    Heal(player, (int)PlayerActions.Healed, player.HealAmount);
                }
                else
                {
                    player.LastAction = PlayerActions.FailedHealing;
                    _damagesToEnemy[AttackType.Heal].Add(0f);
                }
                break;
            
            default:
                Console.Clear();
                Logger.UndefinedCommand();
                PlayerTurn(player, enemy);
                break;
        }
    }
    
    private void EnemyTurn(Player player, Enemy enemy)
    {
        int enemyCommand = new Random().Next(1, enemy.CommandCount + 1);

        switch (enemyCommand)
        {
            case 1:
                if (player.Shield == ShieldState.Unactive)
                {
                    Attack(enemy, player, (int)EnemyActions.AttackedWithWeapon, enemy.WeaponDamage);
                }
                else
                {
                    BlockAttack(player, enemy);
                }
                break;
            
            case 2:
                if (player.Shield == ShieldState.Unactive)
                {
                    Attack(enemy, player,(int)EnemyActions.AttackedWithWeapon, enemy.WeaponDamage * enemy.SecondAbiltyModifier);
                    enemy.CurrentHealth -= enemy.WeaponDamage;
                    _damagesToPlayer[AttackType.Self].Add(enemy.WeaponDamage);
                }
                else
                {
                    BlockAttack(player, enemy);
                }
                break;
            
            case 3:
                if (player.LastAction == PlayerActions.AttackedWithWeapon)
                {
                    enemy.CurrentHealth -= enemy.HealAmount;
                    enemy.LastAction = EnemyActions.DamagedHimself;
                    _damagesToPlayer[AttackType.Self].Add(enemy.HealAmount);
                }
                else
                {
                    Heal(enemy, (int)EnemyActions.Healed, enemy.HealAmount);
                }
                player.Shield = ShieldState.Unactive;
                break;
            
            default:
                Logger.UndefinedCommand();
                break;
        }
        
        Logger.WriteDamageFromTo(_damagesToEnemy, player.Name, enemy.Name);
        Logger.WriteDamageFromTo(_damagesToPlayer, enemy.Name, player.Name);
        Logger.EndTurnMessage();
    }
    
    
    private void Attack(BasePlayer damager, BasePlayer defender, int action, float damage)
    {
        defender.CurrentHealth -= damage;
        damager.SetLastAction(action);
        
        if(damager is Player)
            _damagesToEnemy[AttackType.Damage].Add(damage);
        else
            _damagesToPlayer[AttackType.Damage].Add(damage);
    }

    private void Heal(BasePlayer healer, int action, float healAmount)
    {
        healer.CurrentHealth += healAmount;
        healer.SetLastAction(action);
        
        if(healer is Player)
            _damagesToEnemy[AttackType.Heal].Add(healAmount);
        else
            _damagesToPlayer[AttackType.Heal].Add(healAmount);
    }
    
    private void BlockAttack(Player player, Enemy enemy)
    {
        enemy.LastAction = EnemyActions.WasBlocked;
        player.Shield = ShieldState.Unactive;
        _damagesToPlayer[AttackType.Damage].Add(0);
    }
}