using System;
using System.Collections.Generic;


namespace UniformQuoridor.Core
{
	public class Board
	{
		public int Size { get; init; }

		public Cell[,] Cells { get; init; }
		public List<Fence> Fences { get; } = new List<Fence>();

		public Board(int size)
		{
			Size = size;
			Cells = new Cell[Size, Size];

			InitCells();
		}

		private void InitCells()
		{
			for (int r = 0; r < Size; r++)
			{
				for (int c = 0; c < Size; c++)
				{
					Cells[r, c] = new Cell(r, c);
				}
			}

			InitAdjacentCells();
		}

		private void InitAdjacentCells()
		{
			int firstIndex = 0, lastIndex = Size - 1;


			// corner cells

			// top-left
			Cells[firstIndex, firstIndex].Right = Cells[firstIndex, firstIndex + 1];
			Cells[firstIndex, firstIndex].Bottom = Cells[firstIndex + 1, firstIndex];

			// top-right
			Cells[firstIndex, lastIndex].Bottom = Cells[firstIndex + 1, lastIndex];
			Cells[firstIndex, lastIndex].Left = Cells[firstIndex, lastIndex - 1];
			
			// bottom-right
			Cells[lastIndex, lastIndex].Left = Cells[lastIndex, lastIndex - 1];
			Cells[lastIndex, lastIndex].Top = Cells[lastIndex - 1, lastIndex];

			// bottom-left
			Cells[lastIndex, firstIndex].Top = Cells[lastIndex - 1, firstIndex];
			Cells[lastIndex, firstIndex].Right = Cells[lastIndex, firstIndex + 1];


			// edge and inner cells

			for (int i = firstIndex + 1; i <= lastIndex - 1; i++)
			{
				// top edge
				Cells[firstIndex, i].Right = Cells[firstIndex, i + 1];
				Cells[firstIndex, i].Bottom = Cells[firstIndex + 1, i];
				Cells[firstIndex, i].Left = Cells[firstIndex, i - 1];

				// right edge
				Cells[i, lastIndex].Top = Cells[i - 1, lastIndex];
				Cells[i, lastIndex].Bottom = Cells[i + 1, lastIndex];
				Cells[i, lastIndex].Left = Cells[i, lastIndex - 1];

				// bottom edge
				Cells[lastIndex, i].Left = Cells[lastIndex, i - 1];
				Cells[lastIndex, i].Top = Cells[lastIndex - 1, i];
				Cells[lastIndex, i].Right = Cells[lastIndex, i + 1];

				// left edge
				Cells[i, firstIndex].Top = Cells[i - 1, firstIndex];
				Cells[i, firstIndex].Right = Cells[i, firstIndex + 1];
				Cells[i, firstIndex].Bottom = Cells[i + 1, firstIndex];

				// inner cells
				for (int c = firstIndex + 1; c <= lastIndex - 1; c++)
				{
					Cells[i, c].Top = Cells[i  - 1, c];
					Cells[i, c].Right = Cells[i, c + 1];
					Cells[i, c].Bottom = Cells[i + 1, c];
					Cells[i, c].Left = Cells[i, c - 1];
				}
			}
		}

		public List<Cell> AvailableCells(Player player)
		{
			var available = new List<Cell>(5);

			available.AddRange(AvailableToTop(player));
			available.AddRange(AvailableToRight(player));
			available.AddRange(AvailableToBottom(player));
			available.AddRange(AvailableToLeft(player));

			return available;
		}

		private List<Cell> AvailableToTop(Player player)
		{
			var cell = player.Cell;
			var available = new List<Cell>(2);

			if (cell.Top == null) return available;

			if (cell.Top.IsFree) available.Add(cell.Top);
			else
			{
				if (cell.Top.Top != null) available.Add(cell.Top.Top);
				else
				{
					if (cell.Top.Left != null) available.Add(cell.Top.Left);
					if (cell.Top.Right != null) available.Add(cell.Top.Right);
				}
			}

			return available;
		}

