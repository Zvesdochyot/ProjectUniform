using System;
using UniformQuoridor.Core;

namespace UniformQuoridor.Controller.Actions
{
    public abstract class ActionBase<T>
    {
        protected abstract string Name { get; }

        private string Argument { get; }
        
        private T CoreArgument { get; }
        
        private Player Player { get; }

        public ActionBase(Player player, string argument)
        {
            Player = player;
            ParseArgument(argument);
        }

        public void ParseArgument(string argument)
        {
            if (!ArgumentIsValid(argument))
            {
                throw new ArgumentException($"Invalid argument for action {Name}");
            }
            InitCoreArgument(argument);
        }

        protected abstract bool ArgumentIsValid(string argument);
        
        protected abstract void InitCoreArgument(string argument);
        
        public override string ToString()
        {
            return $"{Player} {Name} {Argument}";
        }
    }
}
