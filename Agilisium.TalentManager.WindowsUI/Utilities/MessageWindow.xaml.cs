using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cat.UserInterface.Utilities
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        #region Private Members

        private static MessageWindow currentWind;

        private static bool isUserSelecedOk = true;

        #endregion

        #region Public Data Members

        public static string Remarks
        {
            get { return currentWind.txtMessage.Text.Trim(); }
        }

        #endregion

        #region Constructor

        private MessageWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            isUserSelecedOk = true;
            this.Close();
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            isUserSelecedOk = false;
            this.Close();
        }

        #endregion

        #region Public Methods

        public static void Show(string message, string title = null)
        {
            showMessageWindow(message, title);
        }

        public static void Show(List<string> messageList, string title = null, bool isValidationMsg = true)
        {
            StringBuilder msg = new StringBuilder();
            if (isValidationMsg)
            {
                msg.Append("Please fix the below validation issues before Saving" + Environment.NewLine);
                msg.Append("---------------------------------------------------------" + Environment.NewLine);
            }

            if (messageList != null && (messageList != null && messageList.Count > 0))
            {
                foreach (var str in messageList) msg.Append(string.Format("{0}{1}", str, Environment.NewLine));
            }

            string msgText = string.IsNullOrWhiteSpace(msg.ToString()) ? "No items found" : msg.ToString();
            showMessageWindow(msgText, title, false);
        }

        public static void Show(Exception exp)
        {
            showMessageWindow(string.Format("Error Message: {0} from \"{1}\"", exp.Message, exp.Source), "Application Error", true);
        }

        public static bool GetConfirmation(string message)
        {
            showMessageWindow(message, "Confirm Your Decision", false, true);
            return isUserSelecedOk;
        }

        public static string GetRemarks(string title)
        {
            showRemarksWindow(title);
            return currentWind.txtMessage.Text.Trim();
        }

        #endregion

        #region Private Methods

        private static void showMessageWindow(string message, string title = null, bool isErrorMsg = false, bool requireConfirm = false)
        {
            currentWind = new MessageWindow();
            currentWind.Topmost = true;
            currentWind.DragMove();
            if (string.IsNullOrWhiteSpace(title)) title = "Application Alert";
            if (isErrorMsg) currentWind.Background = (Brush)Brushes.Red;

            currentWind.btnOk.Content = requireConfirm ? "Yes" : "Okay";
            currentWind.btnNo.Visibility = requireConfirm ? Visibility.Visible : Visibility.Collapsed;
            currentWind.btnOk.IsCancel = requireConfirm ? false : true;
            currentWind.txbMessageTitle.Text = title;
            currentWind.txtMessage.Text = message;
            currentWind.ShowDialog();
        }

        private static void showRemarksWindow(string title)
        {
            currentWind = new MessageWindow();
            currentWind.Topmost = true;
            currentWind.DragMove();
            currentWind.btnOk.Content = "Okay";
            currentWind.btnNo.Visibility = Visibility.Collapsed;
            currentWind.btnOk.IsCancel = true;
            currentWind.txbMessageTitle.Text = title;
            currentWind.txtMessage.IsReadOnly = false;
            currentWind.txtMessage.Focus();
            currentWind.ShowDialog();
        }

        #endregion
    }
}
