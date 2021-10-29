namespace UniformQuoridor.Core
{
    public class Cell 
    {
        public int Row { get; }
        
        public int Column { get; }

        public Cell Top { get; set; }

        public Cell Right { get; set; }

        public Cell Bottom { get; set; }

        public Cell Left { get; set; }

        public bool IsFree { get; set; } = true;
        
        public Cell(int row, int column) 
        {
            Row = row;
            Column = column;
        }
    }
}
