using Game.Enums;
using Game.Units;

namespace Game.ConsoleOutput
{
    public class Logger
    {
        public void WelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("Добро пожаловать в игру!\n");
        }

        public void RequestPlayerName()
        {
            Console.Write("Введите ваше имя: ");
        }

        public void ShowPlayersStatus(Player player, Enemy enemy)
        {
            Console.Write($"Ваше здоровье: ");
            SetHealthColor(player);
            Console.Write($"Ваши щиты: ");
            SetShieldsColor(player.ShieldCount);

            Console.WriteLine();
            
            Console.Write($"Здоровье противника: ");
            SetHealthColor(enemy);

            Console.WriteLine();
        }

        public void ShowAvaliableActions(BaseUnit unit)
        {
            Console.WriteLine($"{unit.Name}, выберите действие:");
            
            for (int i = 0; i < unit.GetActionsCount(); i++)
            {
                Console.WriteLine($"{i + 1}. {unit.GetActionDescription(i)}");
            }
            
            Console.WriteLine();
        }

        public void UndefinedCommand()
        {
            Console.WriteLine("Команда не распознана! Введите команду ещё раз:\n");
        }
        
        public void InvalidUsername()
        {
            Console.WriteLine("\nНедопустимое имя! Попробуйте еще раз:");
        }

        public void EndTurnMessage()
        {
            Console.WriteLine("\nДля продолжения нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
        }

        void SetHealthColor(BaseUnit unit)
        {
            float maxHealth = unit.Health.MaxHealth;
            float currentHealth = unit.Health.CurrentHealth;

            if (currentHealth > maxHealth * 0.1f)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"{currentHealth} hp");
            Console.ResetColor();
        }
        
        void SetShieldsColor(int shieldsCount)
        {
            if (shieldsCount > 1)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;
            
            Console.WriteLine($"{shieldsCount}");
            Console.ResetColor();
        }

        void DamageMessage(string damager, string defender, float damage)
        {
            if (damage == 0)
                Console.WriteLine($"Игрок {defender} заблокировал атаку игрока {damager} щитом.");
            else
            {
                if (damager == defender)
                    Console.Write($"Игрок {damager} нанёс cебе урон в размере: ");
                else
                    Console.Write($"Игрок {damager} нанёс игроку {defender} урон в размере: ");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{damage} hp");
                Console.ResetColor();
            }
        }

        void HealingMessage(string healer, float healedHealth)
        {
            if (healedHealth == 0)
            {
                Console.WriteLine($"Игрок {healer} провалил исцеление.");
                return;
            }
            
            Console.Write($"Игрок {healer} вылечил себе: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{healedHealth} hp");
            Console.ResetColor();
        }

        void WriteDamageWithType(ERecordType recordType, string damager, string defender, List<float> damages)
        {
            foreach (var damage in damages)
            {
                switch (recordType)
                {
                    case ERecordType.DamageToEnemy:
                        DamageMessage(damager, defender, damage);
                        break;
                    case ERecordType.SelfInflictedDamage:
                        DamageMessage(damager, damager, damage);
                        break;
                    case ERecordType.SelfHealing:
                        HealingMessage(damager, damage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(ERecordType), recordType, null);
                }
            }

            damages.Clear();
        }

        public void WriteDamageFromTo(Dictionary<ERecordType, List<float>> damages, string damager, string defender)
        {
            foreach (var damage in damages)
            {
                WriteDamageWithType(damage.Key, damager, defender, damage.Value);
            }
        }
    }
}