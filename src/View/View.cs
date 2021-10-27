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

		private int _zeroRow, _zeroColumn;

		private Board _board;
		private Player _player1, _player2;

		public GameSnapshot(Board board, Player player1, Player player2)
		{
			_zeroRow = Console.CursorTop;
			_zeroColumn = Console.CursorLeft;

			_board = board;
			_player1 = player1;
			_player2 = player2;
		}
		
		public void Print()
		{
			PrintFrame();
			PrintCells();
			PrintFences();
			PrintPlayers();

			Console.SetCursorPosition(0, _zeroRow + CellIndexToSnapshotIndex(_board.Size));
		}

		private void PrintFrame()
		{
			int firstIndex = 0, lastIndex = _board.Size * 2;
			for (int i = 0; i <= lastIndex; i ++)
			{
				WriteAt(firstIndex, i, _fenceSymbol);  // top
				WriteAt(i, lastIndex, _fenceSymbol);  // right
				WriteAt(lastIndex, i, _fenceSymbol);  // bottom
				WriteAt(i, firstIndex, _fenceSymbol);  // left
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

		private int CellIndexToSnapshotIndex(int index) => 1 + index * 2;

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
			Console.SetCursorPosition(_zeroColumn + column * 2, _zeroRow + row);
			Console.Write(symbol);
		}
	}
}
