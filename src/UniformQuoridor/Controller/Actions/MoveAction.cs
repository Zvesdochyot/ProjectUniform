using System;
using System.Text.RegularExpressions;
using UniformQuoridor.Core;

namespace UniformQuoridor.Controller.Actions
{
    public class MoveAction : ActionBase<Cell>
    {
        public override string Name => "move";

        public MoveAction(Player player, string argument) : base(player, argument) { }
        
        protected override bool ArgumentIsValid(string argument)
        {
            return Regex.IsMatch(argument, "^[A-I][1-9]$");
        }

        protected override void InitCoreArgument(string argument)
        {
            int row = (int) Char.GetNumericValue(argument[1]) - 1;
            int column = argument[0] - 65;
            CoreArgument = new Cell(row, column);
        }
    }
}
