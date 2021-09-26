using System;

namespace Exceptions
{
    public class InvalidResourceException : Exception
    {
        protected InvalidResourceException() { }

        public InvalidResourceException(string message) : base(message) { }
    }
}