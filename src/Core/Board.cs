using System;
using System.Collections.Generic;

namespace UniformQuoridor.Core
{
	public class Board
	{
		private int _size;

		private Cell[,] _cells;
		private Axis[,] _fenceAxes;

		public Board(int size)
		{
			_size = size;
			InitCells();
		}

		private void InitCells()
		{
			_cells = new Cell[_size, _size];

			for (int x = 0; x < _size; x++)
			{
				for (int y = 0; y < _size; y++)
				{
					_cells[x, y] = new Cell(x, y);
				}
			}

			InitAdjacentCells();
		}

		private void InitAdjacentCells()
		{
			int firstIndex = 0, lastIndex = _size - 1;


			// corner cells

			// top-left
			_cells[firstIndex, firstIndex].Right = _cells[firstIndex + 1, firstIndex];
			_cells[firstIndex, firstIndex].Bottom = _cells[firstIndex, firstIndex + 1];

			// top-right
			_cells[lastIndex, firstIndex].Bottom = _cells[lastIndex, firstIndex + 1];
			_cells[lastIndex, firstIndex].Left = _cells[lastIndex - 1, firstIndex];
			
			// bottom-right
			_cells[lastIndex, lastIndex].Left = _cells[lastIndex - 1, lastIndex];
			_cells[lastIndex, lastIndex].Top = _cells[lastIndex, lastIndex - 1];

			// bottom-left
			_cells[firstIndex, lastIndex].Top = _cells[firstIndex, lastIndex - 1];
			_cells[firstIndex, lastIndex].Right = _cells[firstIndex + 1, lastIndex];


			// edge cells

			for (int i = firstIndex + 1; i <= lastIndex - 1; i++)
			{
				// top
				_cells[i, firstIndex].Right = _cells[i + 1, firstIndex];
				_cells[i, firstIndex].Bottom = _cells[i, firstIndex + 1];
				_cells[i, firstIndex].Left = _cells[i - 1, firstIndex];

				// right
				_cells[lastIndex, i].Top = _cells[lastIndex, i - 1];
				_cells[lastIndex, i].Bottom = _cells[lastIndex, i + 1];
				_cells[lastIndex, i].Left = _cells[lastIndex - 1, i];

				// bottom
				_cells[i, lastIndex].Left = _cells[i - 1, lastIndex];
				_cells[i, lastIndex].Top = _cells[i, lastIndex - 1];
				_cells[i, lastIndex].Right = _cells[i + 1, lastIndex];

				// left
				_cells[firstIndex, i].Top = _cells[firstIndex, i - 1];
				_cells[firstIndex, i].Right = _cells[firstIndex + 1, i];
				_cells[firstIndex, i].Bottom = _cells[firstIndex, i + 1];
			}


			// inner cells

			for (int x = firstIndex + 1; x <= lastIndex - 1; x++)
			{
				for (int y = firstIndex + 1; y <= lastIndex - 1; y++)
				{
					_cells[x, y].Top = _cells[x, y - 1];
					_cells[x, y].Right = _cells[x + 1, y];
					_cells[x, y].Bottom = _cells[x, y + 1];
					_cells[x, y].Left = _cells[x - 1, y];
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
			var closest = new Cell(0, 0);  // merely to ensure the initialization, will immediately be replaced
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
