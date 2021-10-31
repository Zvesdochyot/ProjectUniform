using System;
using System.Threading;
using UniformQuoridor.Core;
using UniformQuoridor.Controller;
using UniformQuoridor.View;

namespace UniformQuoridor.Client
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            const int boardSize = 9;
            const int playersCount = 2;
            bool isClosed = false;

            while (!isClosed)
            {
                var game = new GameSession(boardSize, playersCount);
                var controller = new GameController(game);
                var view = new GameView(game);
                
                Console.Clear();
                
                GameView.PrintPrelude();

                bool isTypesCorrect;
                PlayerType[] playersType;
                do 
                {
                    string[] rawTypes = view.AskForPlayersType();
                    isTypesCorrect = GameController.TryParsePlayersType(rawTypes, out playersType);
                    GameView.ClearLines(ViewParameters.FirstIndex, 2);
                } while (!isTypesCorrect);
                controller.SetPlayersType(playersType);
                
                GameView.PrintCountdown();
                
                while (!game.IsEnded)
                {
                    try
                    {
                        // Dummy
                        if (game.CurrentPlayer.PlayerType == PlayerType.Computer)
                        {
                            controller.DoRandomAction();
                        }
                        
                        view.AskForPrint();
                        string rawInput = view.AskForInput();
                        controller.AcceptRequest(rawInput);
                        view.ClearErrorMessage();

                    }
                    catch (Exception exception)
                    {
                        view.PrintError(exception.Message);
                    }
                }
            
                Console.Clear();
                
                view.PrintWinner();

                bool isDecisionCorrect = false;
                do
                {
                    string decision = GameView.AskForRelapse();
                    switch (decision.ToLower())
                    {
                        case "restart":
                            isDecisionCorrect = true;
                            break;
                        case "exit":
                            isDecisionCorrect = true;
                            isClosed = true;
                            break;
                    }
                    GameView.ClearLines(ViewParameters.FirstIndex, 1);
                } while (!isDecisionCorrect);
            }
            
            GameView.SayGoodbye();
        }
    }
}
