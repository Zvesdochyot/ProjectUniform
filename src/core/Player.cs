using System.Linq;

namespace UniformQuoridor.Core
{
	public class Player 
	{
		public int Id { get; init; }
		
		public int[] TargetCellIds { get; init; }

		public Cell Cell { get; init; }

		public Player(int id, int boardSize)
		{
			Id = id;
			
			TargetCellIds = new int[boardSize];
			if (id == 1)
			{
				TargetCellIds = Enumerable.Range(1, boardSize).ToArray();
			}
			else
			{
				int lastCellId = boardSize * boardSize;
				TargetCellIds = Enumerable.Range(lastCellId - boardSize + 1, lastCellId).ToArray();
			}
		}
	}
}
