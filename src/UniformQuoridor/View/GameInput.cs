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
            
            ClearOldInputLines(_viewParameters.InputFirstIndex);
            Console.WriteLine($"Available cells to move to: {cellsView}");
            Console.Write($"Player {_player.Id}, please, enter your next action: ");
            return Console.ReadLine();
        }

        private static void ClearOldInputLines(int rowNumber)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(ViewParameters.FirstIndex, rowNumber + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(ViewParameters.FirstIndex, rowNumber);
        }
    }
}
