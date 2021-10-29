using System;

namespace UniformQuoridor.Core.Exceptions
{
    public class FenceUnplaceableException : Exception
    {
        public FenceUnplaceableException(string message) : base(message) { }
    }
}
