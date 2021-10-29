using UniformQuoridor.Core;
using UniformQuoridor.Controller.Actions;


namespace UniformQuoridor.View
{
	class View {
		private GameSnapshot _gameSnapshot;

		public View(Board board, Player player1, Player player2)
		{
			_gameSnapshot = new GameSnapshot(board, player1, player2);
		}

		public void Update(dynamic action)
		{
			if (action is MoveAction) _gameSnapshot.MovePlayer(action.Player);
			else _gameSnapshot.AddFence(action.CoreArgument);

			// todo: update CommandLine

			// todo: update Actions
		}
	}
}
