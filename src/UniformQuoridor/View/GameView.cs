using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
    public class GameView
    {
        private readonly GameSession _game;
        private readonly ViewParameters _params;

        public GameView(GameSession game)
        {
            _game = game;
            _params = new ViewParameters(game.Board);
        }

        public void AskForPrint()
        {
            var snapshotComponent = new GameSnapshot(_game.Board, _game.Players, _params);
            snapshotComponent.Print();
        }

        public string AskForInput()
        {
            var inputComponent = new GameInput(_game.CurrentPlayer, _params);
            return inputComponent.Input();
        }
    }
}