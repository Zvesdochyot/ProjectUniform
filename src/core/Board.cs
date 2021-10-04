using System;
using System.Collections.Generic;

namespace UniformQuoridor.Model
{
	public class Board
	{
		private int _size;
	
		public Board(int size)
		{
			_size = size;
			InitAdjacencyList(InitCells(size*size));
		}

		private Dictionary<int, Cell> InitCells(int numCells)
		{
			var cells = new Dictionary<int, Cell>(numCells);

			for (int x = 1, id = 1; x <= _size; x++)
			{
				for (int y = 1; y <= _size; y++, id++)
				{
					cells[_size * x + y] = new Cell(x, y);
				}
			}

			return cells;
		}			

		private void InitAdjacencyList(Dictionary<int, Cell> cells)
		{
			// corner cells

			int topLeftCornerId = 1;
			cells[topLeftCornerId].Right = cells[topLeftCornerId + 1];
			cells[topLeftCornerId].Bottom = cells[topLeftCornerId + _size];

			int topRightCornerId = _size;
			cells[topRightCornerId].Bottom =  cells[topRightCornerId + _size];
			cells[topRightCornerId].Left = cells[topRightCornerId - 1];
			
			int bottomRightCornerId = cells.Count;
			cells[bottomRightCornerId].Left = cells[bottomRightCornerId - 1];
			cells[bottomRightCornerId].Top = cells[bottomRightCornerId - _size];

			int bottomLeftCornerId = cells.Count - _size + 1;
			cells[bottomLeftCornerId].Top = cells[bottomLeftCornerId - _size];
			cells[bottomLeftCornerId].Right =  cells[bottomLeftCornerId + 1];


			// edge cells

			int firstTopEdgeId = topLeftCornerId + 1, lastTopEdgeId = topRightCornerId - 1;
			for (int c = firstTopEdgeId; c <= lastTopEdgeId; c++)
			{
				cells[c].Right = cells[c + 1];
				cells[c].Bottom = cells[c + _size];
				cells[c].Left = cells[c - 1];
			}
			
			int firstRightEdgeId = topRightCornerId + _size, lastRightEdgeId = bottomRightCornerId - _size;
			for (int c = firstRightEdgeId; c <= lastRightEdgeId; c++)
			{
				cells[c].Top = cells[c - _size];
				cells[c].Bottom = cells[c + _size];
				cells[c].Left = cells[c - 1];
			}

			int firstBottomEdgeId = bottomLeftCornerId + 1, lastBottomEdgeId = bottomRightCornerId - 1;
			for (int c = firstBottomEdgeId; c <= lastBottomEdgeId; c++)
			{
				cells[c].Left = cells[c - 1];
				cells[c].Top = cells[c - _size];
				cells[c].Right = cells[c + 1];
			}
			
			int firstLeftEdgeId = topLeftCornerId + _size, lastLeftEdgeId = bottomLeftCornerId - _size;
			for (int c = firstLeftEdgeId; c <= lastLeftEdgeId; c++)
			{
				cells[c].Top = cells[c - _size];
				cells[c].Right = cells[c + 1];
				cells[c].Bottom = cells[c + _size];
			}

			// central cells

			int firstCentralRowId = firstLeftEdgeId + 1, lastCentralRowId = lastLeftEdgeId + 1;
			int numCellsInCentralRow = _size - 2;
			for (int rowFirstId = firstCentralRowId; rowFirstId <= lastCentralRowId; rowFirstId += _size)
			{
				for (int i = rowFirstId; i < rowFirstId + numCellsInCentralRow; i++)
				{
					cells[rowFirstId].Top = cells[rowFirstId - _size];
					cells[rowFirstId].Right = cells[rowFirstId + 1];
					cells[rowFirstId].Bottom = cells[rowFirstId + _size];
					cells[rowFirstId].Left = cells[rowFirstId - 1];
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
