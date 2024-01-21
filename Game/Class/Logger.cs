using Game.Enums;

namespace Game.Class;

public class Logger
{
    public void WelcomeMessage(Player player)
    {
        Console.Write("Введите ваше имя: ");
        string? input = Console.ReadLine();
            
        while (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Недопустимое имя! Попробуйте еще раз");
            input = Console.ReadLine();
        }

        player.Name = input;
        Console.Clear();
        Console.WriteLine("Добро пожаловать в игру!\n");
    }
    
    public void ShowHealthStatus(Player player, Enemy enemy)
    {
        Console.Write($"Ваше здоровье: ");
        SetHealthColor(player.CurrentHealth, player.MaxHealth);
        
        Console.Write($"Здоровье противника: ");
        SetHealthColor(enemy.CurrentHealth, enemy.MaxHealth);
        
        Console.WriteLine();
    }
    
    public void ShowAvaliableActions(Player player)
    {
        Console.WriteLine($"{player.Name}, выберите действие:\n" +
                          $"1. Ударить оружием (урон: {player.WeaponDamage})\n" +
                          $"2. Щит: следующая атака противника не наносит урона\n" +
                          $"3. Огненный шар: наносит урон в размере {player.FireballDamage}\n" +
                          $"4. Исцелить 100 hp (сработает только если в предыдущем ходе враг не атаковал оружием)\n");
    }

    public void UndefinedCommand()
    {
        Console.WriteLine("Команда не распознана!");
    }
    
    public void EndTurnMessage()
    {
        Console.WriteLine("\nДля продолжения нажмите любую клавишу.");
        Console.ReadKey();
        Console.Clear();
    }
    
    void SetHealthColor(float currentHealth, float maxHealth)
    {
        if (currentHealth > maxHealth * 0.1f)
            Console.ForegroundColor = ConsoleColor.Green;
        else
            Console.ForegroundColor = ConsoleColor.Red;
        
        Console.WriteLine($"{currentHealth} hp");
        Console.ResetColor();
    }
    
    void DamageMessage(string damager, string defender, float damage)
    {
        if(damage == 0)
            Console.WriteLine($"Игрок {defender} заблокировал атаку игрока {damager} щитом.");
        else
        {
            if(damager == defender)
                Console.Write($"Игрок {damager} нанёс cебе урон в размере ");
            else
                Console.Write($"Игрок {damager} нанёс игроку {defender} урон в размере ");
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{damage} hp");
            Console.ResetColor();
        }
    }

    void HealingMessage(string healer, float healedHealth)
    {
        Console.Write($"Игрок {healer} вылечил себе ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{healedHealth} hp");
        Console.ResetColor();
    }

    void WriteDamageWithType(AttackType attackType, string damager, string defender, List<float> damages)
    {
        foreach (var damage in damages)
        {
            switch (attackType)
            {
                case AttackType.Damage:
                    DamageMessage(damager, defender, damage);
                    break;
                case AttackType.Self:
                    DamageMessage(damager, damager, damage);
                    break;
                case AttackType.Heal:
                    HealingMessage(damager, damage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
            }
        }
        
        damages.Clear();
    }

    public void WriteDamageFromTo(Dictionary<AttackType, List<float>> damages, string damager, string defender)
    {
        foreach (var damage in damages)
        {
            WriteDamageWithType(damage.Key,damager, defender, damage.Value);
        }
    }
}