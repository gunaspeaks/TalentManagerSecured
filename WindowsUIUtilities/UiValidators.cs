using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Cat.UiUtilities
{
    public class UiValidators
    {
        public static bool IsValidMandatory(TextBox textControl, string fieldName, bool shouldDisplayMsg = true)
        {
            if (String.IsNullOrWhiteSpace(textControl.Text))
            {
                if (shouldDisplayMsg == false) return false;
                //textControl.Select(0, 0);
                throw new InvalidOperationException(String.Format("The \"{0}\" is required.", fieldName));
            }
            return true;
        }

        public static bool IsValidValue(TextBox textControl, string fieldName, bool shouldDisplayMsg = true)
        {
            return true;
        }

        public static bool IsValidMandatory(ComboBox comboControl, string fieldName, bool shouldDisplayMsg = true)
        {
            if (comboControl.SelectedItem == null)
            {
                if (shouldDisplayMsg == false) return false;
                //comboControl.Focus();
                throw new InvalidOperationException(String.Format("The field \"{0}\" is required.", fieldName));
            }
            return true;
        }

        public static bool IsValidMandatory(DatePicker dateControl, string fieldName, bool shouldDisplayMsg = true)
        {
            if (dateControl.SelectedDate.HasValue == false)
            {
                if (shouldDisplayMsg == false) return false;
                //dateControl.Focus();
                throw new InvalidOperationException(String.Format("The field \"{0}\" is required.", fieldName));
            }
            return true;
        }
    }
}
