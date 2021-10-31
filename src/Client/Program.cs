using System;
using UniformQuoridor.Core;
using UniformQuoridor.Controller;
using UniformQuoridor.View;

namespace UniformQuoridor.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var game = new GameSession(9, 2);
            // while (!gameSession.IsEnded)
            // {
            //
            // }
            
            // var view = new GameView(game.Board, game.Players[0], game.Players[1]);
            // view.Print();
            // string rawInput = view.AskInput();
            
            // var controller = new GameController(game);
            // controller.AcceptRequest(rawInput);
            
            var controller = new GameController(game);
            controller.AcceptRequest("move E2");

            var view = new GameSnapshot(game.Board, game.Players[0], game.Players[1]);
            view.Print();
            
            Console.ReadKey();
        }
    }
}
