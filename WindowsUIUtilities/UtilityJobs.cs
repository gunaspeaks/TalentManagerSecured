using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cat.UiUtilities
{
    public class UtilityJobs
    {
        public static void ExecuteCommandLineUtility(string applicationPath, string arguments = null)
        {
            Process process = null;
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo(applicationPath);
                if (arguments != null) processInfo.Arguments = arguments;
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = true;
                processInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process = Process.Start(processInfo);
                process.WaitForExit();
            }
            finally
            {
                if (process != null)
                {
                    if (process.HasExited == false) process.Kill();
                    process.Dispose();
                }
            }
        }

    }
}
