using System;

namespace UniformQuoridor.Core.Exceptions
{
    public class FenceLimitationException : Exception
    {
        public FenceLimitationException(string message) : base(message) { }
    }
}
