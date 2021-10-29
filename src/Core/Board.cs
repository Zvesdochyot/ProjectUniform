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

        public void AddFence(Fence fence)
        {
            Fences.Add(fence);

            var cellToTopLeft = Cells[fence.CenterRow, fence.CenterColumn];

            if (fence.Axis == Axis.Horizontal)
            {
                var cellToBottomLeft = cellToTopLeft.Bottom;
                cellToTopLeft.Bottom = null;
                cellToBottomLeft.Top = null;

                var cellToBottomRight = cellToTopLeft.Right.Bottom;
                cellToTopLeft.Right.Bottom = null;
                cellToBottomRight.Top = null;
            }
            else
            {
                var cellToTopRight = cellToTopLeft.Right;
                cellToTopLeft.Right = null;
                cellToTopRight.Left = null;

                var cellToBottomRight = cellToTopLeft.Bottom.Right;
                cellToTopLeft.Bottom.Right = null;
                cellToBottomRight.Left = null;
            }
        }

        private bool FenceIsAvailable(Fence fence)
        {
            if (Fences.Exists(
                (f) => f.CenterRow == fence.CenterRow && f.CenterColumn == fence.CenterColumn
            )) return false;

            var cellToTopLeft = Cells[fence.CenterRow, fence.CenterColumn];

            if (fence.Axis == Axis.Horizontal)
            {
                // neither of left and right vertical pairs has a fence in between
                return cellToTopLeft.Bottom != null && cellToTopLeft.Right.Bottom != null;
            }
            else
            {
                // if neither of top and bottom horizontal pairs ones has a fence in between
                return cellToTopLeft.Right != null && cellToTopLeft.Bottom.Right != null;
            }
        }

        public void RemoveFence(Fence fence) {
            Fences.Remove(fence);
        }

        public bool PathExists(Cell a, Cell b)
		{
			// depth-first, visiting the cell which is the closest one to the end cell

			var visited = new List<Cell>();
			var available = new Dictionary<Cell, double>() { {a, Distance(a, b)} };
			Cell current;

			while (available.Count != 0)
			{
				current = Closest(available);
                //
                Console.Write($"\n{available.Count} available:");
				foreach (var (c, d) in available)
                {
                    Console.Write($" [{c.Row}, {c.Column}]");
                }
                Console.WriteLine($"\nchosen: [{current.Row}, {current.Column}]");
                //

				if (current == b) return true;

				available.Remove(current);
				visited.Add(current);

				if (current.Top != null && !visited.Contains(current.Top))
				{
					available.TryAdd(current.Top, Distance(current.Top, b));
				}
				if (current.Right != null && !visited.Contains(current.Right))
				{
					available.TryAdd(current.Right, Distance(current.Right, b));
				}
				if (current.Bottom != null && !visited.Contains(current.Bottom))
				{
					available.TryAdd(current.Bottom, Distance(current.Bottom, b));
				}
				if (current.Left != null && !visited.Contains(current.Left))
				{
					available.TryAdd(current.Left, Distance(current.Left, b));
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

        public List<Cell> AvailableCells(Player player)
        {
            var available = new List<Cell>(5);

            available.AddRange(AvailableCellsToTop(player));
            available.AddRange(AvailableCellsToRight(player));
            available.AddRange(AvailableCellsToBottom(player));
            available.AddRange(AvailableCellsToLeft(player));

            return available;
        }

        private List<Cell> AvailableCellsToTop(Player player)
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

        private List<Cell> AvailableCellsToRight(Player player)
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

        private List<Cell> AvailableCellsToBottom(Player player)
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

        private List<Cell> AvailableCellsToLeft(Player player)
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

            int lastCellIndex = Size - 1;
            Fence candidate;

            for (int r = 0; r <=  lastCellIndex - 1; r++)
            {
                for (int c = 0; c <=  lastCellIndex - 1; c++)
                {
                    candidate = new Fence(r, c, Axis.Horizontal);
                    if (FenceIsAvailable(candidate)) available.Add(candidate);

                    candidate = new Fence(r, c, Axis.Vertical);
                    if (FenceIsAvailable(candidate)) available.Add(candidate);
                }
            }

            return available;
        }
    }
}
