using System;
using System.Collections.Generic;

namespace UniformQuoridor.Core
{
	public class Board
	{
		public int Size { get; init; }

		public Cell[,] Cells { get; init; }
		private List<Fence> _fences;

		public Board(int size)
		{
			Size = size;
			Cells = new Cell[Size, Size];

			InitCells();
		}

		private void InitCells()
		{
			for (int y = 0; y < Size; y++)
			{
				for (int x = 0; x < Size; x++)
				{
					Cells[x, y] = new Cell(x, y);
				}
			}

			InitAdjacentCells();
		}

		private void InitAdjacentCells()
		{
			int firstIndex = 0, lastIndex = Size - 1;


			// corner cells

			// top-left
			Cells[firstIndex, firstIndex].Right = Cells[firstIndex + 1, firstIndex];
			Cells[firstIndex, firstIndex].Bottom = Cells[firstIndex, firstIndex + 1];

			// top-right
			Cells[lastIndex, firstIndex].Bottom = Cells[lastIndex, firstIndex + 1];
			Cells[lastIndex, firstIndex].Left = Cells[lastIndex - 1, firstIndex];
			
			// bottom-right
			Cells[lastIndex, lastIndex].Left = Cells[lastIndex - 1, lastIndex];
			Cells[lastIndex, lastIndex].Top = Cells[lastIndex, lastIndex - 1];

			// bottom-left
			Cells[firstIndex, lastIndex].Top = Cells[firstIndex, lastIndex - 1];
			Cells[firstIndex, lastIndex].Right = Cells[firstIndex + 1, lastIndex];


			// edge cells

			for (int i = firstIndex + 1; i <= lastIndex - 1; i++)
			{
				// top
				Cells[i, firstIndex].Right = Cells[i + 1, firstIndex];
				Cells[i, firstIndex].Bottom = Cells[i, firstIndex + 1];
				Cells[i, firstIndex].Left = Cells[i - 1, firstIndex];

				// right
				Cells[lastIndex, i].Top = Cells[lastIndex, i - 1];
				Cells[lastIndex, i].Bottom = Cells[lastIndex, i + 1];
				Cells[lastIndex, i].Left = Cells[lastIndex - 1, i];

				// bottom
				Cells[i, lastIndex].Left = Cells[i - 1, lastIndex];
				Cells[i, lastIndex].Top = Cells[i, lastIndex - 1];
				Cells[i, lastIndex].Right = Cells[i + 1, lastIndex];

				// left
				Cells[firstIndex, i].Top = Cells[firstIndex, i - 1];
				Cells[firstIndex, i].Right = Cells[firstIndex + 1, i];
				Cells[firstIndex, i].Bottom = Cells[firstIndex, i + 1];
			}


			// inner cells

			for (int y = firstIndex + 1; y <= lastIndex - 1; y++)
			{
				for (int x = firstIndex + 1; x <= lastIndex - 1; x++)
				{
					Cells[x, y].Top = Cells[x, y - 1];
					Cells[x, y].Right = Cells[x + 1, y];
					Cells[x, y].Bottom = Cells[x, y + 1];
					Cells[x, y].Left = Cells[x - 1, y];
				}
			}
		}

		private List<Cell> AvailableCells(Player player)
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

		private List<Fence> AvailableFences()
		{
			var available = new List<Fence>();
			var unencounteredFences = _fences;

			int encounteredFenceIndex;
			for (int y = 0; y <=  Size - 1 - 1; y++)
			{
				for (int x = 0; x <=  Size - 1 - 1; x++)
				{
					encounteredFenceIndex = unencounteredFences.FindIndex(
						fence => fence.CenterX == x && fence.CenterY == y
					);
					if (encounteredFenceIndex != -1)
					{
						unencounteredFences.RemoveAt(encounteredFenceIndex);
						continue;
					}

					if (Cells[x, y].Bottom != null && Cells[x + 1, y].Bottom != null)
					{
						available.Add(new Fence(x, y, Axis.Horizontal));
					}

					if (Cells[x, y].Right != null && Cells[x, y + 1].Right != null)
					{
						available.Add(new Fence(x, y, Axis.Vertical));
					}
				}
			}

			return available;
		}

		private bool pathExists(Cell a, Cell b)
		{
			// depth-first, visiting the cell which is the closest to the end

			var candidates = new Dictionary<Cell, double>() { {a, distance(a, b)} };
			Cell current;
			var visited = new List<Cell>();

			while (candidates.Count != 0)
			{
				current = closest(candidates, b);

				if (current == b) return true;
				visited.Add(current);

				if (!visited.Contains(current.Top) && current.Top != null)
				{
					candidates.Add(current.Top, distance(current.Top, b));
				}
				if (!visited.Contains(current.Right) && current.Right != null)
				{
					candidates.Add(current.Right, distance(current.Right, b));
				}
				if (!visited.Contains(current.Bottom) && current.Bottom != null)
				{
					candidates.Add(current.Bottom, distance(current.Bottom, b));
				}
				if (!visited.Contains(current.Top) && current.Left != null)
				{
					candidates.Add(current.Left, distance(current.Left, b));
				}
			}

			return false;
		}

		private double distance(Cell a, Cell b)
		{
			int distanceX = a.X - b.X, distanceY = a.Y - b.Y;
			return Math.Sqrt(distanceX*distanceX + distanceY*distanceY);
		}

		private Cell closest(Dictionary<Cell, double> distances, Cell target)
		{
			Cell closest = default;
			double smallestDistance = -1;

			foreach (var cellDistance in distances)
			{
				if (cellDistance.Value < smallestDistance)
				{
					closest = cellDistance.Key;
					smallestDistance = cellDistance.Value;
				}
			}

			return closest;
		}
	}
}
