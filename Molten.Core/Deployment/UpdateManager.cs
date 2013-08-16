using System;
using System.Deployment.Application;
using System.Reflection;
using System.Windows;
using F = System.Windows.Forms;

namespace Molten.Core.Deployment
{
    /// <summary>
    /// Contains methods for managing ClickOnce updates for this application.
    /// </summary>
    /// <remarks>This was initially pulled from an MSDN example, but has since been altered / improved upon.</remarks>
    public static class UpdateManager
    {
        /// <summary>
        /// Checks for updates to the currently running application.
        /// </summary>
        /// <param name="displayCurrentMessage">Whether or not to display a message indicating that the application is up to date.</param>
        public static void CheckForUpdates(bool displayCurrentMessage = false)
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException)
                {
                    // No internet connection available.
                    MessageBox.Show("Unable to check for updates, or download the update package. Either the update server is offline, or this computer is unable to reach the update server via the internet.", "Update Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("The ClickOnce deployment package is corrupt. Please contact the application developer.\r\n\r\nError: " + ide.Message, "Update Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated for an unknown reason.\r\n\r\nError: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    try
                    {
                        string currentVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(3);
                        string newVersion = info.AvailableVersion.ToString(3);
                        MessageBox.Show("Installed version: " + currentVersion + "\r\nAvailable version: " + newVersion + "\r\n\r\nThe application will now download and update in the background. You will be notified again when it is complete.", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
                        ad.UpdateCompleted += (_, __) =>
                        {
                            MessageBox.Show("Successfully updated to version " + newVersion + ".\r\n\r\nThe application will now restart to complete the update.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            RestartApplication();
                        };
                        ad.UpdateAsync();
                    }
                    catch (DeploymentDownloadException dde)
                    {
                        MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (displayCurrentMessage)
                    {
                        MessageBox.Show("You are currently running the latest version of the software (version " + Assembly.GetEntryAssembly().GetName().Version.ToString(3) + "). There are no updates at this time.", "Up to date", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                if (displayCurrentMessage)
                {
                    MessageBox.Show("This application instance is missing its network deployment information, likely because the application was compiled and run directly from Visual Studio. Automatic updating only works if the application is installed with ClickOnce.", "Update error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// Curiously, this is how you best restart a WPF ClickOnce application. It also works for WinForms applications, as well.
        /// </summary>
        private static void RestartApplication()
        {
            F.Application.Restart();
            Environment.Exit(0);
        }
    }
}
