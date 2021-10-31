using System;
using System.Threading;
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

        public static void PrintPrelude()
        {
            Console.WriteLine("Welcome to the console board game Quoridor. We hope you enjoy your stay in our (un)uniform game.");
            Thread.Sleep(3000);
            ClearLines(ViewParameters.FirstIndex, 1);
        }

        public string[] AskForPlayersType()
        {
            string[] rawPlayersType = new string[_game.Players.Length];
            
            for (var i = 0; i < _game.Players.Length; i++)
            {
                Console.Write($"{_game.Players[i]}, please, choose your type (computer / human): ");
                rawPlayersType[i] = Console.ReadLine();
            }

            return rawPlayersType;
        }

        public static void PrintCountdown()
        {
            for (int i = 3; i > 0; i--)
            {
                Console.Write($"The Game starts in {i}..");
                Thread.Sleep(1000);
                ClearLines(ViewParameters.FirstIndex, 1);
            }
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

        public void PrintError(string message)
        {
            var errorComponent = new GameError(message, _params);
            errorComponent.Print();
        }

        public void ClearErrorMessage()
        {
            ClearLines(_params.ErrorFirstIndex, 1);
        }
        
        public void PrintWinner()
        {
            Console.Write($"{_game.CurrentPlayer} won. Congrats!");
            Thread.Sleep(3000);
            ClearLines(ViewParameters.FirstIndex, 1);
        }

        public static string AskForRelapse()
        {
            Console.Write("Please, choose if you want to restart or exit the game (restart / exit): ");
            return Console.ReadLine();
        }

        public static void SayGoodbye()
        {
            Console.Write("See you next time!");
            Thread.Sleep(3000);
        }
        
        public static void ClearLines(int startingRowIndex, int rowsCount)
        {
            for (int i = 0; i < rowsCount; i++)
            {
                Console.SetCursorPosition(ViewParameters.FirstIndex, startingRowIndex + i);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(ViewParameters.FirstIndex, startingRowIndex);
        }
    }
}
