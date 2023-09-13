using System;
using System.Runtime.Serialization;

namespace TaskManagerWpf.Logic
{
    [Serializable]
    internal class SameTitleException : Exception
    {
        public SameTitleException()
        {
        }

        public SameTitleException(string? message) : base(message)
        {
        }

        public SameTitleException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SameTitleException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}