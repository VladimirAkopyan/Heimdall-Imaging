namespace HighSpecter
{
    partial class UI_NetStatusPanel
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI_NetStatusPanel));
            this.UI_LabelGPS = new System.Windows.Forms.Label();
            this.UI_displayLattitude = new System.Windows.Forms.Label();
            this.UI_displayLongitude = new System.Windows.Forms.Label();
            this.UI_LabelLight = new System.Windows.Forms.Label();
            this.UI_displayL1 = new System.Windows.Forms.Label();
            this.UI_displayL2 = new System.Windows.Forms.Label();
            this.UI_Feedback = new System.Windows.Forms.TextBox();
            this.UI_StatusPanel = new System.Windows.Forms.Panel();
            this.UI_LabelConnectonStatus = new System.Windows.Forms.Label();
            this.UI_DisplayConnectionStatus = new System.Windows.Forms.Label();
            this.UI_LabelImaging = new System.Windows.Forms.Label();
            this.UI_DisplayImagging = new System.Windows.Forms.Label();
            this.UI_LabelExposure = new System.Windows.Forms.Label();
            this.UI_DisplayExposure = new System.Windows.Forms.Label();
            this.UI_LabelFps = new System.Windows.Forms.Label();
            this.UI_DisplayFps = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.UI_LabelNetworkConnections = new System.Windows.Forms.Label();
            this.UI_DisplayNetworkConnections = new System.Windows.Forms.Label();
            this.UI_LabelSentTelemetry = new System.Windows.Forms.Label();
            this.UI_DisplaySentTelemetry = new System.Windows.Forms.Label();
            this.UI_LabelIPAdress = new System.Windows.Forms.Label();
            this.UI_DisplayIpAdress = new System.Windows.Forms.Label();
            this.UI_PanelNetworkCommunication = new System.Windows.Forms.Panel();
            this.UI_LabelRecievedComms = new System.Windows.Forms.Label();
            this.UI_DisplayRecievedComms = new System.Windows.Forms.Label();
            this.UI_Panel_NetworkSettings = new System.Windows.Forms.Panel();
            this.UI_LabelPort = new System.Windows.Forms.Label();
            this.UI_DisplayPort = new System.Windows.Forms.Label();
            this.UI_StatusPanel.SuspendLayout();
            this.UI_PanelNetworkCommunication.SuspendLayout();
            this.UI_Panel_NetworkSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // UI_LabelGPS
            // 
            this.UI_LabelGPS.AutoSize = true;
            this.UI_LabelGPS.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelGPS.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelGPS.Location = new System.Drawing.Point(4, 72);
            this.UI_LabelGPS.Name = "UI_LabelGPS";
            this.UI_LabelGPS.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelGPS.Size = new System.Drawing.Size(151, 30);
            this.UI_LabelGPS.TabIndex = 30;
            this.UI_LabelGPS.Text = "GPS Readings:";
            // 
            // UI_displayLattitude
            // 
            this.UI_displayLattitude.AutoSize = true;
            this.UI_displayLattitude.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_displayLattitude.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_displayLattitude.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_displayLattitude.Location = new System.Drawing.Point(220, 62);
            this.UI_displayLattitude.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayLattitude.Name = "UI_displayLattitude";
            this.UI_displayLattitude.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayLattitude.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_displayLattitude.Size = new System.Drawing.Size(81, 42);
            this.UI_displayLattitude.TabIndex = 31;
            this.UI_displayLattitude.Text = "N/a";
            // 
            // UI_displayLongitude
            // 
            this.UI_displayLongitude.AutoSize = true;
            this.UI_displayLongitude.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_displayLongitude.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_displayLongitude.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_displayLongitude.Location = new System.Drawing.Point(423, 58);
            this.UI_displayLongitude.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayLongitude.Name = "UI_displayLongitude";
            this.UI_displayLongitude.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayLongitude.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_displayLongitude.Size = new System.Drawing.Size(81, 42);
            this.UI_displayLongitude.TabIndex = 32;
            this.UI_displayLongitude.Text = "N/a";
            // 
            // UI_LabelLight
            // 
            this.UI_LabelLight.AutoSize = true;
            this.UI_LabelLight.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelLight.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelLight.Location = new System.Drawing.Point(4, 125);
            this.UI_LabelLight.Name = "UI_LabelLight";
            this.UI_LabelLight.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelLight.Size = new System.Drawing.Size(159, 30);
            this.UI_LabelLight.TabIndex = 33;
            this.UI_LabelLight.Text = "Light Readings:";
            // 
            // UI_displayL1
            // 
            this.UI_displayL1.AutoSize = true;
            this.UI_displayL1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_displayL1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_displayL1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_displayL1.Location = new System.Drawing.Point(220, 112);
            this.UI_displayL1.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayL1.Name = "UI_displayL1";
            this.UI_displayL1.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayL1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_displayL1.Size = new System.Drawing.Size(81, 42);
            this.UI_displayL1.TabIndex = 34;
            this.UI_displayL1.Text = "N/a";
            this.UI_displayL1.Click += new System.EventHandler(this.UI_displayL1_Click);
            // 
            // UI_displayL2
            // 
            this.UI_displayL2.AutoSize = true;
            this.UI_displayL2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_displayL2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_displayL2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_displayL2.Location = new System.Drawing.Point(423, 109);
            this.UI_displayL2.Margin = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayL2.Name = "UI_displayL2";
            this.UI_displayL2.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.UI_displayL2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_displayL2.Size = new System.Drawing.Size(81, 42);
            this.UI_displayL2.TabIndex = 35;
            this.UI_displayL2.Text = "N/a";
            // 
            // UI_Feedback
            // 
            this.UI_Feedback.CausesValidation = false;
            this.UI_Feedback.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.UI_Feedback.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UI_Feedback.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_Feedback.HideSelection = false;
            this.UI_Feedback.Location = new System.Drawing.Point(0, 335);
            this.UI_Feedback.Name = "UI_Feedback";
            this.UI_Feedback.ReadOnly = true;
            this.UI_Feedback.ShortcutsEnabled = false;
            this.UI_Feedback.Size = new System.Drawing.Size(684, 30);
            this.UI_Feedback.TabIndex = 36;
            this.UI_Feedback.TabStop = false;
            this.UI_Feedback.Text = "Ready";
            this.UI_Feedback.WordWrap = false;
            // 
            // UI_StatusPanel
            // 
            this.UI_StatusPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.UI_StatusPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UI_StatusPanel.Controls.Add(this.UI_LabelConnectonStatus);
            this.UI_StatusPanel.Controls.Add(this.UI_DisplayConnectionStatus);
            this.UI_StatusPanel.Controls.Add(this.UI_LabelImaging);
            this.UI_StatusPanel.Controls.Add(this.UI_DisplayImagging);
            this.UI_StatusPanel.Controls.Add(this.UI_LabelExposure);
            this.UI_StatusPanel.Controls.Add(this.UI_DisplayExposure);
            this.UI_StatusPanel.Controls.Add(this.UI_LabelFps);
            this.UI_StatusPanel.Controls.Add(this.UI_DisplayFps);
            this.UI_StatusPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.UI_StatusPanel.Location = new System.Drawing.Point(0, 0);
            this.UI_StatusPanel.Name = "UI_StatusPanel";
            this.UI_StatusPanel.Size = new System.Drawing.Size(684, 50);
            this.UI_StatusPanel.TabIndex = 37;
            // 
            // UI_LabelConnectonStatus
            // 
            this.UI_LabelConnectonStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_LabelConnectonStatus.AutoSize = true;
            this.UI_LabelConnectonStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelConnectonStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelConnectonStatus.Location = new System.Drawing.Point(549, 5);
            this.UI_LabelConnectonStatus.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelConnectonStatus.Name = "UI_LabelConnectonStatus";
            this.UI_LabelConnectonStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelConnectonStatus.Size = new System.Drawing.Size(54, 32);
            this.UI_LabelConnectonStatus.TabIndex = 36;
            this.UI_LabelConnectonStatus.Text = "Net";
            // 
            // UI_DisplayConnectionStatus
            // 
            this.UI_DisplayConnectionStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_DisplayConnectionStatus.AutoSize = true;
            this.UI_DisplayConnectionStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_DisplayConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayConnectionStatus.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_DisplayConnectionStatus.Location = new System.Drawing.Point(603, 3);
            this.UI_DisplayConnectionStatus.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayConnectionStatus.Name = "UI_DisplayConnectionStatus";
            this.UI_DisplayConnectionStatus.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayConnectionStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayConnectionStatus.Size = new System.Drawing.Size(69, 38);
            this.UI_DisplayConnectionStatus.TabIndex = 35;
            this.UI_DisplayConnectionStatus.Text = "No";
            // 
            // UI_LabelImaging
            // 
            this.UI_LabelImaging.AutoSize = true;
            this.UI_LabelImaging.Dock = System.Windows.Forms.DockStyle.Left;
            this.UI_LabelImaging.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelImaging.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelImaging.Location = new System.Drawing.Point(0, 0);
            this.UI_LabelImaging.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelImaging.Name = "UI_LabelImaging";
            this.UI_LabelImaging.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelImaging.Size = new System.Drawing.Size(102, 32);
            this.UI_LabelImaging.TabIndex = 29;
            this.UI_LabelImaging.Text = "Imaging";
            // 
            // UI_DisplayImagging
            // 
            this.UI_DisplayImagging.AutoEllipsis = true;
            this.UI_DisplayImagging.AutoSize = true;
            this.UI_DisplayImagging.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_DisplayImagging.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayImagging.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_DisplayImagging.Location = new System.Drawing.Point(102, 3);
            this.UI_DisplayImagging.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayImagging.Name = "UI_DisplayImagging";
            this.UI_DisplayImagging.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayImagging.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayImagging.Size = new System.Drawing.Size(69, 38);
            this.UI_DisplayImagging.TabIndex = 34;
            this.UI_DisplayImagging.Text = "No";
            this.UI_DisplayImagging.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UI_LabelExposure
            // 
            this.UI_LabelExposure.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_LabelExposure.AutoSize = true;
            this.UI_LabelExposure.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelExposure.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelExposure.Location = new System.Drawing.Point(199, 5);
            this.UI_LabelExposure.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelExposure.Name = "UI_LabelExposure";
            this.UI_LabelExposure.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelExposure.Size = new System.Drawing.Size(57, 32);
            this.UI_LabelExposure.TabIndex = 33;
            this.UI_LabelExposure.Text = "Exp.";
            // 
            // UI_DisplayExposure
            // 
            this.UI_DisplayExposure.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_DisplayExposure.AutoSize = true;
            this.UI_DisplayExposure.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_DisplayExposure.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayExposure.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_DisplayExposure.Location = new System.Drawing.Point(256, 3);
            this.UI_DisplayExposure.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayExposure.Name = "UI_DisplayExposure";
            this.UI_DisplayExposure.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayExposure.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayExposure.Size = new System.Drawing.Size(69, 38);
            this.UI_DisplayExposure.TabIndex = 32;
            this.UI_DisplayExposure.Text = "No";
            // 
            // UI_LabelFps
            // 
            this.UI_LabelFps.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_LabelFps.AutoSize = true;
            this.UI_LabelFps.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelFps.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelFps.Location = new System.Drawing.Point(375, 5);
            this.UI_LabelFps.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelFps.Name = "UI_LabelFps";
            this.UI_LabelFps.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelFps.Size = new System.Drawing.Size(51, 32);
            this.UI_LabelFps.TabIndex = 31;
            this.UI_LabelFps.Text = "Fps";
            // 
            // UI_DisplayFps
            // 
            this.UI_DisplayFps.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_DisplayFps.AutoSize = true;
            this.UI_DisplayFps.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UI_DisplayFps.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayFps.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_DisplayFps.Location = new System.Drawing.Point(425, 5);
            this.UI_DisplayFps.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayFps.Name = "UI_DisplayFps";
            this.UI_DisplayFps.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayFps.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayFps.Size = new System.Drawing.Size(69, 38);
            this.UI_DisplayFps.TabIndex = 30;
            this.UI_DisplayFps.Text = "No";
            // 
            // UI_LabelNetworkConnections
            // 
            this.UI_LabelNetworkConnections.AutoSize = true;
            this.UI_LabelNetworkConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UI_LabelNetworkConnections.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelNetworkConnections.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelNetworkConnections.Location = new System.Drawing.Point(0, 0);
            this.UI_LabelNetworkConnections.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelNetworkConnections.Name = "UI_LabelNetworkConnections";
            this.UI_LabelNetworkConnections.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelNetworkConnections.Size = new System.Drawing.Size(120, 28);
            this.UI_LabelNetworkConnections.TabIndex = 29;
            this.UI_LabelNetworkConnections.Text = "Connections";
            this.toolTip1.SetToolTip(this.UI_LabelNetworkConnections, "Total Connections since application was started");
            // 
            // UI_DisplayNetworkConnections
            // 
            this.UI_DisplayNetworkConnections.AutoEllipsis = true;
            this.UI_DisplayNetworkConnections.AutoSize = true;
            this.UI_DisplayNetworkConnections.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayNetworkConnections.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_DisplayNetworkConnections.Location = new System.Drawing.Point(120, 2);
            this.UI_DisplayNetworkConnections.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayNetworkConnections.Name = "UI_DisplayNetworkConnections";
            this.UI_DisplayNetworkConnections.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayNetworkConnections.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayNetworkConnections.Size = new System.Drawing.Size(43, 32);
            this.UI_DisplayNetworkConnections.TabIndex = 34;
            this.UI_DisplayNetworkConnections.Text = "0";
            this.UI_DisplayNetworkConnections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.UI_DisplayNetworkConnections, "Total Connections since application was started");
            // 
            // UI_LabelSentTelemetry
            // 
            this.UI_LabelSentTelemetry.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_LabelSentTelemetry.AutoSize = true;
            this.UI_LabelSentTelemetry.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelSentTelemetry.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelSentTelemetry.Location = new System.Drawing.Point(219, 2);
            this.UI_LabelSentTelemetry.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelSentTelemetry.Name = "UI_LabelSentTelemetry";
            this.UI_LabelSentTelemetry.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelSentTelemetry.Size = new System.Drawing.Size(139, 28);
            this.UI_LabelSentTelemetry.TabIndex = 33;
            this.UI_LabelSentTelemetry.Text = "Sent telemetry";
            this.toolTip1.SetToolTip(this.UI_LabelSentTelemetry, "Number of Telemtry frames Sent");
            // 
            // UI_DisplaySentTelemetry
            // 
            this.UI_DisplaySentTelemetry.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_DisplaySentTelemetry.AutoSize = true;
            this.UI_DisplaySentTelemetry.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplaySentTelemetry.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_DisplaySentTelemetry.Location = new System.Drawing.Point(367, 0);
            this.UI_DisplaySentTelemetry.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplaySentTelemetry.Name = "UI_DisplaySentTelemetry";
            this.UI_DisplaySentTelemetry.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplaySentTelemetry.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplaySentTelemetry.Size = new System.Drawing.Size(43, 32);
            this.UI_DisplaySentTelemetry.TabIndex = 32;
            this.UI_DisplaySentTelemetry.Text = "0";
            this.toolTip1.SetToolTip(this.UI_DisplaySentTelemetry, "Number of Telemtry frames Sent");
            // 
            // UI_LabelIPAdress
            // 
            this.UI_LabelIPAdress.AutoSize = true;
            this.UI_LabelIPAdress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelIPAdress.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelIPAdress.Location = new System.Drawing.Point(4, -5);
            this.UI_LabelIPAdress.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelIPAdress.Name = "UI_LabelIPAdress";
            this.UI_LabelIPAdress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelIPAdress.Size = new System.Drawing.Size(103, 28);
            this.UI_LabelIPAdress.TabIndex = 29;
            this.UI_LabelIPAdress.Text = "IP Address";
            this.toolTip1.SetToolTip(this.UI_LabelIPAdress, "IP adress where connection is being awaited");
            // 
            // UI_DisplayIpAdress
            // 
            this.UI_DisplayIpAdress.AutoEllipsis = true;
            this.UI_DisplayIpAdress.AutoSize = true;
            this.UI_DisplayIpAdress.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayIpAdress.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_DisplayIpAdress.Location = new System.Drawing.Point(196, -5);
            this.UI_DisplayIpAdress.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayIpAdress.Name = "UI_DisplayIpAdress";
            this.UI_DisplayIpAdress.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayIpAdress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayIpAdress.Size = new System.Drawing.Size(43, 32);
            this.UI_DisplayIpAdress.TabIndex = 34;
            this.UI_DisplayIpAdress.Text = "0";
            this.UI_DisplayIpAdress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.UI_DisplayIpAdress, "Total Connections since application was started");
            // 
            // UI_PanelNetworkCommunication
            // 
            this.UI_PanelNetworkCommunication.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.UI_PanelNetworkCommunication.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UI_PanelNetworkCommunication.Controls.Add(this.UI_LabelNetworkConnections);
            this.UI_PanelNetworkCommunication.Controls.Add(this.UI_DisplayNetworkConnections);
            this.UI_PanelNetworkCommunication.Controls.Add(this.UI_LabelSentTelemetry);
            this.UI_PanelNetworkCommunication.Controls.Add(this.UI_DisplaySentTelemetry);
            this.UI_PanelNetworkCommunication.Controls.Add(this.UI_LabelRecievedComms);
            this.UI_PanelNetworkCommunication.Controls.Add(this.UI_DisplayRecievedComms);
            this.UI_PanelNetworkCommunication.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UI_PanelNetworkCommunication.Location = new System.Drawing.Point(0, 301);
            this.UI_PanelNetworkCommunication.Name = "UI_PanelNetworkCommunication";
            this.UI_PanelNetworkCommunication.Size = new System.Drawing.Size(684, 34);
            this.UI_PanelNetworkCommunication.TabIndex = 42;
            // 
            // UI_LabelRecievedComms
            // 
            this.UI_LabelRecievedComms.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_LabelRecievedComms.AutoSize = true;
            this.UI_LabelRecievedComms.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelRecievedComms.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelRecievedComms.Location = new System.Drawing.Point(467, 2);
            this.UI_LabelRecievedComms.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelRecievedComms.Name = "UI_LabelRecievedComms";
            this.UI_LabelRecievedComms.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelRecievedComms.Size = new System.Drawing.Size(136, 28);
            this.UI_LabelRecievedComms.TabIndex = 31;
            this.UI_LabelRecievedComms.Text = "Recieved Com";
            // 
            // UI_DisplayRecievedComms
            // 
            this.UI_DisplayRecievedComms.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_DisplayRecievedComms.AutoSize = true;
            this.UI_DisplayRecievedComms.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayRecievedComms.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.UI_DisplayRecievedComms.Location = new System.Drawing.Point(621, 0);
            this.UI_DisplayRecievedComms.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayRecievedComms.Name = "UI_DisplayRecievedComms";
            this.UI_DisplayRecievedComms.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayRecievedComms.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayRecievedComms.Size = new System.Drawing.Size(43, 32);
            this.UI_DisplayRecievedComms.TabIndex = 30;
            this.UI_DisplayRecievedComms.Text = "0";
            // 
            // UI_Panel_NetworkSettings
            // 
            this.UI_Panel_NetworkSettings.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.UI_Panel_NetworkSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UI_Panel_NetworkSettings.Controls.Add(this.UI_LabelIPAdress);
            this.UI_Panel_NetworkSettings.Controls.Add(this.UI_DisplayIpAdress);
            this.UI_Panel_NetworkSettings.Controls.Add(this.UI_LabelPort);
            this.UI_Panel_NetworkSettings.Controls.Add(this.UI_DisplayPort);
            this.UI_Panel_NetworkSettings.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.UI_Panel_NetworkSettings.Location = new System.Drawing.Point(0, 274);
            this.UI_Panel_NetworkSettings.Name = "UI_Panel_NetworkSettings";
            this.UI_Panel_NetworkSettings.Size = new System.Drawing.Size(684, 27);
            this.UI_Panel_NetworkSettings.TabIndex = 43;
            // 
            // UI_LabelPort
            // 
            this.UI_LabelPort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_LabelPort.AutoSize = true;
            this.UI_LabelPort.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_LabelPort.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_LabelPort.Location = new System.Drawing.Point(459, -5);
            this.UI_LabelPort.Margin = new System.Windows.Forms.Padding(0);
            this.UI_LabelPort.Name = "UI_LabelPort";
            this.UI_LabelPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_LabelPort.Size = new System.Drawing.Size(49, 28);
            this.UI_LabelPort.TabIndex = 31;
            this.UI_LabelPort.Text = "Port";
            // 
            // UI_DisplayPort
            // 
            this.UI_DisplayPort.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.UI_DisplayPort.AutoSize = true;
            this.UI_DisplayPort.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_DisplayPort.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.UI_DisplayPort.Location = new System.Drawing.Point(561, -5);
            this.UI_DisplayPort.Margin = new System.Windows.Forms.Padding(0);
            this.UI_DisplayPort.Name = "UI_DisplayPort";
            this.UI_DisplayPort.Padding = new System.Windows.Forms.Padding(10, 2, 10, 2);
            this.UI_DisplayPort.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UI_DisplayPort.Size = new System.Drawing.Size(43, 32);
            this.UI_DisplayPort.TabIndex = 30;
            this.UI_DisplayPort.Text = "0";
            // 
            // UI_NetStatusPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 365);
            this.Controls.Add(this.UI_Panel_NetworkSettings);
            this.Controls.Add(this.UI_PanelNetworkCommunication);
            this.Controls.Add(this.UI_StatusPanel);
            this.Controls.Add(this.UI_Feedback);
            this.Controls.Add(this.UI_LabelGPS);
            this.Controls.Add(this.UI_displayLattitude);
            this.Controls.Add(this.UI_displayLongitude);
            this.Controls.Add(this.UI_LabelLight);
            this.Controls.Add(this.UI_displayL1);
            this.Controls.Add(this.UI_displayL2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.MinimumSize = new System.Drawing.Size(697, 391);
            this.Name = "UI_NetStatusPanel";
            this.Text = "HIghSpecter";
            this.ResizeEnd += new System.EventHandler(this.Resized);
            this.Resize += new System.EventHandler(this.Resized);
            this.UI_StatusPanel.ResumeLayout(false);
            this.UI_StatusPanel.PerformLayout();
            this.UI_PanelNetworkCommunication.ResumeLayout(false);
            this.UI_PanelNetworkCommunication.PerformLayout();
            this.UI_Panel_NetworkSettings.ResumeLayout(false);
            this.UI_Panel_NetworkSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UI_LabelGPS;
        private System.Windows.Forms.Label UI_displayLattitude;
        private System.Windows.Forms.Label UI_displayLongitude;
        private System.Windows.Forms.Label UI_LabelLight;
        private System.Windows.Forms.Label UI_displayL1;
        private System.Windows.Forms.Label UI_displayL2;
        private System.Windows.Forms.TextBox UI_Feedback;
        private System.Windows.Forms.Panel UI_StatusPanel;
        private System.Windows.Forms.Label UI_LabelImaging;
        private System.Windows.Forms.Label UI_DisplayImagging;
        private System.Windows.Forms.Label UI_LabelExposure;
        private System.Windows.Forms.Label UI_DisplayExposure;
        private System.Windows.Forms.Label UI_LabelFps;
        private System.Windows.Forms.Label UI_DisplayFps;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label UI_LabelConnectonStatus;
        private System.Windows.Forms.Label UI_DisplayConnectionStatus;
        private System.Windows.Forms.Panel UI_PanelNetworkCommunication;
        private System.Windows.Forms.Label UI_LabelNetworkConnections;
        private System.Windows.Forms.Label UI_DisplayNetworkConnections;
        private System.Windows.Forms.Label UI_LabelSentTelemetry;
        private System.Windows.Forms.Label UI_DisplaySentTelemetry;
        private System.Windows.Forms.Label UI_LabelRecievedComms;
        private System.Windows.Forms.Label UI_DisplayRecievedComms;
        private System.Windows.Forms.Panel UI_Panel_NetworkSettings;
        private System.Windows.Forms.Label UI_LabelIPAdress;
        private System.Windows.Forms.Label UI_DisplayIpAdress;
        private System.Windows.Forms.Label UI_LabelPort;
        private System.Windows.Forms.Label UI_DisplayPort;

    }
}

