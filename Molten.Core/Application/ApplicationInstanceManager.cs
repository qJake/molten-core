using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Molten.Core
{
    /// <summary>
    /// Creates an InstanceManager that, when disposed from the IDisposable interface, releases the mutex of the application.
    /// </summary>
    /// <remarks>TODO: Handle edge case and non-standard conditions. This implementation is not perfect.</remarks>
    public class ApplicationInstanceManager : IDisposable
    {
        private Mutex mutex;

        /// <summary>
        /// Initializes a new instance of the ApplicationInstanceManager class.
        /// </summary>
        /// <remarks>TODO: Throw an error if this application doesn't have a GUID in its assembly information.</remarks>
        public ApplicationInstanceManager()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
            string mutexId = string.Format("Local\\{{{0}}}", appGuid);
            mutex = new Mutex(false, mutexId);
        }

        /// <summary>
        /// Checks the application's GUID for an existing running instance of the application. Returns false if there is an existing instance running, true otherwise. This also creates and locks a mutex for the current process and holds it until <see cref="Dispose" /> is called.
        /// </summary>
        /// <returns>True if there are no other running instances, false otherwise.</returns>
        public bool CheckSingleInstance()
        {
            try
            {
                return mutex.WaitOne(100);
            }
            catch (AbandonedMutexException)
            {
                // Other process was terminated without releasing mutex, which is fine.
                return true;
            }
        }

        /// <summary>
        /// Disposes of this ApplicationInstanceManager and releases the mutex for this application.
        /// </summary>
        public void Dispose()
        {
            mutex.ReleaseMutex();
        }
    }
}
