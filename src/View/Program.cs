using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var gameSession = new GameSession(9, 2);
            
            while (!gameSession.IsEnded)
            {

            }

            
            // var test = new GameSnapshot(testBoard, testPlayers);
            // test.Print();

            System.Console.ReadKey();
        }
    }
}
