using System;
using System.Linq;
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

        public static bool TryParsePlayersType(string[] rawTypes, out PlayerType[] types)
        {
            types = new PlayerType[rawTypes.Length];
            
            for (int i = 0; i < rawTypes.Length; i++)
            {
                if (Enum.TryParse<PlayerType>(rawTypes[i], true, out var playerType))
                {
                    types[i] = playerType;
                }
                else
                {
                    return false;
                }
            }

            return !types.All(type => type == PlayerType.Computer);
        }
        
        public void SetPlayersType(PlayerType[] playersType)
        {
            for (int i = 0; i < _game.Players.Length; i++)
            {
                _game.Players[i].PlayerType = playersType[i];
            }
        }
        
        public void AcceptRequest(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("The action you have entered is empty.");
            }

            var divided = input.Split(PartsSeparator);

            if (divided.Length != 2)
            {
                throw new ArgumentException("The action you have entered is invalid.");
            }
            
            ParseAction(divided[0], divided[1]);
        }

        public void DoRandomAction()
        {
            var random = new Random();
            int result = random.Next(2);
            if (result == 0)
            {
                _game.RandomMove();
            }
            else
            {
                _game.RandomPlace();
            }
        }
        
        private void ParseAction(string command, string argument)
        {
            switch (command.ToLower())
            {
                case MoveCommand:
                    var moveAction = new MoveAction(_game.CurrentPlayer, argument);
                    if (moveAction.IsParsed)
                    {
                        _game.Move(moveAction.CoreArgument.Row, moveAction.CoreArgument.Column);
                    }
                    break;
                case PlaceCommand:
                    var placeAction = new PlaceAction(_game.CurrentPlayer, argument);
                    if (placeAction.IsParsed)
                    {
                        _game.Place(placeAction.CoreArgument.CenterRow, placeAction.CoreArgument.CenterColumn, placeAction.CoreArgument.Axis);
                    }
                    break;
                default:
                    throw new ArgumentException("The action‘s command you have entered is invalid.");
            }
        }
    }
}
