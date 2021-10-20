using System;

namespace ShortUrl.Core.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string message) : base(message)
        {

        }
    }
}
