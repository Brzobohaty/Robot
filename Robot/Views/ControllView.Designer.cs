﻿namespace Robot
{
    partial class ControllView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControllView));
            this.messageLabelControl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAbsolutPositioning = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonWiden = new System.Windows.Forms.Button();
            this.buttonNarrow = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonRotateLeft = new System.Windows.Forms.Button();
            this.buttonRotateRight = new System.Windows.Forms.Button();
            this.buttonDefaultPosition = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelForMoveJoystick = new System.Windows.Forms.PictureBox();
            this.panelForDirectMoveJoystick = new System.Windows.Forms.PictureBox();
            this.trackBarFrontNarrow = new System.Windows.Forms.TrackBar();
            this.trackBarBackNarrow = new System.Windows.Forms.TrackBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonTiltFront = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonTiltBack = new System.Windows.Forms.Button();
            this.buttonTiltLeft = new System.Windows.Forms.Button();
            this.buttonTiltRight = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelForMoveJoystick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelForDirectMoveJoystick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrontNarrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBackNarrow)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageLabelControl
            // 
            this.messageLabelControl.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.messageLabelControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.messageLabelControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.messageLabelControl.ForeColor = System.Drawing.Color.Red;
            this.messageLabelControl.Location = new System.Drawing.Point(0, 567);
            this.messageLabelControl.Name = "messageLabelControl";
            this.messageLabelControl.Size = new System.Drawing.Size(724, 55);
            this.messageLabelControl.TabIndex = 14;
            this.messageLabelControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GrayText;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(724, 55);
            this.label1.TabIndex = 11;
            this.label1.Text = "Ovládání";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonAbsolutPositioning
            // 
            this.buttonAbsolutPositioning.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonAbsolutPositioning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonAbsolutPositioning.ForeColor = System.Drawing.Color.Black;
            this.buttonAbsolutPositioning.Location = new System.Drawing.Point(0, 517);
            this.buttonAbsolutPositioning.Name = "buttonAbsolutPositioning";
            this.buttonAbsolutPositioning.Size = new System.Drawing.Size(724, 50);
            this.buttonAbsolutPositioning.TabIndex = 15;
            this.buttonAbsolutPositioning.Text = "Absolutní pozicování robota";
            this.buttonAbsolutPositioning.UseVisualStyleBackColor = true;
            this.buttonAbsolutPositioning.Click += new System.EventHandler(this.buttonAbsolutPositioning_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.trackBarBackNarrow);
            this.panel2.Controls.Add(this.trackBarFrontNarrow);
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(724, 462);
            this.panel2.TabIndex = 16;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 509F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.buttonWiden, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonNarrow, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveUp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveDown, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonRotateLeft, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonRotateRight, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonDefaultPosition, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonStop, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 220);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(724, 150);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // buttonWiden
            // 
            this.buttonWiden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonWiden.AutoSize = true;
            this.buttonWiden.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonWiden.Image = ((System.Drawing.Image)(resources.GetObject("buttonWiden.Image")));
            this.buttonWiden.Location = new System.Drawing.Point(83, 21);
            this.buttonWiden.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonWiden.Name = "buttonWiden";
            this.buttonWiden.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonWiden.Size = new System.Drawing.Size(69, 52);
            this.buttonWiden.TabIndex = 4;
            this.buttonWiden.UseVisualStyleBackColor = false;
            // 
            // buttonNarrow
            // 
            this.buttonNarrow.AutoSize = true;
            this.buttonNarrow.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonNarrow.Image = ((System.Drawing.Image)(resources.GetObject("buttonNarrow.Image")));
            this.buttonNarrow.Location = new System.Drawing.Point(83, 77);
            this.buttonNarrow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNarrow.Name = "buttonNarrow";
            this.buttonNarrow.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNarrow.Size = new System.Drawing.Size(69, 52);
            this.buttonNarrow.TabIndex = 5;
            this.buttonNarrow.UseVisualStyleBackColor = false;
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveUp.AutoSize = true;
            this.buttonMoveUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonMoveUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonMoveUp.Image")));
            this.buttonMoveUp.Location = new System.Drawing.Point(21, 21);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMoveUp.Size = new System.Drawing.Size(56, 52);
            this.buttonMoveUp.TabIndex = 3;
            this.buttonMoveUp.UseVisualStyleBackColor = false;
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMoveDown.AutoSize = true;
            this.buttonMoveDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonMoveDown.Image")));
            this.buttonMoveDown.Location = new System.Drawing.Point(21, 77);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMoveDown.Size = new System.Drawing.Size(56, 52);
            this.buttonMoveDown.TabIndex = 2;
            this.buttonMoveDown.UseVisualStyleBackColor = false;
            // 
            // buttonRotateLeft
            // 
            this.buttonRotateLeft.AutoSize = true;
            this.buttonRotateLeft.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonRotateLeft.Image = ((System.Drawing.Image)(resources.GetObject("buttonRotateLeft.Image")));
            this.buttonRotateLeft.Location = new System.Drawing.Point(158, 77);
            this.buttonRotateLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRotateLeft.Name = "buttonRotateLeft";
            this.buttonRotateLeft.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRotateLeft.Size = new System.Drawing.Size(54, 52);
            this.buttonRotateLeft.TabIndex = 16;
            this.buttonRotateLeft.UseVisualStyleBackColor = false;
            this.buttonRotateLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonRotateLeft_MouseDown);
            this.buttonRotateLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonRotateLeft_MouseUp);
            // 
            // buttonRotateRight
            // 
            this.buttonRotateRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRotateRight.AutoSize = true;
            this.buttonRotateRight.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonRotateRight.Image = ((System.Drawing.Image)(resources.GetObject("buttonRotateRight.Image")));
            this.buttonRotateRight.Location = new System.Drawing.Point(158, 21);
            this.buttonRotateRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRotateRight.Name = "buttonRotateRight";
            this.buttonRotateRight.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonRotateRight.Size = new System.Drawing.Size(54, 52);
            this.buttonRotateRight.TabIndex = 16;
            this.buttonRotateRight.UseVisualStyleBackColor = false;
            this.buttonRotateRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonRotateRight_MouseDown);
            this.buttonRotateRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonRotateRight_MouseUp);
            // 
            // buttonDefaultPosition
            // 
            this.buttonDefaultPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDefaultPosition.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonDefaultPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonDefaultPosition.Location = new System.Drawing.Point(218, 23);
            this.buttonDefaultPosition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonDefaultPosition.Name = "buttonDefaultPosition";
            this.buttonDefaultPosition.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonDefaultPosition.Size = new System.Drawing.Size(190, 50);
            this.buttonDefaultPosition.TabIndex = 6;
            this.buttonDefaultPosition.Text = "Výchozí pozice";
            this.buttonDefaultPosition.UseVisualStyleBackColor = false;
            // 
            // buttonStop
            // 
            this.buttonStop.AutoSize = true;
            this.buttonStop.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonStop.Image = ((System.Drawing.Image)(resources.GetObject("buttonStop.Image")));
            this.buttonStop.Location = new System.Drawing.Point(218, 77);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStop.Size = new System.Drawing.Size(56, 52);
            this.buttonStop.TabIndex = 16;
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonStop_MouseDown);
            this.buttonStop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonStop_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.panel1.Size = new System.Drawing.Size(724, 220);
            this.panel1.TabIndex = 14;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panelForMoveJoystick, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelForDirectMoveJoystick, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(724, 200);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panelForMoveJoystick
            // 
            this.panelForMoveJoystick.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelForMoveJoystick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForMoveJoystick.Location = new System.Drawing.Point(462, 0);
            this.panelForMoveJoystick.Margin = new System.Windows.Forms.Padding(0);
            this.panelForMoveJoystick.Name = "panelForMoveJoystick";
            this.panelForMoveJoystick.Size = new System.Drawing.Size(200, 200);
            this.panelForMoveJoystick.TabIndex = 11;
            this.panelForMoveJoystick.TabStop = false;
            this.panelForMoveJoystick.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelForJoystick_MouseDown);
            this.panelForMoveJoystick.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelForJoystick_MouseMove);
            this.panelForMoveJoystick.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelForJoystick_MouseUp);
            // 
            // panelForDirectMoveJoystick
            // 
            this.panelForDirectMoveJoystick.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelForDirectMoveJoystick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForDirectMoveJoystick.Location = new System.Drawing.Point(62, 0);
            this.panelForDirectMoveJoystick.Margin = new System.Windows.Forms.Padding(0);
            this.panelForDirectMoveJoystick.Name = "panelForDirectMoveJoystick";
            this.panelForDirectMoveJoystick.Size = new System.Drawing.Size(200, 200);
            this.panelForDirectMoveJoystick.TabIndex = 10;
            this.panelForDirectMoveJoystick.TabStop = false;
            // 
            // trackBarFrontNarrow
            // 
            this.trackBarFrontNarrow.Location = new System.Drawing.Point(21, 400);
            this.trackBarFrontNarrow.Maximum = 100;
            this.trackBarFrontNarrow.Name = "trackBarFrontNarrow";
            this.trackBarFrontNarrow.Size = new System.Drawing.Size(253, 56);
            this.trackBarFrontNarrow.TabIndex = 16;
            // 
            // trackBarBackNarrow
            // 
            this.trackBarBackNarrow.Location = new System.Drawing.Point(309, 400);
            this.trackBarBackNarrow.Maximum = 100;
            this.trackBarBackNarrow.Name = "trackBarBackNarrow";
            this.trackBarBackNarrow.Size = new System.Drawing.Size(253, 56);
            this.trackBarBackNarrow.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonTiltRight);
            this.panel3.Controls.Add(this.buttonTiltLeft);
            this.panel3.Controls.Add(this.buttonTiltBack);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.buttonTiltFront);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(265, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(194, 194);
            this.panel3.TabIndex = 12;
            // 
            // buttonTiltFront
            // 
            this.buttonTiltFront.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTiltFront.AutoSize = true;
            this.buttonTiltFront.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonTiltFront.Image = ((System.Drawing.Image)(resources.GetObject("buttonTiltFront.Image")));
            this.buttonTiltFront.Location = new System.Drawing.Point(69, 29);
            this.buttonTiltFront.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltFront.Name = "buttonTiltFront";
            this.buttonTiltFront.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltFront.Size = new System.Drawing.Size(56, 52);
            this.buttonTiltFront.TabIndex = 4;
            this.buttonTiltFront.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 27);
            this.label2.TabIndex = 5;
            this.label2.Text = "Naklonění";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonTiltBack
            // 
            this.buttonTiltBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTiltBack.AutoSize = true;
            this.buttonTiltBack.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonTiltBack.Image = ((System.Drawing.Image)(resources.GetObject("buttonTiltBack.Image")));
            this.buttonTiltBack.Location = new System.Drawing.Point(69, 140);
            this.buttonTiltBack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltBack.Name = "buttonTiltBack";
            this.buttonTiltBack.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltBack.Size = new System.Drawing.Size(56, 52);
            this.buttonTiltBack.TabIndex = 6;
            this.buttonTiltBack.UseVisualStyleBackColor = false;
            // 
            // buttonTiltLeft
            // 
            this.buttonTiltLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTiltLeft.AutoSize = true;
            this.buttonTiltLeft.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonTiltLeft.Image = ((System.Drawing.Image)(resources.GetObject("buttonTiltLeft.Image")));
            this.buttonTiltLeft.Location = new System.Drawing.Point(3, 85);
            this.buttonTiltLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltLeft.Name = "buttonTiltLeft";
            this.buttonTiltLeft.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltLeft.Size = new System.Drawing.Size(56, 52);
            this.buttonTiltLeft.TabIndex = 7;
            this.buttonTiltLeft.UseVisualStyleBackColor = false;
            // 
            // buttonTiltRight
            // 
            this.buttonTiltRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTiltRight.AutoSize = true;
            this.buttonTiltRight.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonTiltRight.Image = ((System.Drawing.Image)(resources.GetObject("buttonTiltRight.Image")));
            this.buttonTiltRight.Location = new System.Drawing.Point(135, 85);
            this.buttonTiltRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltRight.Name = "buttonTiltRight";
            this.buttonTiltRight.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTiltRight.Size = new System.Drawing.Size(56, 52);
            this.buttonTiltRight.TabIndex = 8;
            this.buttonTiltRight.UseVisualStyleBackColor = false;
            // 
            // ControllView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.buttonAbsolutPositioning);
            this.Controls.Add(this.messageLabelControl);
            this.Controls.Add(this.label1);
            this.Name = "ControllView";
            this.Size = new System.Drawing.Size(724, 622);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelForMoveJoystick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelForDirectMoveJoystick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrontNarrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBackNarrow)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label messageLabelControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAbsolutPositioning;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonWiden;
        private System.Windows.Forms.Button buttonDefaultPosition;
        private System.Windows.Forms.Button buttonNarrow;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox panelForDirectMoveJoystick;
        private System.Windows.Forms.Button buttonRotateLeft;
        private System.Windows.Forms.Button buttonRotateRight;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.PictureBox panelForMoveJoystick;
        private System.Windows.Forms.TrackBar trackBarBackNarrow;
        private System.Windows.Forms.TrackBar trackBarFrontNarrow;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonTiltBack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonTiltFront;
        private System.Windows.Forms.Button buttonTiltRight;
        private System.Windows.Forms.Button buttonTiltLeft;
    }
}
