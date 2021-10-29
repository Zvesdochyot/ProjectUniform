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

            int initialColumnIndex = board.Size / 2;

            TargetCells = new Cell[board.Size];
            if (id == 1)
            {
                Cell = board.Cells[board.Size - 1, initialColumnIndex];
                for (int c = 0; c < board.Size; c++) TargetCells[c] = board.Cells[0, c];
            }
            else
            {
                Cell = board.Cells[0, initialColumnIndex];
                for (int c = 0; c < board.Size; c++) TargetCells[c] = board.Cells[board.Size - 1, c];
            }
        }
    }
}
