namespace UniformQuoridor.Core
{
	public class Player 
	{
		public int Id { get; init; }

		public Cell[] TargetCells { get; init; }

		public Cell Cell { get; set; }

		public Player(int id, Board board)
		{
			Id = id;

			TargetCells = new Cell[board.Size];
			if (id == 1)
			{
				for (int x = 0; x < board.Size; x++) TargetCells[x] = board.Cells[x, 0];
			}
			else
			{
				int lastRowIndex = board.Size - 1;
				for (int x = 0; x < board.Size; x++) TargetCells[x] = board.Cells[x, lastRowIndex];
			}
		}
	}
}
