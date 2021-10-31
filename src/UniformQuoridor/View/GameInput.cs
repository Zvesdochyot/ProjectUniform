using System;
using UniformQuoridor.Core;

namespace UniformQuoridor.View
{
    public class GameInput
    {
        private readonly Player _player;
        private readonly ViewParameters _viewParameters;
        
        public GameInput(Player player, ViewParameters viewParameters)
        {
            _player = player;
            _viewParameters = viewParameters;
        }

        public string Input()
        {
            var available = Board.AvailableCells(_player);
            var cellsView = String.Join(", ", available);
            
            GameView.ClearLines(_viewParameters.InputFirstIndex, 2);
            Console.WriteLine($"Available cells to move to: {cellsView}");
            Console.Write($"{_player}, please, enter your next action: ");
            return Console.ReadLine();
        }
    }
}
