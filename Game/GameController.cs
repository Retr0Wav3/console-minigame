using Game.Enums;
using Game.ConsoleOutput;
using Game.Systems;
using Game.Units;

namespace Game.Class
{
    public class GameController
    {
        public Input Input { get; }
        public Logger Logger { get; }
        
        private Player _player;
        private Enemy _enemy;
        private int _playerCommand;
        private int _enemyCommand;

        public GameController(Player player, Enemy enemy)
        {
            _player = player;
            _enemy = enemy;
            Input = new Input(player);
            Logger = new Logger();
        }


        public void Start()
        {
            Input.GetPlayerName(_player);
            Logger.WelcomeMessage();

            while (_player.Health.CurrentHealth > 0 && _enemy.Health.CurrentHealth > 0)
            {
                Logger.ShowPlayersStatus(_player, _enemy);
                PlayerTurn(_player, _enemy);
                EnemyTurn(_player, _enemy);
            }
        }


        private void PlayerTurn(Player player, Enemy enemy)
        {
            Logger.ShowAvaliableActions(player);
            _playerCommand = Input.GetPlayerCommandNumber();

            switch (_playerCommand)
            {
                case 1:
                    player.Attack(enemy, _player.WeaponDamage, EAttackType.Weapon);
                    break;

                case 2:
                    player.BlockAttack();
                    break;

                case 3:
                    player.Attack(enemy, _player.AbilityDamage, EAttackType.Ability);
                    break;

                case 4:
                    player.Heal(player.Health.HealAmount, enemy.LastAction);
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
            _enemyCommand = Input.GetAICommandNumber(enemy.CommandCount);

            switch (_enemyCommand)
            {
                case 1:
                    enemy.Attack(player, enemy.WeaponDamage, EAttackType.Weapon);
                    break;

                case 2:
                    enemy.Attack(player, enemy.WeaponDamage, EAttackType.Ability);
                    break;

                case 3:
                    enemy.Heal(enemy.Health.HealAmount, player.LastAction);
                    break;

                default:
                    Logger.UndefinedCommand();
                    break;
            }

            Logger.WriteDamageFromTo(_player.DamageHistory, player.Name, enemy.Name);
            Logger.WriteDamageFromTo(_enemy.DamageHistory, enemy.Name, player.Name);
            Logger.EndTurnMessage();
        }
    }
}