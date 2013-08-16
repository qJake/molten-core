using System;

namespace Molten.Core.Storage
{
    /// <summary>
    /// Occurs whenever the system is unable to initialize a storage area.
    /// </summary>
    public class StorageInitializationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the StorageInitializationException class.
        /// </summary>
        public StorageInitializationException() : base() { }

        /// <summary>
        /// Initializes a new instance of the StorageInitializationException class with the specified message.
        /// </summary>
        public StorageInitializationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the StorageInitializationException class with the specified message and inner exception.
        /// </summary>
        public StorageInitializationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
