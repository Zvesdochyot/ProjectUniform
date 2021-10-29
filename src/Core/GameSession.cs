using System;
using System.Linq;
using UniformQuoridor.Core.Exceptions;

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
            var available = Board.AvailableFences();

            var found = available.SingleOrDefault(fence =>
                fence.CenterRow == row && fence.CenterColumn == column && fence.Axis == axis);
            
            if (found == null)
            {
                throw new FenceUnplaceableException(
                    "A fence you are trying to place has already been placed on this cell.");
            }

            Board.AddFence(found);
            var pathExistsResult = new bool[Players.Length];
            foreach (var player in Players)
            {
                pathExistsResult[player.Id - 1] = player.TargetCells.Any(cell => Board.PathExists(player.Cell, cell));
            }

            if (!pathExistsResult.All(value => value))
            {
                Board.RemoveFence(found);
                throw new FenceUnplaceableException(
                    "A fence you are trying to place blocks all possible paths for one of the players.");
            }

            CurrentPlayer = GetNextPlayer();
        }

        private Player PickRandomPlayer()
        {
            var rng = new Random();
            int randomIndex = rng.Next(Players.Length);
            return Players[randomIndex];
        }

        private Player GetNextPlayer()
        {
            return Players[CurrentPlayer.Id % Players.Length];
        }
    }
}
