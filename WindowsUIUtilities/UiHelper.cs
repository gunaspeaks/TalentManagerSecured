using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Cat.UiUtilities
{
    public static class UiHelper
    {
        //private static System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
        //private static System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();

        public static string ShowFolderBrowserDialog()
        {
            //System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
            //if (res == System.Windows.Forms.DialogResult.OK)
            //{
            //    return folderDialog.SelectedPath;
            //}
            return "";
        }

        public static void ShowGridWithAnimation(Grid control)
        {
            //DoubleAnimation dbl = new DoubleAnimation(5, TimeSpan.FromMilliseconds(500));
            //control.BeginAnimation(Grid.OpacityProperty, dbl);
        }

        public static void HideGridWithAnimation(Grid control)
        {
            //DoubleAnimation dbl = new DoubleAnimation(0, TimeSpan.FromMilliseconds(500));
            //control.BeginAnimation(Grid.OpacityProperty, dbl);
        }

        public static string ShowFileBrowserDialog()
        {
            //System.Windows.Forms.DialogResult res = fileDialog.ShowDialog();
            //fileDialog.Multiselect = true;
            //fileDialog.DefaultExt = ".backup";
            //if (res == System.Windows.Forms.DialogResult.OK)
            //{
            //    return fileDialog.FileName;
            //}
            return "";
        }

    }
}
