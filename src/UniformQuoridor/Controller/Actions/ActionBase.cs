using System;
using UniformQuoridor.Core;

namespace UniformQuoridor.Controller.Actions
{
    public abstract class ActionBase<T>
    {
        public abstract string Name { get; }

        public string Argument { get; private set; }
        
        public T CoreArgument { get; protected set; }

        public bool IsParsed { get; private set; }
        
        private Player Player { get; }

        protected ActionBase(Player player, string argument)
        {
            Player = player;
            ParseArgument(argument);
        }

        private void ParseArgument(string argument)
        {
            if (!ArgumentIsValid(argument))
            {
                throw new ArgumentException($"Invalid argument for action {Name}.");
            }

            IsParsed = true;
            Argument = argument;
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
