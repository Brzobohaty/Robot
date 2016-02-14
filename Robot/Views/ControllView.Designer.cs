namespace Robot
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
            this.buttonDefaultPosition = new System.Windows.Forms.Button();
            this.buttonNarrow = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelForJoystick = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelForJoystick)).BeginInit();
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
            this.messageLabelControl.Size = new System.Drawing.Size(468, 55);
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
            this.label1.Size = new System.Drawing.Size(468, 55);
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
            this.buttonAbsolutPositioning.Size = new System.Drawing.Size(468, 50);
            this.buttonAbsolutPositioning.TabIndex = 15;
            this.buttonAbsolutPositioning.Text = "Absolutní pozicování robota";
            this.buttonAbsolutPositioning.UseVisualStyleBackColor = true;
            this.buttonAbsolutPositioning.Click += new System.EventHandler(this.buttonAbsolutPositioning_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(468, 462);
            this.panel2.TabIndex = 16;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 313F));
            this.tableLayoutPanel1.Controls.Add(this.buttonWiden, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonDefaultPosition, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonNarrow, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveUp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonMoveDown, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 220);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(468, 150);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // buttonWiden
            // 
            this.buttonWiden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonWiden.AutoSize = true;
            this.buttonWiden.Image = ((System.Drawing.Image)(resources.GetObject("buttonWiden.Image")));
            this.buttonWiden.Location = new System.Drawing.Point(83, 21);
            this.buttonWiden.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonWiden.Name = "buttonWiden";
            this.buttonWiden.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonWiden.Size = new System.Drawing.Size(69, 52);
            this.buttonWiden.TabIndex = 4;
            this.buttonWiden.UseVisualStyleBackColor = true;
            // 
            // buttonDefaultPosition
            // 
            this.buttonDefaultPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDefaultPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.buttonDefaultPosition.Location = new System.Drawing.Point(158, 23);
            this.buttonDefaultPosition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonDefaultPosition.Name = "buttonDefaultPosition";
            this.buttonDefaultPosition.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonDefaultPosition.Size = new System.Drawing.Size(190, 50);
            this.buttonDefaultPosition.TabIndex = 6;
            this.buttonDefaultPosition.Text = "Výchozí pozice";
            this.buttonDefaultPosition.UseVisualStyleBackColor = true;
            // 
            // buttonNarrow
            // 
            this.buttonNarrow.AutoSize = true;
            this.buttonNarrow.Image = ((System.Drawing.Image)(resources.GetObject("buttonNarrow.Image")));
            this.buttonNarrow.Location = new System.Drawing.Point(83, 77);
            this.buttonNarrow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNarrow.Name = "buttonNarrow";
            this.buttonNarrow.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonNarrow.Size = new System.Drawing.Size(69, 52);
            this.buttonNarrow.TabIndex = 5;
            this.buttonNarrow.UseVisualStyleBackColor = true;
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
            this.buttonMoveDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonMoveDown.Image")));
            this.buttonMoveDown.Location = new System.Drawing.Point(21, 77);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMoveDown.Size = new System.Drawing.Size(56, 52);
            this.buttonMoveDown.TabIndex = 2;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.panel1.Size = new System.Drawing.Size(468, 220);
            this.panel1.TabIndex = 14;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panelForJoystick, 1, 0);
            this.tableLayoutPanel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(468, 200);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panelForJoystick
            // 
            this.panelForJoystick.Cursor = System.Windows.Forms.Cursors.Cross;
            this.panelForJoystick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForJoystick.Location = new System.Drawing.Point(134, 0);
            this.panelForJoystick.Margin = new System.Windows.Forms.Padding(0);
            this.panelForJoystick.Name = "panelForJoystick";
            this.panelForJoystick.Size = new System.Drawing.Size(200, 200);
            this.panelForJoystick.TabIndex = 10;
            this.panelForJoystick.TabStop = false;
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
            this.Size = new System.Drawing.Size(468, 622);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelForJoystick)).EndInit();
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
        private System.Windows.Forms.PictureBox panelForJoystick;
    }
}
