using System;
using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
    public class GameView
    {
        private readonly GameSession _game;
        
        public GameView(GameSession game)
        {
            _game = game;
        }

        public void AskForPrint()
        {
            var snapshot = new GameSnapshot(_game.Board, _game.Players[0], _game.Players[1]);
            snapshot.Print();
        }

        public string AskForInput()
        {
            var available = Board.AvailableCells(_game.CurrentPlayer);
            Console.WriteLine("Available cells: ");
            available.ForEach(cell => Console.WriteLine($"x: {cell.Row} --- y: {cell.Column}"));
            Console.WriteLine();
            Console.Write($"Player {_game.CurrentPlayer.Id}, please, enter your next move: ");
            return Console.ReadLine();
        }
    }
}