		private List<Cell> AvailableToRight(Player player)
		{
			var cell = player.Cell;
			var available = new List<Cell>(2);

			if (cell.Right == null) return available;

			if (cell.Right.IsFree) available.Add(cell.Right);
			else
			{
				if (cell.Right.Right != null) available.Add(cell.Right.Right);
				else
				{
					if (cell.Right.Top != null) available.Add(cell.Right.Top);
					if (cell.Right.Bottom != null) available.Add(cell.Right.Bottom);
				}
			}

			return available;
		}

		private List<Cell> AvailableToBottom(Player player)
		{
			var cell = player.Cell;
			var available = new List<Cell>(2);

			if (cell.Bottom == null) return available;

			if (cell.Bottom.IsFree) available.Add(cell.Bottom);
			else
			{
				if (cell.Bottom.Bottom != null) available.Add(cell.Bottom.Bottom);
				else
				{
					if (cell.Bottom.Right != null) available.Add(cell.Bottom.Right);
					if (cell.Bottom.Left != null) available.Add(cell.Bottom.Left);
				}
			}

			return available;
		}
		
		private List<Cell> AvailableToLeft(Player player)
		{
			var cell = player.Cell;
			var available = new List<Cell>(2);

			if (cell.Left == null) return available;

			if (cell.Left.IsFree) available.Add(cell.Left);
			else
			{
				if (cell.Left.Left != null) available.Add(cell.Left.Left);
				else
				{
					if (cell.Left.Bottom != null) available.Add(cell.Left.Bottom);
					if (cell.Left.Top != null) available.Add(cell.Left.Top);
				}
			}

			return available;
		}

		public List<Fence> AvailableFences()
		{
			var available = new List<Fence>();
			var unencountered = new List<Fence>(Fences);

			int lastIndex = Size - 1, encounteredFenceIndex;
			for (int r = 0; r <=  lastIndex - 1; r++)
			{
				for (int c = 0; c <=  lastIndex - 1; c++)
				{
					encounteredFenceIndex = unencountered.FindIndex(
						(fence) => fence.CenterRow == r && fence.CenterColumn == c
					);
					if (encounteredFenceIndex != -1)  // if the fence exists
					{
						unencountered.RemoveAt(encounteredFenceIndex);
						continue;  // it is unavailable
					}

					var cellToTopLeft = Cells[r, c];

					if (cellToTopLeft.Bottom != null && cellToTopLeft.Right.Bottom != null)
					{
						available.Add(new Fence(r, c, Axis.Horizontal));
					}

					if (cellToTopLeft.Right != null && cellToTopLeft.Bottom.Right != null)
					{
						available.Add(new Fence(r, c, Axis.Vertical));
					}
				}
			}

			return available;
		}

		private bool PathExists(Cell a, Cell b)
		{
			// depth-first, visiting the cell which is the closest one to the end cell

			var candidates = new Dictionary<Cell, double>() { {a, Distance(a, b)} };
			Cell current;
			var visited = new List<Cell>();

			while (candidates.Count != 0)
			{
				current = Closest(candidates);

				if (current == b) return true;
				visited.Add(current);

				if (!visited.Contains(current.Top) && current.Top != null)
				{
					candidates.Add(current.Top, Distance(current.Top, b));
				}
				if (!visited.Contains(current.Right) && current.Right != null)
				{
					candidates.Add(current.Right, Distance(current.Right, b));
				}
				if (!visited.Contains(current.Bottom) && current.Bottom != null)
				{
					candidates.Add(current.Bottom, Distance(current.Bottom, b));
				}
				if (!visited.Contains(current.Top) && current.Left != null)
				{
					candidates.Add(current.Left, Distance(current.Left, b));
				}
			}

			return false;
		}

		private static double Distance(Cell a, Cell b)
		{
			int distanceX = a.Column - b.Column, distanceY = a.Row - b.Row;
			return Math.Sqrt(distanceX*distanceX + distanceY*distanceY);
		}

		private static Cell Closest(Dictionary<Cell, double> distances)
		{
			Cell closest = default;
			double smallestDistance = Double.PositiveInfinity;

			foreach (var (cell, distance) in distances)
			{
				if (distance < smallestDistance)
				{
					closest = cell;
					smallestDistance = distance;
				}
			}

			return closest;
		}
	}
}
