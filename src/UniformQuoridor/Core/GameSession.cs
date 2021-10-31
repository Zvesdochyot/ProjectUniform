using System;
using System.Linq;
using UniformQuoridor.Core.Exceptions;

namespace UniformQuoridor.Core
{
    public class GameSession
    {
        public Board Board { get; }
        
        public Player[] Players { get; }
        
        public Player CurrentPlayer { get; private set; }
        
        public bool IsEnded { get; set; }
        
        public GameSession(int boardSize, int playersCount)
        {
            Board = new Board(boardSize);
            Players = new Player[playersCount];

            for (int id = 1; id <= playersCount; id++)
            {
                Players[id - 1] = new Player(id, Board);
            }

            ChooseFirstPlayer();
        }

        public void Move(int row, int column)
        {
            var challenger = Board.Cells[row, column];
            var available = Board.AvailableCells(CurrentPlayer);
            
            if (!available.Contains(challenger))
            {
                throw new UnreachableCellException(
                    "A cell you are trying to move to is unreachable.");
            }

            CurrentPlayer.Cell = challenger;

            if (CurrentPlayer.TargetCells.Contains(CurrentPlayer.Cell))
            {
                IsEnded = true;
            }
            else
            {
                PassTurn();
            }
        }
        
        public void Place(int row, int column, Axis axis)
        {
            var challenger = new Fence(row, column, axis);
            
            if (!Board.FenceIsAvailable(challenger))
            {
                throw new UnplaceableFenceException(
                    "A fence you are trying to place has already been placed on this cell.");
            }
            
            Board.AddFence(challenger);
            
            var pathExistsResult = new bool[Players.Length];
            foreach (var player in Players)
            {
                pathExistsResult[player.Id - 1] = player.TargetCells.Any(cell => Board.PathExists(player.Cell, cell));
            }

            if (!pathExistsResult.All(value => value))
            {
                Board.RemoveFence(challenger);
                throw new UnplaceableFenceException(
                    "A fence you are trying to place blocks all possible paths for one of the players.");
            }
            
            CurrentPlayer.RemainingFences -= 1;
            PassTurn();
        }
        
        private void ChooseFirstPlayer()
        {
            var rng = new Random();
            int randomIndex = rng.Next(Players.Length);
            CurrentPlayer = Players[randomIndex];
        }

        private void PassTurn()
        {
            CurrentPlayer = Players[CurrentPlayer.Id % Players.Length];
        }
    }
}
