using System;

namespace UniformQuoridor.View
{
    public class GameError
    {
        private readonly string _message;
        private readonly ViewParameters _viewParameters;
        
        public GameError(string message, ViewParameters viewParameters)
        {
            _message = message;
            _viewParameters = viewParameters;
        }

        public void Print()
        {
            GameView.ClearLines(_viewParameters.ErrorFirstIndex, 1);
            Console.Write($"Error: {_message} Please try again.");
        }
    }
}
