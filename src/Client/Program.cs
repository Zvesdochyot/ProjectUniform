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
            var controller = new GameController(game);
            var view = new GameView(game);
            
            view.AskForPrint();
            
            while (!game.IsEnded)
            {
                try
                {
                    string rawInput = view.AskForInput();
                    controller.AcceptRequest(rawInput);
                    view.AskForPrint();

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            
            Console.Clear();
            Console.WriteLine($"Player {game.CurrentPlayer.Id} won. Congrats!");
            
            Console.ReadKey();
        }
    }
}
