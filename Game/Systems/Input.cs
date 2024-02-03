using Game.ConsoleOutput;
using Game.Units;

namespace Game.Systems;

public class Input
{
    private Player _player;
    private Logger _logger;
    private Random _random;

    public Input(Player player)
    {
        _player = player;
        _logger = new();
        _random = new();
    }
    
    public void GetPlayerName(BaseUnit unit)
    {
        _logger.RequestPlayerName();
        string? input = Console.ReadLine();

        while (string.IsNullOrEmpty(input))
        {
            _logger.InvalidUsername();
            input = Console.ReadLine();
        }
        
        _player.Name = input;
    }

    public int GetPlayerCommandNumber()
    {
        string input = Console.ReadLine();
        int commandNumber;

        while (!int.TryParse(input, out commandNumber))
        {
            _logger.UndefinedCommand();
            input = Console.ReadLine();
        }

        return commandNumber;
    }

    public int GetAICommandNumber(int amountOfCommands)
    {
        return _random.Next(1, amountOfCommands + 1);
    }
}