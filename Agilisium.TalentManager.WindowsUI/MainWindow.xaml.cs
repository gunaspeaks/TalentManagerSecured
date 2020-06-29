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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Agilisium.TalentManager.WindowsUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Public Properties

        private string _UaId;
        public string UaId
        {
            get { return _UaId; }
            set { _UaId = value; }
        }

        #endregion

        #region Construction

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers - Menu Buttons

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //dbProcessor = new ApplicationDataBusinessProcessor();
            Unloaded += MainWindowUnloaded;
            //MessageHelper.ShowTemporaryMessage(CacheHelper.WindowsIdentityName);
        }

        private void MainWindowUnloaded(object sender, RoutedEventArgs e)
        {
            //dbProcessor.Dispose();
        }

        private void WindowContentRendered(object sender, EventArgs e)
        {
            UpdateUiStatusWithMessage("Initializing application settings");
            try
            {
                //RestrictedPage restrictedPage = new RestrictedPage();
                //applicationPages.Add("RestrictedPage", restrictedPage);

                #region Onboarding Screens

                //var onboardingReq = new Pages.Onboarding.OnboardingRequests();
                //onboardingReq.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnManageRequests);
                //applicationPages.Add(btnManageRequests.Name, onboardingReq);

                //var regApp = new Pages.Onboarding.ManageApplications();
                //regApp.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnManageApplication);
                //applicationPages.Add(btnManageApplication.Name, regApp);

                //var analysisHistory = new Pages.Onboarding.AnalysisHistoryPage();
                //analysisHistory.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnAnalysisHistory);
                //applicationPages.Add(btnAnalysisHistory.Name, analysisHistory);

                //var castOwner = new Pages.Onboarding.ManageApplicationOwner();
                //castOwner.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnApplicationOwner);
                //applicationPages.Add(btnApplicationOwner.Name, castOwner);

                #endregion

                #region Pre-Analysis Screens

                //var tableSize = new Pages.SourceCodeDelivery.TableSizeReader();
                //tableSize.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnTableSizeReader);
                //applicationPages.Add(btnTableSizeReader.Name, tableSize);

                //var automatedJobs = new Pages.Automation.CreateTripletSchema();
                //automatedJobs.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnCreateSchema);
                //applicationPages.Add(btnCreateSchema.Name, automatedJobs);

                //var srcAnalyzer = new Pages.SourceCodeDelivery.SourceFilesAnalyzer();
                //srcAnalyzer.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnAnalyzeSourceCode);
                //applicationPages.Add(btnAnalyzeSourceCode.Name, srcAnalyzer);

                //var compareSource = new Pages.Automation.CompareSourceVersions();
                //compareSource.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnCompareSourceCode);
                //applicationPages.Add(btnCompareSourceCode.Name, compareSource);

                #endregion

                #region Mini Tools

                //var upPage = new Cat.MiniTools.SqlToCastFormatConverter.ToolMainPage();
                //upPage.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnCppHeaderFilesFinder);
                //applicationPages.Add(btnCppHeaderFilesFinder.Name, upPage);

                #endregion

                #region Post-Analysis Screens

                //var manageDocs = new Pages.PostAnalysis.ManageAppDocs();
                //manageDocs.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnManageAppDocs);
                //applicationPages.Add(btnManageAppDocs.Name, manageDocs);

                ////var backup = new Pages.PostAnalysis.BackupApplication();
                ////backup.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                ////menuButtons.Add(btnBackupApplication);
                ////applicationPages.Add(btnBackupApplication.Name, backup);

                //var verifyRes = new Pages.PostAnalysis.VerifyAnalysisResults();
                //verifyRes.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnVerifyAnalysisResults);
                //applicationPages.Add(btnVerifyAnalysisResults.Name, verifyRes);

                //var backupDBServer = new Pages.PostAnalysis.BackupSchemasServerPage();
                //backupDBServer.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnBackupDatabase);
                //applicationPages.Add(btnBackupDatabase.Name, backupDBServer);

                //var backupDBLocal = new Pages.PostAnalysis.BackupSchemasLocalPage();
                //backupDBLocal.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnBackupDatabaseLocal);
                //applicationPages.Add(btnBackupDatabaseLocal.Name, backupDBLocal);

                //var restoreDB = new Pages.PostAnalysis.RestoreDatabasesPage();
                //restoreDB.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnRestoreDatabase);
                //applicationPages.Add(btnRestoreDatabase.Name, restoreDB);

                #endregion

                #region Automated Analysis Screens

                //var automation = new Pages.Automation.ConfigureAutomatedJobs();
                //automation.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnConfigureAutomatedJobs);
                //applicationPages.Add(btnConfigureAutomatedJobs.Name, automation);

                #endregion

                #region Supported Technologies Screens

                //var manTech = new Pages.TechSpec.ManageTechnologies();
                //manTech.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnManageTechnology);
                //applicationPages.Add(btnManageTechnology.Name, manTech);

                #endregion

                #region Application Settings Screens

                //var manFrame = new Pages.TechSpec.ManageFrameworks();
                //manFrame.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnManageFrameworks);
                //applicationPages.Add(btnManageFrameworks.Name, manFrame);

                //var manExt = new Pages.TechSpec.ManageFileExtensions();
                //manExt.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnManageExtensions);
                //applicationPages.Add(btnManageExtensions.Name, manExt);

                //var manLayout = new Pages.Settings.ChangeWindowBackground();
                //manLayout.CurrentMainWindow = this;
                //manLayout.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnChangeApplicationBackground);
                //applicationPages.Add(btnChangeApplicationBackground.Name, manLayout);

                //var appOwner = new Pages.Settings.ManageCastCoEs();
                //appOwner.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnManageCastCoe);
                //applicationPages.Add(btnManageCastCoe.Name, appOwner);

                //var mngDD = new Pages.Settings.ManageListItems();
                //mngDD.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnConfigureDropDownItems);
                //applicationPages.Add(btnConfigureDropDownItems.Name, mngDD);


                #endregion

                #region Reports Screens

                //var mnReport = new Pages.Reports.CastExecutionSummary();
                //mnReport.UpdateStatusEvent += ControlUpdateStatusEventHandler;
                //menuButtons.Add(btnMonthlyReport);
                //applicationPages.Add(btnMonthlyReport.Name, mnReport);

                #endregion

                //CacheHelper.LoadApplicationCache();

                //txbUserName.Text = String.Format("Welcome {0} !", CacheHelper.LoggedInUser.DisplayName);
                //var backObj = CacheHelper.GetSystemSettingByName(SystemSettingConstants.DEFAULT_APPLICATION_BACKGROUND);
                //if (backObj != null) Background = (Brush)App.Current.FindResource(backObj);

                brdToolBar.Background = (Brush)App.Current.FindResource("WindowBlackBackground");
            }
            catch (InvalidProgramException progExp)
            {
                //MessageHelper.ShowErrorMessage(progExp);
                DisableApplicationMenus();
            }
            catch (Exception exp)
            {
                //MessageHelper.ShowErrorMessage(exp);
            }
            finally
            {
                UpdateUiStatusWithMessage();
            }
        }

        private void DisableApplicationMenus()
        {
            Style disabledStyle = (Style)Application.Current.Resources["ToolbarButtonDisabledStyle"];
            foreach (var btn in menuButtons)
            {
                btn.IsEnabled = false;
                btn.Style = disabledStyle;
            }
            btnRefresh.IsEnabled = false;
            btnRefresh.Style = disabledStyle;
        }

        private void MenuButtonClickEventHandler(object sender, RoutedEventArgs e)
        {
            //Button menuBtn = sender as Button;
            //string buttonTag = menuBtn.Tag != null ? menuBtn.Tag.ToString() : "";
            //if (buttonTag == "Restricted" && (CacheHelper.LoggedInUser.CastAdminID > 3 && CacheHelper.LoggedInUser.CastAdminID != 100))
            //{
            //    LoadSelectedControl("RestrictedPage");
            //    return;
            //}
            //LoadSelectedControl(menuBtn.Name);
        }

        #endregion

        #region Private Data Members

        private Dictionary<string, UserControl> applicationPages = new Dictionary<string, UserControl>();
        private List<Button> menuButtons = new List<Button>();
        private UserControl SelectedPage;
        //private IToolBarCommands SelectedPageToolBar;
        //private ApplicationDataBusinessProcessor dbProcessor = null;

        #endregion

        #region Private Methods

        private void LoadSelectedControl(string controlName)
        {
            //if (applicationPages.ContainsKey(controlName) == false)
            //{
            //    MessageHelper.ShowInformationMessage("This page is under design.");
            //    return;
            //}

            //SelectedPage = applicationPages[controlName];
            //SelectedPageToolBar = (IToolBarCommands)SelectedPage;

            //if (contentPanel.Children.Count > 0)
            //{
            //    contentPanel.Children.Clear();
            //}
            //contentPanel.Children.Add(SelectedPage);
            //ChangeMenuButtonAccessbility(false, controlName);
            //expMainMenuExpander.IsExpanded = false;
            //EnableToolbarCommands();
        }

        private void UnloadCurrentControl()
        {
            //if (contentPanel.Children.Count > 0)
            //{
            //    contentPanel.Children.Clear();
            //}
            //SelectedPageToolBar = (IToolBarCommands)SelectedPage;
            //ChangeMenuButtonAccessbility(true);

            //Style disableStyle = (Style)Application.Current.Resources["ToolbarButtonDisabledStyle"];

            //btnCancel.Style = disableStyle;
            //btnDelete.Style = disableStyle;
            //btnNew.Style = disableStyle;
            //btnSave.Style = disableStyle;
            //btnUpdate.Style = disableStyle;
            //btnExport.Style = disableStyle;
            //btnRefresh.Content = "Refresh";
            //SelectedPage = null;
            //expMainMenuExpander.IsExpanded = true;
            //btnDynamic.Visibility = Visibility.Hidden;
        }

        private void EnableToolbarCommands()
        {
            //btnCancel.IsEnabled = true;
            //btnDelete.IsEnabled = SelectedPageToolBar.CanDelete;
            //btnNew.IsEnabled = SelectedPageToolBar.CanAddNew;
            //btnSave.IsEnabled = false;
            //btnUpdate.IsEnabled = SelectedPageToolBar.CanEdit;
            //btnExport.IsEnabled = SelectedPageToolBar.CanExport;
            //btnDynamic.Visibility = String.IsNullOrWhiteSpace(SelectedPageToolBar.DynamicActionName) == true ? Visibility.Hidden : Visibility.Visible;
            //if (String.IsNullOrWhiteSpace(SelectedPageToolBar.DynamicActionName) == false) btnDynamic.Content = SelectedPageToolBar.DynamicActionName;

            //Style enabledStyle = (Style)Application.Current.Resources["ToolbarButtonStyle"];
            //Style disabledStyle = (Style)Application.Current.Resources["ToolbarButtonDisabledStyle"];
            //btnCancel.Style = enabledStyle;
            //btnDelete.Style = SelectedPageToolBar.CanDelete ? enabledStyle : disabledStyle;
            //btnNew.Style = SelectedPageToolBar.CanAddNew ? enabledStyle : disabledStyle;
            //btnSave.Style = disabledStyle;
            //btnUpdate.Style = SelectedPageToolBar.CanEdit ? enabledStyle : disabledStyle; ;
            //btnExport.Style = SelectedPageToolBar.CanExport ? enabledStyle : disabledStyle; ;
        }

        private void ChangeMenuButtonAccessbility(bool isEnabled, string controlName = "")
        {
            Style style = isEnabled ? (Style)Application.Current.Resources["MenuButtonNormalStyle"] : (Style)Application.Current.Resources["MenuButtonDisabledStyle"];
            foreach (var btn in menuButtons)
            {
                btn.IsEnabled = isEnabled;
                btn.Style = style;
            }
            if (string.IsNullOrWhiteSpace(controlName) == false)
            {
                var bttn = menuButtons.FirstOrDefault(btn => btn.Name == controlName);
                if (bttn != null) bttn.Style = (Style)Application.Current.Resources["MenuButtonSelectedStyle"];
            }
        }

        private void EnableEditingAction(bool enableEdit)
        {
            Style style = (Style)Application.Current.Resources["ToolbarButtonStyle"];
            Style disabledStyle = (Style)Application.Current.Resources["ToolbarButtonDisabledStyle"];

            btnNew.Style = enableEdit ? disabledStyle : style;
            btnUpdate.Style = enableEdit ? disabledStyle : style;
            btnSave.Style = enableEdit ? style : disabledStyle;
            btnDelete.Style = enableEdit ? disabledStyle : style;
            btnRefresh.Content = enableEdit ? "Reset" : "Refresh";
            btnSave.IsEnabled = enableEdit;
            btnExport.Style = enableEdit ? disabledStyle : style;
        }

        //private void ControlUpdateStatusEventHandler(object sender, StatusEventArgs e)
        //{
        //    UpdateUiStatusWithMessage(e.StatusMessage);
        //}

        private void UpdateUiStatusWithMessage(string message = "")
        {
            contentPanel.Refresh();
            if (string.IsNullOrWhiteSpace(message))
            {
                contentPanel.Cursor = Cursors.Arrow;
                message = "Ready";
                //txbStatus.Foreground = Brushes.LimeGreen;
                expMainMenuExpander.IsEnabled = true;
                brdToolBar.IsEnabled = true;

            }
            else
            {
                contentPanel.Cursor = Cursors.Wait;
                contentPanel.Refresh();
                expMainMenuExpander.IsEnabled = false;
                brdToolBar.IsEnabled = false;
                //txbStatus.Foreground = Brushes.Red;
            }
            txbStatus.Text = message + " ...";
            brdStatus.Refresh();
            txbStatus.Refresh();
            contentPanel.Refresh();
            this.Refresh();
        }

        #endregion

        #region Event Handlers - Toolbar Buttons

        private void AddNewButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (SelectedPageToolBar.ExecuteAddNewOperation())
            //        EnableEditingAction(true);
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (SelectedPageToolBar.ExecuteEditOperation())
            //        EnableEditingAction(true);
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (SelectedPageToolBar.ExecuteSaveOperation())
            //    {
            //        EnableEditingAction(false);
            //    }
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    bool? res = SelectedPageToolBar.ExecuteCancelOperation();

            //    if (res == null) return;
            //    if (res == false)
            //    {
            //        EnableEditingAction(false);
            //        return;
            //    }

            //    if (res == true)
            //    {
            //        UnloadCurrentControl();
            //        expMainMenuExpander.IsExpanded = true;
            //        return;
            //    }
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    SelectedPageToolBar.ExecuteDeleteOperation();
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
        }

        private void RefreshButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (SelectedPageToolBar == null)
            //    {
            //        MessageHelper.ShowInformationMessage("Application Cache will be refreshed");
            //        UpdateUiStatusWithMessage("Reloadind application cache");
            //        CacheHelper.LoadApplicationCache();
            //        return;
            //    }
            //    SelectedPageToolBar.ExecuteRefreshOperation();
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
            //finally
            //{
            //    UpdateUiStatusWithMessage();
            //}
        }

        private void DynamicButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    SelectedPageToolBar.ExecuteDynamicOperation();
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageHelper.ShowErrorMessage(exp);
            //}
        }

        private void ExportButtonClick(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    SelectedPageToolBar.ExecuteExportOperation();
            //}
            //catch (NotImplementedException) { }
            //catch (Exception exp)
            //{
            //    MessageBox.Show(exp.Message);
            //}
        }

        #endregion
    }

    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            //uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
