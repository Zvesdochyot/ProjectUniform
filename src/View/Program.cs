using System;
using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // var game = new GameSession(..);
            // var controller = new Controller(game);
            // var view = new View(game);
            
            // var game = new GameSession(controller, view);
            
            // var game = new GameSession(..);
            // var view = new View();
            // var controller = new Controller(game, view);

            var coreGame = new GameSession(9, 2);
            
            // while (!gameSession.IsEnded)
            // {
            //
            // }
            
            // var test = new GameSnapshot(testBoard, testPlayers);
            // test.Print();

            Console.ReadKey();
        }
    }
}
