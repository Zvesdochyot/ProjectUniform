using System;

namespace UniformQuoridor.Core.Exceptions
{
    public class UnplaceableFenceException : Exception
    {
        public UnplaceableFenceException(string message) : base(message) { }
    }
}
