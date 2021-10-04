namespace UniformQuoridor.Core
{
	public class Fence 
	{
		public int CenterX { get; init; }

		public int CenterY { get; init; }

		public Axis Axis { get; init; }

		public Fence(int centerX, int centerY, Axis axis)
		{
			CenterX = centerX;
			CenterY = centerY;
			Axis = axis;
		}
	}

	public enum Axis
	{
		Horizontal, Vertical
	}
}
