namespace UniformQuoridor.Core
{
    public class Game
    {
        public Game(int size)
        {
            var board = new Board(size);
            var player1 = new Player(1, size);
            var player2 = new Player(2, size);
        }
    }
}
