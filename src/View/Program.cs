using System;
using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var gameSession = new GameSession(9, 2);
            
            // while (!gameSession.IsEnded)
            // {
            //
            // }
            
            Console.WriteLine("Operation 0 / 3..");
            var result = Board.PathExists(gameSession.Players[0].Cell, gameSession.Players[0].TargetCells[0]);

            Console.WriteLine(result);
            Console.WriteLine("Operation 1 / 3..");
            gameSession.Place(1, 1, Axis.Horizontal);
            Console.WriteLine("Operation 2 / 3..");
            gameSession.Place(1, 1, Axis.Vertical);
            Console.WriteLine("Operation 3 / 3..");
            gameSession.Place(1, 1, Axis.Horizontal);
            
            // var test = new GameSnapshot(testBoard, testPlayers);
            // test.Print();

            Console.ReadKey();
        }
    }
}
