using System;
using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
	public class GameSnapshot
	{
		private const string FenceSymbol = "██";
		private const string CellSymbol = "[]";
		
		private readonly string[] _playerSymbols = { "▲▲", "▼▼" };
		
		private readonly Board _board;
		private readonly Player[] _players;
		private readonly ViewParameters _viewParameters;

		public GameSnapshot(Board board, Player[] players, ViewParameters viewParameters)
		{
			_board = board;
			_players = players;
			_viewParameters = viewParameters;
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
				WriteAt(ViewParameters.FirstIndex, CellIndexToSnapshotIndex(i), symbol);  // top
				symbol = $"{1 + i} ";
				WriteAt(CellIndexToSnapshotIndex(i), ViewParameters.FirstIndex, symbol);  // left
			}

			// fence centers
			for (int i = 0; i < _board.Size - 1; i++)
			{
				symbol = $" {1 + i}";
				WriteAt(CellIndexToSnapshotIndex(i) + 1, _viewParameters.LastIndex, symbol);  // right
				symbol = $"{(char) (90 - _board.Size + 2 + i)} ";  // 90 = Z
				WriteAt(_viewParameters.LastIndex, CellIndexToSnapshotIndex(i) + 1, symbol);  // bottom
			}
		}

		private void PrintFrame()
		{
			int frameFirstIndex = ViewParameters.CellAreaFirstIndex - 1;
			int frameLastIndex = _viewParameters.CellAreaLastIndex + 1;
			for (int i = frameFirstIndex; i <= frameLastIndex; i++)
			{
				WriteAt(frameFirstIndex, i, FenceSymbol);  // top
				WriteAt(i, frameLastIndex, FenceSymbol);   // right
				WriteAt(frameLastIndex, i, FenceSymbol);   // bottom
				WriteAt(i, frameFirstIndex, FenceSymbol);  // left
			}
		}

		private void PrintCells()
		{
			for (int r = 0; r < _board.Size; r++)
			{
				for (int c = 0; c < _board.Size; c++)
				{
					WriteAt(CellIndexToSnapshotIndex(r), CellIndexToSnapshotIndex(c), CellSymbol);
				}
			}
		}

		private static int CellIndexToSnapshotIndex(int index) => ViewParameters.CellAreaFirstIndex + index * 2;

		private void PrintFences()
		{
			foreach (var fence in _board.Fences)
			{
				int centerRow = FenceIndexToSnapshotIndex(fence.CenterRow);
				int centerColumn = FenceIndexToSnapshotIndex(fence.CenterColumn);

				WriteAt(centerRow, centerColumn, FenceSymbol);

				if (fence.Axis == Axis.Horizontal)
				{
					WriteAt(centerRow, centerColumn - 1, FenceSymbol);
					WriteAt(centerRow, centerColumn + 1, FenceSymbol);
				}
				else
				{
					WriteAt(centerRow - 1, centerColumn, FenceSymbol);
					WriteAt(centerRow + 1, centerColumn, FenceSymbol);
				}
			}
		}

		private static int FenceIndexToSnapshotIndex(int index) => CellIndexToSnapshotIndex(index) + 1;

		private void PrintPlayers()
		{
			for (int i = 0; i < _players.Length; i++)
			{
				WriteAt(CellIndexToSnapshotIndex(_players[i].Cell.Row), 
					CellIndexToSnapshotIndex(_players[i].Cell.Column), 
					_playerSymbols[i]);
			}
		}

		private static void WriteAt(int row, int column, string symbol)
		{
			Console.SetCursorPosition(column * 2, row);
			Console.Write(symbol);
		}
	}
}
