using System;
using System.Text.RegularExpressions;
using UniformQuoridor.Core;
using UniformQuoridor.Core.Exceptions;

namespace UniformQuoridor.Controller.Actions
{
    public class PlaceAction : ActionBase<Fence>
    {
        public override string Name => "place";

        public PlaceAction(Player player, string argument) : base(player, argument)
        {
            if (player.RemainingFences == 0)
            {
                throw new FenceLimitationException(
                    "You have used the maximum number of available fences.");
            }
        }
        
        protected override bool ArgumentIsValid(string argument)
        {
            return Regex.IsMatch(argument, "^[S-Z][1-9](h|v)$");
        }

        protected override void InitCoreArgument(string argument)
        {
            int row = (int) Char.GetNumericValue(argument[1]) - 1;
            int column = argument[0] - 83;
            char axis = argument[2];
            CoreArgument = axis == 'h' ? new Fence(row, column, Axis.Horizontal) : new Fence(row, column, Axis.Vertical);
        }
    }
}
