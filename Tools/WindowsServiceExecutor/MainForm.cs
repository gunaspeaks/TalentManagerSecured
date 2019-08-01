using Agilisium.TalentManager.ServiceProcessors;
using log4net;
using System;
using System.Windows.Forms;

namespace Agilisium.TalentManager.Tools.WindowsServiceExecutor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void EmailReportingServiceButton_Click(object sender, EventArgs e)
        {
            try
            {
                MyLogger.LogMessage("");
                MyLogger.LogMessage("*********************************************************************************************");
                MyLogger.LogMessage("Email Service has started processing");

                MessageBox.Show(DateTime.Today.DayOfWeek.ToString());

                AllocationsMessengerServiceProcessor processor = new AllocationsMessengerServiceProcessor();
                MyLogger.LogMessage("Generating resource allocation report");
                //processor.GenerateResourceAllocationReport();
                MyLogger.LogMessage("Service execution complete.");

                MessageBox.Show("Service execution complete.");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                MyLogger.LogMessage("Error while generating allocation report", exp);
            }
            finally
            {
                MyLogger.LogMessage("*********************************************************************************************");
            }
        }

        private void AllocationsUpdatorServiceButton_Click(object sender, EventArgs e)
        {
            try
            {
                MyLogger.LogMessage("");
                MyLogger.LogMessage("*********************************************************************************************");
                MyLogger.LogMessage("Allocation Updator Service has initiated");

                AllocationsUpdaterServiceProcessor processor = new AllocationsUpdaterServiceProcessor();

                //processor.ProcessAllocations();
                MyLogger.LogMessage("Service execution complete.");

                MessageBox.Show("Service execution complete.");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                MyLogger.LogMessage("Error while executing Allocation Updator service", exp);
            }
            finally
            {
                MyLogger.LogMessage("*********************************************************************************************");
            }
        }
    }
}
