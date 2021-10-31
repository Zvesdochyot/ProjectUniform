using System;
using UniformQuoridor.Core;
using UniformQuoridor.Controller;
using UniformQuoridor.Core.Exceptions;

namespace UniformQuoridor.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var game = new GameSession(9, 2);
            var controller = new GameController(game);

            try
            {
                controller.AcceptRequest("move E2");
                controller.AcceptRequest("move E8");
                controller.AcceptRequest("move A1");
            }
            catch (UnreachableCellException exception)
            {
                Console.WriteLine(exception.Message);
            }
            
            try
            {
                controller.AcceptRequest("place S1h");
                controller.AcceptRequest("place S1v");
            }
            catch (UnplaceableFenceException exception)
            {
                Console.WriteLine(exception.Message);
            }
            
            // while (!gameSession.IsEnded)
            // {
            //
            // }

            Console.ReadKey();
        }
    }
}
