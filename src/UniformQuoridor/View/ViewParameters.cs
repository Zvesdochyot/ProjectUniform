using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
    public class ViewParameters
    {
        public const int FirstIndex = 0;
        public const int CellAreaFirstIndex = 2;
        
        public int LastIndex { get; }
        
        public int CellAreaLastIndex { get; }
        
        public int InputFirstIndex { get; }
        
        public int ErrorFirstIndex { get; }

        public ViewParameters(Board board)
        {
            int cellAreaSize = board.Size * 2 - 1;
            CellAreaLastIndex = CellAreaFirstIndex + cellAreaSize - 1;
            LastIndex = CellAreaLastIndex + 2;
            InputFirstIndex = LastIndex + 1;
            ErrorFirstIndex = InputFirstIndex + 2;
        }
    }
}
