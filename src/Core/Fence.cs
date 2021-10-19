namespace UniformQuoridor.Core
{
	public class Fence 
	{
		public int CenterRow { get; init; }

		public int CenterColumn { get; init; }

        public Axis Axis { get; }

		public Fence(int centerRow, int centerColumn, Axis axis)
		{
			CenterRow = centerRow;
			CenterColumn = centerColumn;
			Axis = axis;
		}
	}

    public enum Axis
    {
        Horizontal,
        Vertical
    }
}
