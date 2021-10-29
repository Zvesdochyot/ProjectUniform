using System;

using UniformQuoridor.Core;


namespace UniformQuoridor.View
{
	class GameSnapshot
	{
		private const string _fenceSymbol = "██";
		private const string _cellSymbol = "[]";
		private const string _player1Symbol = "▲▲";
		private const string _player2Symbol = "▼▼";

		private int cellAreaSize;
		private int firstIndex = 0, lastIndex;
		private int cellAreaFirstIndex = 2, cellAreaLastIndex;

		private Board _board;
		private Player _player1, _player2;

		public GameSnapshot(Board board, Player player1, Player player2)
		{
			_board = board;
			_player1 = player1;
			_player2 = player2;

			cellAreaSize = board.Size * 2 - 1;
			cellAreaLastIndex = cellAreaFirstIndex + cellAreaSize - 1;
			lastIndex = cellAreaLastIndex + 2;
		}
		
		public void Print()
		{
			PrintNotation();
			PrintFrame();
			PrintCells();
			PrintFences();
			PrintPlayers();
		}

		private void PrintNotation()
		{
			string symbol;

			// cells
			for (int i = 0; i < _board.Size; i++)
			{
				symbol = $"{(char) (65 + i)} ";  // 65 = A
				WriteAt(firstIndex, CellIndexToSnapshotIndex(i), symbol);  // top
				symbol = $"{1 + i} ";
				WriteAt(CellIndexToSnapshotIndex(i), firstIndex, symbol);  // left
			}

			// fence centers
			for (int i = 0; i < _board.Size - 1; i++)
			{
				symbol = $" {1 + i}";
				WriteAt(CellIndexToSnapshotIndex(i) + 1, lastIndex, symbol);  // right
				symbol = $"{(char) (90 - _board.Size + 2 + i)} ";  // 90 = Z
				WriteAt(lastIndex, CellIndexToSnapshotIndex(i) + 1, symbol);  // bottom
			}
		}

		private void PrintFrame()
		{
			int frameFirstIndex = cellAreaFirstIndex - 1, frameLastIndex = cellAreaLastIndex + 1;
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
			for (int r = 0; r < _board.Size; r++)
			{
				for (int c = 0; c < _board.Size; c++)
				{
					WriteAt(CellIndexToSnapshotIndex(r), CellIndexToSnapshotIndex(c), _cellSymbol);
				}
			}
		}

		private int CellIndexToSnapshotIndex(int index) => cellAreaFirstIndex + index * 2;

		private void PrintFences()
		{
			foreach (var fence in _board.Fences)
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
		}

		private int FenceIndexToSnapshotIndex(int index) => CellIndexToSnapshotIndex(index) + 1;

		private void PrintPlayers()
		{
			WriteAt(CellIndexToSnapshotIndex(_player1.Cell.Row),
				CellIndexToSnapshotIndex(_player1.Cell.Column), _player1Symbol);
			WriteAt(CellIndexToSnapshotIndex(_player2.Cell.Row),
				CellIndexToSnapshotIndex(_player1.Cell.Column), _player2Symbol);
		}

		private void WriteAt(int row, int column, string symbol)
		{
			Console.SetCursorPosition(column * 2, row);
			Console.Write(symbol);
		}
	}
}
