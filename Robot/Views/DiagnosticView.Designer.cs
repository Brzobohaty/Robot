﻿namespace Robot
{
    partial class DiagnosticView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagnosticView));
            this.labelMessage = new System.Windows.Forms.Label();
            this.labelDiagnostic = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.robotCanvas = new System.Windows.Forms.PictureBox();
            this.labelLZ_Z = new System.Windows.Forms.Label();
            this.labelPP_Z = new System.Windows.Forms.Label();
            this.labelPZ_Z = new System.Windows.Forms.Label();
            this.labelLP_Z = new System.Windows.Forms.Label();
            this.labelPP_ZK = new System.Windows.Forms.Label();
            this.labelLZ_ZK = new System.Windows.Forms.Label();
            this.labelPZ_ZK = new System.Windows.Forms.Label();
            this.labelLP_ZK = new System.Windows.Forms.Label();
            this.labelPP_R = new System.Windows.Forms.Label();
            this.labelPZ_R = new System.Windows.Forms.Label();
            this.labelLZ_R = new System.Windows.Forms.Label();
            this.labelLP_R = new System.Windows.Forms.Label();
            this.labelPZ_P = new System.Windows.Forms.Label();
            this.labelLP_P = new System.Windows.Forms.Label();
            this.labelLZ_P = new System.Windows.Forms.Label();
            this.labelPP_P = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.robotCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelMessage.ForeColor = System.Drawing.Color.Red;
            this.labelMessage.Location = new System.Drawing.Point(0, 1104);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(747, 55);
            this.labelMessage.TabIndex = 3;
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDiagnostic
            // 
            this.labelDiagnostic.BackColor = System.Drawing.SystemColors.GrayText;
            this.labelDiagnostic.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDiagnostic.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelDiagnostic.Location = new System.Drawing.Point(0, 0);
            this.labelDiagnostic.Name = "labelDiagnostic";
            this.labelDiagnostic.Size = new System.Drawing.Size(747, 55);
            this.labelDiagnostic.TabIndex = 2;
            this.labelDiagnostic.Text = "Diagnostika";
            this.labelDiagnostic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.robotCanvas);
            this.panel1.Controls.Add(this.labelLZ_Z);
            this.panel1.Controls.Add(this.labelPP_Z);
            this.panel1.Controls.Add(this.labelPZ_Z);
            this.panel1.Controls.Add(this.labelLP_Z);
            this.panel1.Controls.Add(this.labelPP_ZK);
            this.panel1.Controls.Add(this.labelLZ_ZK);
            this.panel1.Controls.Add(this.labelPZ_ZK);
            this.panel1.Controls.Add(this.labelLP_ZK);
            this.panel1.Controls.Add(this.labelPP_R);
            this.panel1.Controls.Add(this.labelPZ_R);
            this.panel1.Controls.Add(this.labelLZ_R);
            this.panel1.Controls.Add(this.labelLP_R);
            this.panel1.Controls.Add(this.labelPZ_P);
            this.panel1.Controls.Add(this.labelLP_P);
            this.panel1.Controls.Add(this.labelLZ_P);
            this.panel1.Controls.Add(this.labelPP_P);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(747, 1049);
            this.panel1.TabIndex = 22;
            // 
            // robotCanvas
            // 
            this.robotCanvas.Location = new System.Drawing.Point(39, 456);
            this.robotCanvas.Name = "robotCanvas";
            this.robotCanvas.Size = new System.Drawing.Size(509, 458);
            this.robotCanvas.TabIndex = 38;
            this.robotCanvas.TabStop = false;
            // 
            // labelLZ_Z
            // 
            this.labelLZ_Z.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLZ_Z.Image = ((System.Drawing.Image)(resources.GetObject("labelLZ_Z.Image")));
            this.labelLZ_Z.Location = new System.Drawing.Point(125, 268);
            this.labelLZ_Z.Name = "labelLZ_Z";
            this.labelLZ_Z.Size = new System.Drawing.Size(70, 70);
            this.labelLZ_Z.TabIndex = 37;
            this.labelLZ_Z.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPP_Z
            // 
            this.labelPP_Z.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPP_Z.Image = ((System.Drawing.Image)(resources.GetObject("labelPP_Z.Image")));
            this.labelPP_Z.Location = new System.Drawing.Point(386, 149);
            this.labelPP_Z.Name = "labelPP_Z";
            this.labelPP_Z.Size = new System.Drawing.Size(70, 70);
            this.labelPP_Z.TabIndex = 36;
            this.labelPP_Z.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPZ_Z
            // 
            this.labelPZ_Z.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPZ_Z.Image = ((System.Drawing.Image)(resources.GetObject("labelPZ_Z.Image")));
            this.labelPZ_Z.Location = new System.Drawing.Point(386, 268);
            this.labelPZ_Z.Name = "labelPZ_Z";
            this.labelPZ_Z.Size = new System.Drawing.Size(70, 70);
            this.labelPZ_Z.TabIndex = 35;
            this.labelPZ_Z.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelLP_Z
            // 
            this.labelLP_Z.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLP_Z.Image = ((System.Drawing.Image)(resources.GetObject("labelLP_Z.Image")));
            this.labelLP_Z.Location = new System.Drawing.Point(125, 149);
            this.labelLP_Z.Name = "labelLP_Z";
            this.labelLP_Z.Size = new System.Drawing.Size(70, 70);
            this.labelLP_Z.TabIndex = 34;
            this.labelLP_Z.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPP_ZK
            // 
            this.labelPP_ZK.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPP_ZK.Image = ((System.Drawing.Image)(resources.GetObject("labelPP_ZK.Image")));
            this.labelPP_ZK.Location = new System.Drawing.Point(310, 149);
            this.labelPP_ZK.Name = "labelPP_ZK";
            this.labelPP_ZK.Size = new System.Drawing.Size(70, 70);
            this.labelPP_ZK.TabIndex = 33;
            this.labelPP_ZK.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelLZ_ZK
            // 
            this.labelLZ_ZK.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLZ_ZK.Image = ((System.Drawing.Image)(resources.GetObject("labelLZ_ZK.Image")));
            this.labelLZ_ZK.Location = new System.Drawing.Point(201, 268);
            this.labelLZ_ZK.Name = "labelLZ_ZK";
            this.labelLZ_ZK.Size = new System.Drawing.Size(70, 70);
            this.labelLZ_ZK.TabIndex = 32;
            this.labelLZ_ZK.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPZ_ZK
            // 
            this.labelPZ_ZK.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPZ_ZK.Image = ((System.Drawing.Image)(resources.GetObject("labelPZ_ZK.Image")));
            this.labelPZ_ZK.Location = new System.Drawing.Point(310, 268);
            this.labelPZ_ZK.Name = "labelPZ_ZK";
            this.labelPZ_ZK.Size = new System.Drawing.Size(70, 70);
            this.labelPZ_ZK.TabIndex = 31;
            this.labelPZ_ZK.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelLP_ZK
            // 
            this.labelLP_ZK.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLP_ZK.Image = ((System.Drawing.Image)(resources.GetObject("labelLP_ZK.Image")));
            this.labelLP_ZK.Location = new System.Drawing.Point(201, 149);
            this.labelLP_ZK.Name = "labelLP_ZK";
            this.labelLP_ZK.Size = new System.Drawing.Size(70, 70);
            this.labelLP_ZK.TabIndex = 30;
            this.labelLP_ZK.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPP_R
            // 
            this.labelPP_R.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPP_R.Image = ((System.Drawing.Image)(resources.GetObject("labelPP_R.Image")));
            this.labelPP_R.Location = new System.Drawing.Point(402, 43);
            this.labelPP_R.Name = "labelPP_R";
            this.labelPP_R.Size = new System.Drawing.Size(70, 70);
            this.labelPP_R.TabIndex = 29;
            this.labelPP_R.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPZ_R
            // 
            this.labelPZ_R.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPZ_R.Image = ((System.Drawing.Image)(resources.GetObject("labelPZ_R.Image")));
            this.labelPZ_R.Location = new System.Drawing.Point(402, 374);
            this.labelPZ_R.Name = "labelPZ_R";
            this.labelPZ_R.Size = new System.Drawing.Size(70, 70);
            this.labelPZ_R.TabIndex = 28;
            this.labelPZ_R.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelLZ_R
            // 
            this.labelLZ_R.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLZ_R.Image = ((System.Drawing.Image)(resources.GetObject("labelLZ_R.Image")));
            this.labelLZ_R.Location = new System.Drawing.Point(112, 374);
            this.labelLZ_R.Name = "labelLZ_R";
            this.labelLZ_R.Size = new System.Drawing.Size(70, 70);
            this.labelLZ_R.TabIndex = 27;
            this.labelLZ_R.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelLP_R
            // 
            this.labelLP_R.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLP_R.Image = ((System.Drawing.Image)(resources.GetObject("labelLP_R.Image")));
            this.labelLP_R.Location = new System.Drawing.Point(112, 43);
            this.labelLP_R.Name = "labelLP_R";
            this.labelLP_R.Size = new System.Drawing.Size(70, 70);
            this.labelLP_R.TabIndex = 26;
            this.labelLP_R.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPZ_P
            // 
            this.labelPZ_P.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPZ_P.Image = ((System.Drawing.Image)(resources.GetObject("labelPZ_P.Image")));
            this.labelPZ_P.Location = new System.Drawing.Point(478, 374);
            this.labelPZ_P.Name = "labelPZ_P";
            this.labelPZ_P.Size = new System.Drawing.Size(70, 70);
            this.labelPZ_P.TabIndex = 25;
            this.labelPZ_P.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelLP_P
            // 
            this.labelLP_P.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLP_P.Image = ((System.Drawing.Image)(resources.GetObject("labelLP_P.Image")));
            this.labelLP_P.Location = new System.Drawing.Point(36, 43);
            this.labelLP_P.Name = "labelLP_P";
            this.labelLP_P.Size = new System.Drawing.Size(70, 70);
            this.labelLP_P.TabIndex = 24;
            this.labelLP_P.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip.SetToolTip(this.labelLP_P, "<b>aa</b>\r\nbb");
            // 
            // labelLZ_P
            // 
            this.labelLZ_P.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelLZ_P.Image = ((System.Drawing.Image)(resources.GetObject("labelLZ_P.Image")));
            this.labelLZ_P.Location = new System.Drawing.Point(36, 374);
            this.labelLZ_P.Name = "labelLZ_P";
            this.labelLZ_P.Size = new System.Drawing.Size(70, 70);
            this.labelLZ_P.TabIndex = 23;
            this.labelLZ_P.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPP_P
            // 
            this.labelPP_P.BackColor = System.Drawing.Color.LightSlateGray;
            this.labelPP_P.Image = ((System.Drawing.Image)(resources.GetObject("labelPP_P.Image")));
            this.labelPP_P.Location = new System.Drawing.Point(478, 43);
            this.labelPP_P.Name = "labelPP_P";
            this.labelPP_P.Size = new System.Drawing.Size(70, 70);
            this.labelPP_P.TabIndex = 22;
            this.labelPP_P.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 0;
            this.toolTip.AutoPopDelay = 999999999;
            this.toolTip.InitialDelay = 0;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 0;
            // 
            // DiagnosticView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.labelDiagnostic);
            this.Name = "DiagnosticView";
            this.Size = new System.Drawing.Size(747, 1159);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.robotCanvas)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label labelDiagnostic;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label labelLZ_Z;
        public System.Windows.Forms.Label labelPP_Z;
        public System.Windows.Forms.Label labelPZ_Z;
        public System.Windows.Forms.Label labelLP_Z;
        public System.Windows.Forms.Label labelPP_ZK;
        public System.Windows.Forms.Label labelLZ_ZK;
        public System.Windows.Forms.Label labelPZ_ZK;
        public System.Windows.Forms.Label labelLP_ZK;
        public System.Windows.Forms.Label labelPP_R;
        public System.Windows.Forms.Label labelPZ_R;
        public System.Windows.Forms.Label labelLZ_R;
        public System.Windows.Forms.Label labelLP_R;
        public System.Windows.Forms.Label labelPZ_P;
        public System.Windows.Forms.Label labelLP_P;
        public System.Windows.Forms.Label labelLZ_P;
        public System.Windows.Forms.Label labelPP_P;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox robotCanvas;
    }
}
