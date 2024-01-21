using Game.Class;

namespace Game;

internal class Program
{
    public static void Main(string[] args)
    {
        var player = new Player();
        var enemy = new Enemy();
        var game = new Class.Game();
        
        game.GameLoop(player,enemy);
    }
}