using System;
using UniformQuoridor.Controller.Actions;
using UniformQuoridor.Core;

namespace UniformQuoridor.Controller
{
    public class GameController
    {
        private const char PartsSeparator = ' ';
        
        private const string MoveCommand = "move";
        private const string PlaceCommand = "place";

        private readonly GameSession _game;
            
        public GameController(GameSession game)
        {
            _game = game;
        }
        
        public void AcceptRequest(string input)
        {
            var divided = input.Split(PartsSeparator);

            if (divided.Length != 2)
            {
                // TODO: throw an error on wrong input
            }
            
            ParseAction(divided[0], divided[1]);
        }

        private void ParseAction(string command, string argument)
        {
            switch (command.ToLower())
            {
                case MoveCommand:
                    var moveAction = new MoveAction(_game.CurrentPlayer, argument);
                    if (moveAction.IsParsed)
                    {
                        // Console.WriteLine($"Trying to move player to {moveAction.CoreArgument.Row} {moveAction.CoreArgument.Column}");
                        _game.Move(moveAction.CoreArgument.Row, moveAction.CoreArgument.Column);
                    }
                    break;
                case PlaceCommand:
                    var placeAction = new PlaceAction(_game.CurrentPlayer, argument);
                    if (placeAction.IsParsed)
                    {
                        // Console.WriteLine($"Trying to place fence at {placeAction.CoreArgument.CenterRow} {placeAction.CoreArgument.CenterColumn} {placeAction.CoreArgument.Axis}");
                        _game.Place(placeAction.CoreArgument.CenterRow, placeAction.CoreArgument.CenterColumn, placeAction.CoreArgument.Axis);
                    }
                    break;
                default:
                    // TODO: throw an error on wrong input
                    break;
            }
        }
        
        
    }
}
