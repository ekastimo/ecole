using System;

namespace Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Thrown when the workflow has a an error of is invalid
    /// </summary>
    public class ClientFriendlyException : Exception
    {
        /// <inheritdoc />
        public ClientFriendlyException(string message) : base(message)
        {
        }
    }

    [Serializable]
    public class NotFoundException : ClientFriendlyException
    {
        public NotFoundException(string message) : base(message) { }
    }

    [Serializable]
    public class NotAuthorizedException : ClientFriendlyException
    {
        public NotAuthorizedException(string message) : base(message) { }
    }

    [Serializable]
    public class ApplicationValidationException : ClientFriendlyException
    {
        public ApplicationValidationException(string message) : base(message) { }
    }

    /// <summary>
    /// Throw this exception when you encounter an existing record matching the one you are attempting to create
    /// </summary>
    [Serializable]
    public class DuplicateEntityException : ClientFriendlyException
    {
        public DuplicateEntityException(string message) : base(message) { }
    }
}