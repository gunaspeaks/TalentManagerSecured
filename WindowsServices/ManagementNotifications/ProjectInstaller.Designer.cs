namespace Agilisium.TalentManager.WindowsServices.ManagementNotifications
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ManagementNotificationsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.ManagementNotificationsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ManagementNotificationsServiceProcessInstaller
            // 
            this.ManagementNotificationsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalService;
            this.ManagementNotificationsServiceProcessInstaller.Password = null;
            this.ManagementNotificationsServiceProcessInstaller.Username = null;
            // 
            // ManagementNotificationsServiceInstaller
            // 
            this.ManagementNotificationsServiceInstaller.DelayedAutoStart = true;
            this.ManagementNotificationsServiceInstaller.Description = "A Windows service designed by Agilisium for sending alerts for POD managers about" +
    " the allocations on their umberella";
            this.ManagementNotificationsServiceInstaller.DisplayName = "Agilisium - Management Notifications Service";
            this.ManagementNotificationsServiceInstaller.ServiceName = "Agilisium - Management Notifications Service";
            this.ManagementNotificationsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ManagementNotificationsServiceProcessInstaller,
            this.ManagementNotificationsServiceInstaller});

        }

        #endregion
        public System.ServiceProcess.ServiceProcessInstaller ManagementNotificationsServiceProcessInstaller;
        public System.ServiceProcess.ServiceInstaller ManagementNotificationsServiceInstaller;
    }
}