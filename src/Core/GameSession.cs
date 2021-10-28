using System;
using System.Linq;

namespace UniformQuoridor.Core
{
    public class GameSession
    {
        public Board Board { get; }
        
        public Player[] Players { get; }

        public Player CurrentPlayer { get; set; }
        
        public bool IsEnded { get; set; } = false;
        
        public GameSession(int boardSize, int playersCount)
        {
            Board = new Board(boardSize);
            Players = new Player[playersCount];
            
            for (int id = 1; id <= playersCount; id++)
            {
                Players[id - 1] = new Player(id, Board, PlayerType.Computer);
            }

            CurrentPlayer = PickRandomPlayer();
        }

        public void Move(int row, int column)
        {
            var availableCells = Board.AvailableCells(CurrentPlayer);
        }
        
        public void Place(int row, int column, Axis axis)
        {
            var availableFences = Board.AvailableFences();

            var foundFence = availableFences.SingleOrDefault(fence =>
                fence.CenterRow == row && fence.CenterColumn == column && fence.Axis == axis);
            
            Board.Fences.Add(foundFence);
        }

        private Player PickRandomPlayer()
        {
            var rng = new Random();
            int randomIndex = rng.Next(Players.Length);
            return Players[randomIndex];
        }
    }
}
