using System;

namespace UrbanNative.Domain.Exceptions
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string? errorcode,string message)
            : base(message)
        {
        }
    }
}
