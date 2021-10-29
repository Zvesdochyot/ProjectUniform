using UniformQuoridor.Core;

namespace UniformQuoridor.Controller.Actions
{
    public class PlaceAction : ActionBase<Fence>
    {
        protected override string Name => "place";

        public PlaceAction(Player player, string argument) : base(player, argument) { }
        
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
