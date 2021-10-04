using System;
using System.Collections.Generic;

namespace UniformQuoridor.Core
{
	public class Board
	{
		private int _size;

		private Dictionary<int, Cell> _cells;
		private Dictionary<int, Fence> _fences;

		public Board(int size)
		{
			_size = size;
			InitCells();
		}

		private void InitCells()
		{
			_cells = new Dictionary<int, Cell>(_size * _size);

			for (int x = 1, id = 1; x <= _size; x++)
			{
				for (int y = 1; y <= _size; y++, id++)
				{
					_cells[_size * x + y] = new Cell(x, y);
				}
			}

			InitAdjacentCells();
		}

		private void InitAdjacentCells()
		{
			// corner cells

			int topLeftCornerId = 1;
			_cells[topLeftCornerId].Right = cellToRightOfId(topLeftCornerId);
			_cells[topLeftCornerId].Bottom = cellToBottomOfId(topLeftCornerId);

			int topRightCornerId = _size;
			_cells[topRightCornerId].Bottom = cellToBottomOfId(topRightCornerId);
			_cells[topRightCornerId].Left = cellToLeftOfId(topRightCornerId);
			
			int bottomRightCornerId = _cells.Count;
			_cells[bottomRightCornerId].Left = cellToLeftOfId(bottomRightCornerId);
			_cells[bottomRightCornerId].Top = cellToTopOfId(bottomRightCornerId);

			int bottomLeftCornerId = _cells.Count - _size + 1;
			_cells[bottomLeftCornerId].Top =cellToTopOfId(bottomLeftCornerId);
			_cells[bottomLeftCornerId].Right =  cellToRightOfId(bottomLeftCornerId);


			// edge cells

			int firstTopEdgeId = topLeftCornerId + 1, lastTopEdgeId = topRightCornerId - 1;
			for (int c = firstTopEdgeId; c <= lastTopEdgeId; c++)
			{
				_cells[c].Right = cellToRightOfId(c);
				_cells[c].Bottom = cellToBottomOfId(c);
				_cells[c].Left = cellToLeftOfId(c);
			}
			
			int firstRightEdgeId = topRightCornerId + _size, lastRightEdgeId = bottomRightCornerId - _size;
			for (int c = firstRightEdgeId; c <= lastRightEdgeId; c++)
			{
				_cells[c].Top = cellToTopOfId(c);
				_cells[c].Bottom = cellToBottomOfId(c);
				_cells[c].Left = cellToLeftOfId(c);
			}

			int firstBottomEdgeId = bottomLeftCornerId + 1, lastBottomEdgeId = bottomRightCornerId - 1;
			for (int c = firstBottomEdgeId; c <= lastBottomEdgeId; c++)
			{
				_cells[c].Left = cellToLeftOfId(c);
				_cells[c].Top = cellToTopOfId(c);
				_cells[c].Right = cellToRightOfId(c);
			}
			
			int firstLeftEdgeId = topLeftCornerId + _size, lastLeftEdgeId = bottomLeftCornerId - _size;
			for (int c = firstLeftEdgeId; c <= lastLeftEdgeId; c++)
			{
				_cells[c].Top = cellToTopOfId(c);
				_cells[c].Right = cellToRightOfId(c);
				_cells[c].Bottom = cellToBottomOfId(c);
			}


			// central cells

			int firstCentralRowId = firstLeftEdgeId + 1, lastCentralRowId = lastLeftEdgeId + 1;
			int numCellsInCentralRow = _size - 2;
			for (int rowFirstId = firstCentralRowId; rowFirstId <= lastCentralRowId; rowFirstId += _size)
			{
				for (int i = rowFirstId; i < rowFirstId + numCellsInCentralRow; i++)
				{
					_cells[rowFirstId].Top = cellToTopOfId(rowFirstId);
					_cells[rowFirstId].Right = cellToRightOfId(rowFirstId);
					_cells[rowFirstId].Bottom = cellToBottomOfId(rowFirstId);
					_cells[rowFirstId].Left = cellToLeftOfId(rowFirstId);
				}
			}
		}

		private Cell cellToTopOfId(int id) => _cells[id - _size];

		private Cell cellToRightOfId(int id) => _cells[id + 1];

		private Cell cellToBottomOfId(int id) => _cells[id + _size];

		private Cell cellToLeftOfId(int id) => _cells[id - 1];

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
