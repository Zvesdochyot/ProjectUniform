using System;

namespace UniformQuoridor.Core.Exceptions
{
    public class UnreachableCellException : Exception
    {
        public UnreachableCellException(string message) : base(message) { }
    }
}
