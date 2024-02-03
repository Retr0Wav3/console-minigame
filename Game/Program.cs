using Game.Class;
using Game.Units;


namespace EntryPoint
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var player = new Player(1000f, 45f, 200f, 100f);
            var enemy = new Enemy(2000f, 50f, 100f);
            var game = new GameController(player, enemy);
        
            game.Start();
        }
    }
}