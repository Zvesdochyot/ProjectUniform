using UniformQuoridor.Core;

namespace UniformQuoridor.Controller.Actions
{
    public class MoveAction : ActionBase<Cell>
    {
        protected override string Name => "move";

        public MoveAction(Player player, string argument) : base(player, argument) { }

        protected override bool ArgumentIsValid(string argument)
        {
            throw new System.NotImplementedException();
        }

        protected override void InitCoreArgument(string argument)
        {
            throw new System.NotImplementedException();
        }
    }
}
