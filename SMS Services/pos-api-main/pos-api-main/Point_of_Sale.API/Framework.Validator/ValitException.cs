using System;

namespace ECom.Framework.Validator
{
    public class ValitException : Exception
    {
        public ValitException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
    }
}
