using System;
using System.Collections.Generic;

using UniformQuoridor.Core;


namespace UniformQuoridor.View
{
    class GameSnapshot
    {
        private const string _fenceSymbol = "██";
        private const string _cellSymbol = "[]";
        private const string _player1Symbol = "▲▲";
        private const string _player2Symbol = "▼▼";

        private int _cellAreaSize;
        private int _firstIndex = 0, _lastIndex;
        private int _cellAreaFirstIndex = 2, _cellAreaLastIndex;

        private int _boardSize;
        private Cell _player1Cell, _player2Cell;

        public GameSnapshot(Board board, Player player1, Player player2)
        {
            _boardSize = board.Size;
            _player1Cell = player1.Cell;
            _player2Cell = player2.Cell;

            _cellAreaSize = board.Size * 2 - 1;
            _cellAreaLastIndex = _cellAreaFirstIndex + _cellAreaSize - 1;
            _lastIndex = _cellAreaLastIndex + 2;
        }

        public void Print(Board board)
        {
            PrintNotation();
            PrintFrame();
            PrintCells();
            PrintFences(board.Fences);
            PrintPlayers();
        }

        private void PrintNotation()
        {
            string symbol;

            // cells
            for (int i = 0; i < _boardSize; i++)
            {
                symbol = $"{(char) (65 + i)} ";  // 65 = A
                WriteAt(_firstIndex, CellIndexToSnapshotIndex(i), symbol);  // top
                symbol = $"{1 + i} ";
                WriteAt(CellIndexToSnapshotIndex(i), _firstIndex, symbol);  // left
            }

            // fence centers
            for (int i = 0; i < _boardSize - 1; i++)
            {
                symbol = $" {1 + i}";
                WriteAt(CellIndexToSnapshotIndex(i) + 1, _lastIndex, symbol);  // right
                symbol = $"{(char) (90 - _boardSize + 2 + i)} ";  // 90 = Z
                WriteAt(_lastIndex, CellIndexToSnapshotIndex(i) + 1, symbol);  // bottom
            }
        }

        private void PrintFrame()
        {
            int frameFirstIndex = _cellAreaFirstIndex - 1, frameLastIndex = _cellAreaLastIndex + 1;
            for (int i = frameFirstIndex; i <= frameLastIndex; i++)
            {
                WriteAt(frameFirstIndex, i, _fenceSymbol);  // top
                WriteAt(i, frameLastIndex, _fenceSymbol);  // right
                WriteAt(frameLastIndex, i, _fenceSymbol);  // bottom
                WriteAt(i, frameFirstIndex, _fenceSymbol);  // left
            }
        }

        private void PrintCells()
        {
            for (int r = 0; r < _boardSize; r++)
            {
                for (int c = 0; c < _boardSize; c++)
                {
                    WriteAt(CellIndexToSnapshotIndex(r), CellIndexToSnapshotIndex(c), _cellSymbol);
                }
            }
        }

        private int CellIndexToSnapshotIndex(int index) => _cellAreaFirstIndex + index * 2;

        private void PrintFences(List<Fence> fences)
        {
            foreach (var fence in fences) PrintFence(fence);
        }

        private void PrintFence(Fence fence)
        {
            int centerRow = FenceIndexToSnapshotIndex(fence.CenterRow);
            int centerColumn = FenceIndexToSnapshotIndex(fence.CenterColumn);

            WriteAt(centerRow, centerColumn, _fenceSymbol);

            if (fence.Axis == Axis.Horizontal)
            {
                WriteAt(centerRow, centerColumn - 1, _fenceSymbol);
                WriteAt(centerRow, centerColumn + 1, _fenceSymbol);
            }
            else
            {
                WriteAt(centerRow - 1, centerColumn, _fenceSymbol);
                WriteAt(centerRow + 1, centerColumn, _fenceSymbol);
            }
        }

        private int FenceIndexToSnapshotIndex(int index) => CellIndexToSnapshotIndex(index) + 1;

        private void PrintPlayers()
        {
            WriteAt(CellIndexToSnapshotIndex(_player1Cell.Row),
                CellIndexToSnapshotIndex(_player1Cell.Column), _player1Symbol);
            WriteAt(CellIndexToSnapshotIndex(_player2Cell.Row),
                CellIndexToSnapshotIndex(_player1Cell.Column), _player2Symbol);
        }

        public void MovePlayer(Player player)
        {
            Cell freedCell;
            string playerSymbol;

            if (player.Id == 1)
            {
                freedCell = _player1Cell;
                playerSymbol = _player1Symbol;
                _player1Cell = player.Cell;
            }
            else
            {
                freedCell = _player2Cell;
                playerSymbol = _player2Symbol;
                _player2Cell = player.Cell;
            }

            WriteAt(CellIndexToSnapshotIndex(freedCell.Row), CellIndexToSnapshotIndex(freedCell.Column), _cellSymbol);
            WriteAt(CellIndexToSnapshotIndex(player.Cell.Row), CellIndexToSnapshotIndex(player.Cell.Column), playerSymbol);
        }

        public void AddFence(Fence fence)
        {
            PrintFence(fence);
        }

        private static void WriteAt(int row, int column, string symbol)
        {
            Console.SetCursorPosition(column * 2, row);
            Console.Write(symbol);
        }
    }
}
