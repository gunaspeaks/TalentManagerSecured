using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Agilisium.TalentManager.WindowsUIUtilities
{
    public static class MessageHelper
    {
        public static MessageBoxResult ShowErrorMessage(Exception exp)
        {
            return ShowMessage(exp.Message, "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static MessageBoxResult ShowMessage(string message, string v, object oK, object error)
        {
            throw new NotImplementedException();
        }

        private static MessageBoxResult ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage msgIcon)
        {
            return System.Windows.MessageBox.Show(message, title, buttons, msgIcon);
        }

        public static MessageBoxResult ShowInformationMessage(string message)
        {
            return ShowMessage(message, "Application Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static MessageBoxResult ShowWarningMessage(string message)
        {
            return ShowMessage(message, "Application Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static bool GetUserConfirmation(string message)
        {
            MessageBoxResult res= ShowMessage(message, "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return res == MessageBoxResult.Yes;
        }

        public static MessageBoxResult ShowTemporaryMessage(string message)
        {
            //if (ConfigurationSettings.CanShowTemporarynMessages == false) return MessageBoxResult.OK;
            return ShowMessage(message, "Application Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //
    }
}
