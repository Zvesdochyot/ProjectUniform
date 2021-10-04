namespace UniformQuoridor.Model
{
	public class Game
	{
		public Game(int size)
		{
            var board = new Board(size);
			Player player1 = new Player(1, size), player2 = new Player(2, size);
		}
    }
}
