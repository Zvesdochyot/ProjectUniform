namespace UniformQuoridor.Core
{
    public class Player
    {
        public int Id { get; }

        public Cell[] TargetCells { get; }

        public Cell Cell { get; }

        public Player(int id, int boardSize)
        {
            Id = id;
            TargetCells = new Cell[boardSize];
            
            const int firstRowIndex = 0;
            int lastRowIndex = boardSize - 1;
            if (id == 1)
            {
                Cell = new Cell(lastRowIndex, boardSize / 2);
                for (int c = 0; c < boardSize; c++)
                {
                    TargetCells[c] = new Cell(firstRowIndex, c);
                }
            }
            else
            {
                Cell = new Cell(firstRowIndex, boardSize / 2);
                for (int c = 0; c < boardSize; c++)
                {
                    TargetCells[c] = new Cell(lastRowIndex, c);
                }
            }
        }
    }
}
