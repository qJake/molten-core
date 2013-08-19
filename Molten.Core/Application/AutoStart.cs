using System;
using Microsoft.Win32;

namespace Molten.Core
{
    /// <summary>
    /// Allows (de)registration of an application on startup.
    /// </summary>
    public static class AutoStart
    {
        /// <summary>
        /// Registers an application for startup via the Registry.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <param name="appPath">The path of the application.</param>
        /// <returns>True on success, false otherwise.</returns>
        /// <remarks>TODO: Don't eat the exceptions, do something useful with them, but still make it easy to use.</remarks>
        public static bool Register(string appName, string appPath)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser;
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key.SetValue(appName, appPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Unregisters an application for startup via the Registry.
        /// </summary>
        /// <param name="appName">The name of the application.</param>
        /// <returns>True on success, false otherwise.</returns>
        /// <remarks>TODO: Don't eat the exceptions, do something useful with them, but still make it easy to use.</remarks>
        public static bool Unregister(string appName)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser;
                key = key.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                key.DeleteValue(appName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
