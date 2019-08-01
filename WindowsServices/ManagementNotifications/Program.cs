using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace Agilisium.TalentManager.WindowsServices.ManagementNotifications
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ManagementNotificationsService()
            };

            if (Environment.UserInteractive)
            {
                RunInteractive(ServicesToRun);
                return;
            }

            ServiceBase.Run(ServicesToRun);
        }

        private static void RunInteractive(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Service is running in interactive mode.");

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                onStartMethod.Invoke(service, new object[] { new string[] { } });
            }

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Stopped");
            }

            // Keep the console alive for a second to allow the user to see the message.
            Thread.Sleep(1000);
        }

    }
}
