namespace UniformQuoridor.Core
{
    public class Fence
    {
        public int CenterX { get; }

        public int CenterY { get; }

        public Axis Axis { get; }

        public Fence(int centerX, int centerY, Axis axis)
        {
            CenterX = centerX;
            CenterY = centerY;
            Axis = axis;
        }
    }

    public enum Axis
    {
        Horizontal,
        Vertical
    }
}
